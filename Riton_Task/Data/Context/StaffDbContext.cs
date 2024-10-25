using Microsoft.EntityFrameworkCore;
using Riton_Task.Data.Models;

namespace Riton_Task.Data.Context;

public class StaffDbContext : DbContext
{
    public StaffDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>().HasIndex(x => x.PersonalId).IsUnique();
    }

    public DbSet<Staff> Staffs { get; set; }
    public DbSet<ProcessedFiles> ProcessedFileNames { get; set; }
}