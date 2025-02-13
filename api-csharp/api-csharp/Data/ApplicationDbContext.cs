using Microsoft.EntityFrameworkCore;
using CsvImportApi.Models;

namespace CsvImportApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Profissao)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}