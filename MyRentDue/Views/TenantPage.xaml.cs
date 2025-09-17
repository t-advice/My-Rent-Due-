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

        var list = tenants.Select(t => new TenantView
        {
            Id = t.Id,
            
            FullName = $"{t.FirstName} {t.LastName}",
            Email = t.Email,
            RentDisplay = $"R {t.RentPerMonth:F2} / mo",
            PaymentStatus = t.IsPaid ? "Paid" : "Unpaid"
        }).ToList();

        TenantsList.ItemsSource = list;
    }

    private async void OnAddTenantClicked(object sender, EventArgs e)
    {
        // Navigate to AddTenantPage (stub for now)
        await Shell.Current.GoToAsync(nameof(AddTenantPage));
    }
}
