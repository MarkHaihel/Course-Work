using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace PAIS.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
            .GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                new Book
                {
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Price = 275
                },
                new Book
                {
                    Name = "Lifejacket",
                    Description = "Protective and fashionable",
                    Price = 48.95m
                },
                new Book
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-approved size and weight",
                    Price = 19.50m
                },
                new Book
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Price = 34.95m
                },
                new Book
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Price = 79500
                },
                new Book
                {
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Price = 16
                },
                new Book
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Price = 29.95m
                },
                new Book
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Price = 75
                },
                new Book
                {
                    Name = "Bling-Bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Price = 1200
                }
                );
                context.SaveChanges();
            }
        }
    }
}