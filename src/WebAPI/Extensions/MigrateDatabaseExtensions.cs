using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Extensions;

public static class MigrateDatabaseExtension
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var ctx = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
        using var transactionCtx = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();

        ctx.Database.Migrate();
        transactionCtx.Database.Migrate();
    }
}