using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyRentDue.Models;

namespace MyRentDue.ViewModels
{
    public class TenantViewModel : INotifyPropertyChanged
    {
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string phone = string.Empty;
        private string rentPerMonthText = string.Empty;
        private bool hasPaid;

        // Tenant properties
        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); }
        }

        // Rent as string to fix binding issues
        public string RentPerMonthText
        {
            get => rentPerMonthText;
            set { rentPerMonthText = value; OnPropertyChanged(); }
        }

        public bool HasPaid
        {
            get => hasPaid;
            set { hasPaid = value; OnPropertyChanged(); }
        }

        public int RentPerMonth { get; internal set; }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


