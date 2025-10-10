using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;   

namespace MyRentDue.Models
{
    [Table("Tenants")]
    public class Tenant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CellNumber { get; set; } = string.Empty;

        public decimal RentPerMonth { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FullName => $"{FirstName}{LastName}".Trim();

        public string? Phone { get; internal set; }

        public string Address { get; set; }
        public string ProfileImagePath { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
