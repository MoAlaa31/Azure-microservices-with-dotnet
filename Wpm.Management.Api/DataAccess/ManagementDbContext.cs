using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Wpm.Management.Api.DataAccess
{
    public class ManagementDbContext(DbContextOptions<ManagementDbContext> options) : DbContext(options)
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Breed>().HasData([
                    new Breed(1, "Labrador Retriever"),
                    new Breed(2, "German Shepherd"),
                ]
            );

            modelBuilder.Entity<Pet>().HasData([
                    new Pet { Id = 1, Name = "Buddy", Age = 3, BreedId = 1 },
                    new Pet { Id = 2, Name = "Max", Age = 5, BreedId = 2 },
                    new Pet { Id = 3, Name = "Bella", Age = 2, BreedId = 1 },
                ]
            );
        }
    }

    public static class ManagementDbContextExtensions
    {
        public static void EnsureSeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ManagementDbContext>();
            context!.Database.EnsureCreated();
        }
    }

    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public int BreedId { get; set; }
        public Breed Breed { get; set; } = null!;
    }

    public record Breed(int Id, string Name);
}
