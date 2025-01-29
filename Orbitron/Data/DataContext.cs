using Microsoft.EntityFrameworkCore;
using Orbitron.Models;

namespace Orbitron.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make the phone number as the primary key as each phone number is unique
            modelBuilder.Entity<Phone>().HasKey(p=>p.Number);

            // Make it so each user type is its own table
            modelBuilder.Entity<Seller>().ToTable("Sellers");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");


            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Package> Packages { get; set; }        
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Seller> Sellers {  get; set; }
        public DbSet<Administrator> Administrators { get; set; }
 

    }
}
