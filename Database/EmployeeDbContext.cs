using EmployeeApi.Models;

using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Database
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> DbContextOptions)
            : base(DbContextOptions)
        {

        }
    }
}
