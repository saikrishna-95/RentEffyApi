using Renteffy.Domain.Services.PersistanceInterfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.PersistanceInterfaces.Services;

namespace Renteffy.Integration.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly Cloudinary _cloudinary;

        public ReceiptService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ReceiptResultDto> GenerateReceiptAsync(BookingReceiptDto booking)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(),"TempReceipts");

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var localPath = Path.Combine(tempFolder,$"Receipt_{booking.BookingId}.pdf");

            // PDF GENERATE

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Text("PG BOOKING RECEIPT").FontSize(20).Bold();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text(
                            $"Booking Code : {booking.BookingCode}");

                        col.Item().Text(
                            $"User Name : {booking.UserName}");

                        col.Item().Text(
                            $"PG Name : {booking.PgName}");

                        col.Item().Text(
                            $"Amount : ₹{booking.Amount}");

                        col.Item().Text(
                            $"From Date : {booking.FromDate:dd-MM-yyyy}");

                        col.Item().Text(
                            $"To Date : {booking.ToDate:dd-MM-yyyy}");

                        col.Item().Text(
                            $"Generated On : {DateTime.Now}");
                    });
                });

            }).GeneratePdf(localPath);

            // UPLOAD PDF TO CLOUDINARY

            var pdfBytes = await File.ReadAllBytesAsync(localPath);
            using var stream = new MemoryStream(pdfBytes);
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription($"Receipt_{booking.BookingId}.pdf",stream),
                PublicId = $"receipts/Receipt_{booking.BookingId}",
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(
                    uploadResult.Error.Message);
            }
            stream.Close();

            // DELETE TEMP FILE

            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }

            return new ReceiptResultDto
            {
                LocalPath = localPath,
                CloudUrl = uploadResult.SecureUrl.ToString()
            };
        }
    }
}
