using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Microting.KanbanBase.Infrastructure.Data.Factories;

public class KanbanPnContextFactory : IDesignTimeDbContextFactory<KanbanPnDbContext>
{
    public KanbanPnDbContext CreateDbContext(string[] args)
    {
        var defaultCs = "Server=localhost;port=3306;Database=kanban-pn;user=root;password=secretpassword;Convert Zero Datetime=true;";
        var optionsBuilder = new DbContextOptionsBuilder<KanbanPnDbContext>();

        optionsBuilder.UseMySql(args.Any() ? args[0] : defaultCs, new MariaDbServerVersion(
            ServerVersion.AutoDetect(args.Any() ? args[0] : defaultCs)), mySqlOptionsAction: builder =>
        {
            builder.EnableRetryOnFailure();
        });

        return new KanbanPnDbContext(optionsBuilder.Options);
        // dotnet ef migrations add InitialCreate --project Microting.KanbanBase --startup-project DBMigrator
    }
}
