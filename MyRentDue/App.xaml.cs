using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MyRentDue.Services;

namespace MyRentDue
{
    public partial class App : Application
    {
        public static MyRentDueDatabase Database { get; } = new();

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        

    }
}