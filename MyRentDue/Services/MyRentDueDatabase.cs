using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MyRentDue.Models;
using System.Runtime.CompilerServices;

namespace MyRentDue.Services;

public class MyRentDueDatabase
{
    private SQLiteAsyncConnection? _db;

    public async Task Init()
    {
        if (_db != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyRentDue.db3");
        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<Tenant>();
        await _db.CreateTableAsync<PaymentTransaction>();
    }

    // ✅ Save a tenant
    public async Task SaveTenantAsync(Tenant tenant)
    {
        await Init();
        if (tenant.Id != 0)
            await _db!.UpdateAsync(tenant);
        else
            await _db!.InsertAsync(tenant);
    }

    // ✅ Get all tenants
    public async Task<List<Tenant>> GetTenantsAsync()
    {
        await Init();
        return await _db!.Table<Tenant>().OrderBy(t => t.LastName).ThenBy(t => t.FirstName).ToListAsync();
    }

    // ✅ Save a transaction
    public async Task SaveTransactionAsync(PaymentTransaction tx)
    {
        await Init();
        if (tx.Id != 0)
            await _db!.UpdateAsync(tx);
        else
            await _db!.InsertAsync(tx);
    }

    // ✅ Get all transactions for a tenant
    public async Task<List<PaymentTransaction>> GetTransactionsForTenantAsync(int tenantId)
    {
        await Init();
        return await _db!.Table<PaymentTransaction>()
                         .Where(t => t.TenantId == tenantId)
                         .OrderByDescending(t => t.Date)
                         .ToListAsync();
    }
}

