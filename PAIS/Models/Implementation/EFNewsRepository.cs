using System.Collections.Generic;
using System.Linq;
using System;

namespace PAIS.Models
{
    public class EFNewsRepository : INewsRepository
    {
        private ApplicationDbContext context;

        public EFNewsRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<News> NewsRepo => context.NewsRepo;

        public void SaveNews(News news)
        {
            if (news.NewsID == 0)
            {
                context.NewsRepo.Add(news);
            }
            else
            {
                News dbEntry = context.NewsRepo
                    .FirstOrDefault(p => p.NewsID == news.NewsID);
                if (dbEntry != null)
                {
                    dbEntry.Name = news.Name;
                    dbEntry.Description = news.Description;
                    dbEntry.Author = news.Author;
                    dbEntry.Date = news.Date;
                }
            }
            context.SaveChanges();
        }

        public News DeleteNews(int newsID)
        {
            News dbEntry = context.NewsRepo
            .FirstOrDefault(p => p.NewsID == newsID);
            if (dbEntry != null)
            {
                context.NewsRepo.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public IQueryable<News> LastNews()
        {
            return NewsRepo
                .OrderByDescending(n => n.Date)
                .Skip(Math.Max(0, NewsRepo.Count() - 3));
        }

        public News GetNews(int newsID)
        {
            News dbEntry = context.NewsRepo
                    .FirstOrDefault(p => p.NewsID == newsID);
            if (dbEntry != null)
            {
                dbEntry.Count++;
            }
            context.SaveChanges();

            return NewsRepo
                .Where(n => n.NewsID == newsID)
                .OrderBy(n => n.NewsID)
                .First();
        }
    }
}
