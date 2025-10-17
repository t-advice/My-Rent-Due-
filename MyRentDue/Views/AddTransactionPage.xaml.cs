using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MyRentDue.Models;
using MyRentDue.Views;
using Microsoft.Maui.Controls;

namespace MyRentDue.Views;

[QueryProperty(nameof(TenantIdQuery), "tenantId")]
[QueryProperty(nameof(TransactionIdQuery), "transactionId")]
public partial class AddTransactionPage : ContentPage
{
    private List<Tenant> _tenant = new();
    private int? _prefillTenantId;
    private int? _editingTransactionId;

    public string TenantIdQuery
    {
        get => _prefillTenantId?.ToString() ?? string.Empty;
        set
        {
            if (int.TryParse(value, out var id))
                _prefillTenantId = id;
        }
    }

    public string TransactionIdQuery
    {
        get => _editingTransactionId?.ToString() ?? string.Empty;
        set
        {
            if (int.TryParse(value, out var id))
                _editingTransactionId = id;
        }
    }

    public AddTransactionPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var tenantPicker = this.FindByName<Picker>("TenantPicker");
        var tenantLabel = this.FindByName<Label>("TenantLabelHeader");
        var amountEntry = this.FindByName<Entry>("AmountEntry");
        var datePicker = this.FindByName<DatePicker>("DatePicker");
        var notesEntry = this.FindByName<Entry>("NotesEntry");
        var deleteButton = this.FindByName<Button>("DeleteButton");
        var saveButton = this.FindByName<Button>("SaveButton");

        _tenant = await App.Database.GetTenantsAsync();
        tenantPicker.ItemsSource = _tenant;
        tenantPicker.ItemDisplayBinding = new Binding("FullName");

        // If a tenant id was provided via query, pre-select and hide picker
        if (_prefillTenantId.HasValue)
        {
            var idx = _tenant.FindIndex(t => t.Id == _prefillTenantId.Value);
            if (idx >= 0)
            {
                tenantPicker.SelectedIndex = idx;
                tenantPicker.IsVisible = false;
                if (tenantLabel != null) tenantLabel.IsVisible = false;

                // show chosen tenant as read-only label
                var chosen = _tenant[idx];
                var lbl = new Label { Text = chosen.FullName, FontAttributes = FontAttributes.Bold };
                if (tenantPicker.Parent is VerticalStackLayout parentLayout)
                {
                    parentLayout.Children.Add(lbl);
                }
            }
        }

        // If editing an existing transaction, load it
        if (_editingTransactionId.HasValue)
        {
            var tx = await App.Database.GetTransactionByIdAsync(_editingTransactionId.Value);
            if (tx != null)
            {
                // pre-select tenant
                var idx = _tenant.FindIndex(t => t.Id == tx.TenantId);
                if (idx >= 0)
                    tenantPicker.SelectedIndex = idx;

                amountEntry.Text = tx.Amount.ToString(CultureInfo.InvariantCulture);
                datePicker.Date = tx.Date;
                notesEntry.Text = tx.Notes;

                // show delete button when editing
                if (deleteButton != null) deleteButton.IsVisible = true;
                if (saveButton != null) saveButton.Text = "Update";
            }
        }
    }

    private async void OnSaveTransactionClicked(object sender, EventArgs e)
    {
        var tenantPicker = this.FindByName<Picker>("TenantPicker");
        var amountEntry = this.FindByName<Entry>("AmountEntry");
        var datePicker = this.FindByName<DatePicker>("DatePicker");
        var notesEntry = this.FindByName<Entry>("NotesEntry");

        if (tenantPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Missing info", "Please select a tenant.", "OK");
            return;
        }

        decimal amount = 0;
        if (!string.IsNullOrWhiteSpace(amountEntry.Text))
        {
            var normalized = amountEntry.Text.Replace(',', '.');
            decimal.TryParse(normalized, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out amount);
        }

        var selectedTenant = _tenant[tenantPicker.SelectedIndex];

        // if editing, get original transaction to adjust AmountPaid correctly
        PaymentTransaction? original = null;
        if (_editingTransactionId.HasValue)
        {
            original = await App.Database.GetTransactionByIdAsync(_editingTransactionId.Value);
        }

        var tx = new PaymentTransaction
        {
            Id = _editingTransactionId ?? 0,
            TenantId = selectedTenant.Id,
            Amount = amount,
            Date = datePicker.Date,
            Notes = notesEntry.Text?.Trim() ?? string.Empty
        };

        await App.Database.SaveTransactionAsync(tx);

        // Update tenant.AmountPaid: if editing, remove original amount then add new; otherwise just add
        if (amount >= 0)
        {
            if (original != null)
            {
                selectedTenant.AmountPaid -= original.Amount;
            }

            selectedTenant.AmountPaid += amount;
            await App.Database.SaveTenantAsync(selectedTenant);
        }

        await DisplayAlert("Saved", "Transaction recorded.", "OK");
        await Shell.Current.GoToAsync(".."); // back
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (!_editingTransactionId.HasValue)
            return;

        var tx = await App.Database.GetTransactionByIdAsync(_editingTransactionId.Value);
        if (tx == null)
            return;

        var confirmed = await DisplayAlert("Confirm", "Delete this transaction?", "Yes", "No");
        if (!confirmed) return;

        // subtract amount from tenant and save
        var tenants = await App.Database.GetTenantsAsync();
        var tenant = tenants.FirstOrDefault(t => t.Id == tx.TenantId);
        if (tenant != null)
        {
            tenant.AmountPaid -= tx.Amount;
            if (tenant.AmountPaid < 0) tenant.AmountPaid = 0;
            await App.Database.SaveTenantAsync(tenant);
        }

        await App.Database.DeleteTransactionAsync(tx);
        await DisplayAlert("Deleted", "Transaction removed.", "OK");
        await Shell.Current.GoToAsync("..");
    }
}