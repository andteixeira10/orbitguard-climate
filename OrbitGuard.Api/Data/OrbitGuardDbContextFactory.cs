using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrbitGuard.Api.Data;

public class OrbitGuardDbContextFactory : IDesignTimeDbContextFactory<OrbitGuardDbContext>
{
    public OrbitGuardDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrbitGuardDbContext>();

        optionsBuilder.UseMySql(
            "server=localhost;port=3306;database=orbitguard_db;user=orbitguard_user;password=orbitguard_pass",
            new MySqlServerVersion(new Version(8, 0, 36))
        );

        return new OrbitGuardDbContext(optionsBuilder.Options);
    }
}