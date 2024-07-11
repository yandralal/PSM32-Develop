using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Psm32.DB;

public class Psm32DesignTimeDbContextFactory: IDesignTimeDbContextFactory<Psm32DbContext>
{
    public Psm32DbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder().UseSqlite("Data Source=powerstim32.db").Options; //TODO: use proper path
        return new Psm32DbContext(options);
    }
}
