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
                    Name = "ЕНЦИКЛОПЕДІЯ АФОРИЗМІВ, КРИЛАТИХ ФРАЗ, ЦИТАТ",
                    Description = "«Журналiстика – це творчість і ремесло»",
                    Price = 280
                },
                new Book
                {
                    Name = "ПРОШУ КОРОТКО",
                    Description = "Розмова з Карелом Гвіждялою, примітки, документи",
                    Price = 130
                },
                new Book
                {
                    Name = "Теорія і методика журналістської творчості",
                    Description = "",
                    Price = 70
                },
                new Book
                {
                    Name = "Телевізійна журналістика",
                    Description = "",
                    Price = 40
                },
                new Book
                {
                    Name = "Редагування в засобах масової інформації",
                    Description = "",
                    Price = 70
                },
                new Book
                {
                    Name = "Сучасний англомовний світ і збагачення словникового складу",
                    Description = "",
                    Price = 50
                },
                new Book
                {
                    Name = "Скоропадський, Маннергейм, Врангель: кавалеристи-державники",
                    Description = "",
                    Price = 40
                },
                new Book
                {
                    Name = "Міфи Другої світової війни",
                    Description = "",
                    Price = 30
                },
                new Book
                {
                    Name = "Велика Британія: географія, історія, культура",
                    Description = "",
                    Price = 90
                }
                );
                context.SaveChanges();
            }
        }
    }
}