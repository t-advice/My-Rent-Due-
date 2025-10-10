using Microsoft.Maui.Controls;
using MyRentDue.Models;
using MyRentDue.Services;
using Microsoft.Maui.Controls;
using MyRentDue.ViewModels;

namespace MyRentDue.Views;

public partial class AddTenantPage : ContentPage
{
    public AddTenantPage()
    {
        InitializeComponent();
    }

    private async void OnSaveTenantClicked(object sender, EventArgs e)
    {
        // Validate all required fields
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(RentEntry.Text))
        {
            await DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        // Parse Rent
        if (!int.TryParse(RentEntry.Text, out int rent) || rent <= 0)
        {
            await DisplayAlert("Error", "Rent must be a valid number (e.g., 1, 2, 3...).", "OK");
            return;
        }

        // Create tenant object
        var tenant = new Tenant
        {

            

            FirstName = FirstNameEntry.Text.Trim(),
            LastName = LastNameEntry.Text.Trim(),

            Email = EmailEntry.Text.Trim(),
            Phone = PhoneEntry.Text?.Trim() ?? string.Empty,
            RentPerMonth = rent
        };

        // Save to database
        await App.Database.SaveTenantAsync(tenant);

        await DisplayAlert("Success", "Tenant saved!", "OK");

        // Navigate back to tenant list
        await Shell.Current.GoToAsync("..");
    }

    private async void OnGenerateContractClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Contract", "Generate Contract button clicked.", "OK");
    }
}




