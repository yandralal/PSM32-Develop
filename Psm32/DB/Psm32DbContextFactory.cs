using Microsoft.EntityFrameworkCore;

namespace Psm32.DB;

public class Psm32DbContextFactory
{
    //private readonly string _connectionString;
    private readonly DbContextOptions _options;

    public Psm32DbContextFactory(DbContextOptions options)
    {
       // _connectionString = connectionString;
        _options = options;
    }

    public Psm32DbContext CreateDbContext()
    {
        return new Psm32DbContext(_options);
    }

}
