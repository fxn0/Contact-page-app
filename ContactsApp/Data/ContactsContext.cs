using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data
{
    //klasa odpowiedzialna za kontekst bazodanowy
    public class ContactsContext : DbContext
    {
        public ContactsContext(DbContextOptions<ContactsContext> options) : base(options) { }


        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        
        //metoda odpowiedzialna za konfigurację modelu bazy danych 
        //inicjalizowane są w niej podstawowe kategorie kontaktów
        //nakładane są ograniczenia na dane (email musi być unikalny)
        //oraz tworzy połącznie między kontaktem a kategorią kontaktu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ContactCategory>().HasData(
                new ContactCategory { Id = 1, Name = "Służbowy" },
                new ContactCategory { Id = 2, Name = "Prywatny" },
                new ContactCategory { Id = 3, Name = "Inny" }
            );

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
