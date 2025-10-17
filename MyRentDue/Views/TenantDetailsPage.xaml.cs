using MyRentDue.Models;
using Microsoft.Maui.Controls;

namespace MyRentDue.Views;

[QueryProperty(nameof(TenantIdQuery), "tenantId")]
public partial class TenantDetailsPage : ContentPage
{
    private int _tenantId;

    private Tenant? _tenant;

    public string TenantIdQuery
    {
        get => _tenantId.ToString();
        set
        {
            if (int.TryParse(value, out var id))
                _tenantId = id;
        }
    }

    public TenantDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTenant();
    }

    private async Task LoadTenant()
    {
        var tenants = await App.Database.GetTenantsAsync();
        _tenant = tenants.FirstOrDefault(t => t.Id == _tenantId);
        if (_tenant == null)
            return;

        NameLabel.Text = $"{_tenant.FirstName} {_tenant.LastName}";
        EmailLabel.Text = _tenant.Email;
        PhoneLabel.Text = _tenant.Phone ?? _tenant.CellNumber ?? string.Empty;
        RentPerMonthLabel.Text = $"Monthly rent: R {_tenant.RentPerMonth:F2}";
        AmountPaidLabel.Text = $"Amount paid: R {_tenant.AmountPaid:F2}";

        var remaining = _tenant.RentPerMonth - _tenant.AmountPaid;
        if (remaining < 0) remaining = 0;
        RemainingLabel.Text = $"Remaining: R {remaining:F2}";

        var txs = await App.Database.GetTransactionsForTenantAsync(_tenant.Id);
        var views = txs.Select(h => new TransactionView
        {
            Id = h.Id,
            TenantName = "",
            AmountFormatted = $"R {h.Amount:F2}",
            DateFormatted = h.Date.ToString("yyyy-MM-dd"),
            Notes = h.Notes
        }).ToList();

        TransactionsList.ItemsSource = views;
    }

    private async void OnAddPaymentClicked(object sender, EventArgs e)
    {
        // open AddTransactionPage and pre-select this tenant
        await Shell.Current.GoToAsync($"{nameof(AddTransactionPage)}?tenantId={_tenantId}");
    }
}