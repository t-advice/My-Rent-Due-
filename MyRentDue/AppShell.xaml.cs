namespace MyRentDue
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.AddTenantPage), typeof(Views.AddTenantPage));
            Routing.RegisterRoute(nameof(Views.TransactionsPage), typeof(Views.TransactionsPage));
            Routing.RegisterRoute(nameof(Views.AddTransactionPage), typeof(Views.AddTransactionPage));
            // register tenant details page so we can pass tenantId as query
            Routing.RegisterRoute(nameof(Views.TenantDetailsPage), typeof(Views.TenantDetailsPage));
        }
    }
}
