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

        public async Task<string> GenerateReceiptAsync(BookingReceiptDto booking)
        {
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

            await using var stream =
                File.OpenRead(localPath);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(
                    Path.GetFileName(localPath),
                    stream),

                Folder = $"receipts/{booking.BookingId}",

                PublicId = $"Receipt_{booking.BookingId}"
            };

            var uploadResult =
                await _cloudinary.UploadAsync(uploadParams);

            // DELETE TEMP FILE

            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }

            return uploadResult.SecureUrl.ToString();
        }
    }
}
