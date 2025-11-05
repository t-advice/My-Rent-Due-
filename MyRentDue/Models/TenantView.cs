using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRentDue.Models;

public class TenantView
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RentDisplay { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public object RentPerMonth { get; internal set; }
    public bool IsPaid { get; internal set; }

    /// <summary>
    /// Updates the payment status based on amount paid vs rent due
    /// </summary>
    public void UpdatePaymentStatus(decimal amountPaid, decimal rentPerMonth)
    {
        IsPaid = amountPaid >= rentPerMonth;
        PaymentStatus = IsPaid ? "Paid" : "Unpaid";
    }
}



