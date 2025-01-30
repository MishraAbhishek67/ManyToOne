using Mapping_Many_to_one.Entity;
using Microsoft.EntityFrameworkCore;

namespace Mapping_Many_to_one.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Company to Department (One-to-Many)
                 modelBuilder.Entity<Department>()
                .HasOne(d => d.Company) // A Department has one Company
                .WithMany(c => c.Departments) // A Company has many Departments
                .HasForeignKey(d => d.CompanyId) // Foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete


            // Department to Employee (One-to-Many)
                 modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department) // An Employee has one Department
                .WithMany(d => d.Employees) // A Department has many Employees
                .HasForeignKey(e => e.DepartmentId) // Foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        }
    }
}

