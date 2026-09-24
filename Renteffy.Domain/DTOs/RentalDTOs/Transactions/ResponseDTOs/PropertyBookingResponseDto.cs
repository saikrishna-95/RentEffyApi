using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs
{
    public class PropertyBookingResponseDto
    {
        public int PropertyBookingID { get; set; }
        public int PropertyID { get; set; }
        public int UserID { get; set; }
        public int OwnerID { get; set; }
        public string? PropertyName { get; set; }
        public string? OwnerName { get; set; }
        public string? UserName { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal BookingAmount { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal MaintenanceAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentStatus { get; set; }
        public string? PaymentStatusName { get; set; }
        public int BookingStatus { get; set; }
        public string? BookingStatusName { get; set; }
        public string? RazorpayOrderID { get; set; }
        public string? RazorpayPaymentID { get; set; }
        public DateTime? PaymentConfirmedAt { get; set; }
        public DateTime? BookingConfirmedAt { get; set; }
        public bool UserCancellationAllowed { get; set; }
        public DateTime? UserCancelEligibleFrom { get; set; }
        public DateTime? UserCancelEligibleUntil { get; set; }
        public decimal UserRefundPercentage { get; set; }
        public decimal UserDeductionPercentage { get; set; }
        public bool OwnerCancellationAllowed { get; set; }
        public DateTime? OwnerCancelEligibleFrom { get; set; }
        public DateTime? OwnerCancelEligibleUntil { get; set; }
        public decimal OwnerRefundPercentage { get; set; }
        public decimal OwnerDeductionPercentage { get; set; }
        public decimal? RefundAmount { get; set; }
        public decimal? DeductionAmount { get; set; }
        public string? CancelledBy { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public bool CanUserCancel { get; set; }
        public bool CanOwnerCancel { get; set; }
    }
}
