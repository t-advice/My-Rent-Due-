using MyRentDue.Models;
using MyRentDue.Views;

namespace MyRentDue.Views;


public partial class AddTransactionPage : ContentPage
{
    private List<Tenant> _tenant = new();


    public AddTransactionPage()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _tenant = await App.Database.GetTenantsAsync();
        TenantPicker.ItemsSource = _tenant;
        TenantPicker.ItemDisplayBinding = new Binding("FullName");
    }


    private async void OnSaveTransactionClicked(object sender, EventArgs e)
    {
        if (TenantPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Missing info", "Please select a tenant.", "OK");
            return;
        }


        decimal amount = 0;
        if (!string.IsNullOrWhiteSpace(AmountEntry.Text))
        {
            var normalized = AmountEntry.Text.Replace(',', '.');
            decimal.TryParse(normalized, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out amount);
        }


        var selectedTenant = _tenant[TenantPicker.SelectedIndex];


        var tx = new PaymentTransaction
        {
            TenantId = selectedTenant.Id,
            Amount = amount,
            Date = DatePicker.Date,
            Notes = NotesEntry.Text?.Trim() ?? string.Empty
        };


        await App.Database.SaveTransactionAsync(tx);
        await DisplayAlert("Saved", "Transaction recorded.", "OK");
        await Shell.Current.GoToAsync(".."); // back to TransactionsPage
    }
}