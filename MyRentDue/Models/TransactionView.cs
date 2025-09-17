using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRentDue.Models;

public class TransactionView
{
    public int Id { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string AmountFormatted { get; set; } = string.Empty;
    public string DateFormatted { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}