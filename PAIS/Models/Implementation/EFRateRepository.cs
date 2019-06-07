using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAIS.Models
{
    public class EFRateRepository: IRateRepository
    {
        private ApplicationDbContext context;

        public EFRateRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Rate> Rates => context.Rates;

        public void SaveRate(Rate rate)
        {
            if (rate.RateId == 0)
            {
                context.Rates.Add(rate);
            }
            else
            {
                Rate dbEntry = context.Rates
                    .FirstOrDefault(p => p.RateId == rate.RateId);
                if (dbEntry != null)
                {
                    dbEntry.BookId = rate.BookId;
                    dbEntry.RateId = rate.RateId;
                    dbEntry.Value = rate.Value;
                }
            }
            context.SaveChanges();
        }

        public Rate DeleteRate(int rateId)
        {
            Rate dbEntry = context.Rates
                .FirstOrDefault(p => p.RateId == rateId);

            if (dbEntry != null)
            {
                var rates = context.Rates.Where(c => c.BookId == rateId);
                context.Rates.Remove(dbEntry);
                foreach (var c in rates)
                {
                    context.Rates.Remove(c);
                }
                context.SaveChanges();
            }

            return dbEntry;
        }

        public void DeleteUserRates(string userId)
        {
            foreach (var c in Rates.Where(c => c.UserId == userId))
            {
                Rate dbEntry = context.Rates
                    .FirstOrDefault(p => p.RateId == c.RateId);

                if (dbEntry != null)
                {
                    context.Rates.Remove(dbEntry);
                }
            }

            context.SaveChanges();
        }

        public void DeleteBookRates(int bookId)
        {
            foreach (var c in Rates.Where(c => c.BookId == bookId))
            {
                Rate dbEntry = context.Rates
                    .FirstOrDefault(p => p.RateId == c.RateId);

                if (dbEntry != null)
                {
                    context.Rates.Remove(dbEntry);
                }
            }

            context.SaveChanges();
        }

        public Rate GetRate(int rateId)
        {
            return Rates
                .Where(n => n.RateId == rateId)
                .OrderBy(n => n.RateId)
                .First();
        }
    }
}
