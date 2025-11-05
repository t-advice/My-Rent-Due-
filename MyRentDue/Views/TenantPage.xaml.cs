using MyRentDue.Models;
using MyRentDue.Services;
using Microsoft.Maui.Controls;
using MyRentDue.Views;

namespace MyRentDue.Views;

public partial class TenantPage : ContentPage
{
    public TenantPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTenants();
    }

    private async Task LoadTenants()
    {
        var tenants = await App.Database.GetTenantsAsync();

        var list = tenants.Select(t =>
        {
            // calculate remaining rent using AmountPaid
            var remaining = t.RentPerMonth - t.AmountPaid;
            if (remaining < 0) remaining = 0;

            var tenantView = new TenantView
            {
                Id = t.Id,
                FullName = $"{t.FirstName} {t.LastName}",
                Email = t.Email,
                RentDisplay = $"R {remaining:F2} remaining",
                IsPaid = t.IsPaid,
                RentPerMonth = t.RentPerMonth
            };

            // Update payment status based on amount paid vs rent due
            tenantView.UpdatePaymentStatus(t.AmountPaid, t.RentPerMonth);

            return tenantView;
        }).ToList();

        TenantsList.ItemsSource = list;
    }

    private async void OnAddTenantClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTenantPage));
    }

    // Navigate to details when a tenant is selected
    private async void OnTenantSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        if (e.CurrentSelection[0] is TenantView tv)
        {
            // navigate to TenantDetailsPage and pass tenant id as query parameter
            await Shell.Current.GoToAsync($"{nameof(TenantDetailsPage)}?tenantId={tv.Id}");
        }

        // clear selection so tapping the same item later still triggers selection
        ((CollectionView)sender).SelectedItem = null;
    }
}
