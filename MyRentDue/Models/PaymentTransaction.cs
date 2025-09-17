using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MyRentDue.Models
{
    [SQLite.Table("Transactions")]
    public class PaymentTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        
        public string Notes { get; set; } = string.Empty;
        
        
    }
}
