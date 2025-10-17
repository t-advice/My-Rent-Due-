using MyRentDue.Models;
using MyRentDue.Views;
using System.Collections.Generic;
using System;
using System.Linq;


namespace MyRentDue.Views;


public partial class TransactionsPage : ContentPage
{
    public TransactionsPage()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTransactions();
    }


    private async Task LoadTransactions()
    {
        var tenants = await App.Database.GetTenantsAsync();
        var txs = new List<TransactionView>();

        foreach (var t in tenants)
        {
            var history = await App.Database.GetTransactionsForTenantAsync(t.Id);
            txs.AddRange(history.Select(h => new TransactionView
            {
                Id = h.Id,
                TenantName = $"{t.FirstName} {t.LastName}",
                AmountFormatted = $"R {h.Amount:F2}",
                DateFormatted = h.Date.ToString("yyyy-MM-dd"),
                Notes = h.Notes
            }));
        }

        TransactionsList.ItemsSource = txs
            .OrderByDescending(x => x.DateFormatted)
            .ToList();
    }


    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTransactionPage));
    }

    private async void OnTransactionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        if (e.CurrentSelection[0] is TransactionView tv)
        {
            // Navigate to AddTransactionPage in edit mode
            await Shell.Current.GoToAsync($"{nameof(AddTransactionPage)}?transactionId={tv.Id}");
        }

        ((CollectionView)sender).SelectedItem = null;
    }
}