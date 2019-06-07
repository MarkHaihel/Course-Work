using System.Linq;

namespace PAIS.Models
{
    public interface IRateRepository
    {
        IQueryable<Rate> Rates { get; }

        void SaveRate(Rate rate);

        Rate DeleteRate(int rateId);

        void DeleteUserRates(string userId);

        void DeleteBookRates(int bookId);

        Rate GetRate(int rateId);
    }
}
