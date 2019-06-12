namespace PAIS.Models.ViewModels
{
    public enum SortEnum
    {
        NAME_ASC,
        NAME_DESC,
        YEAR_ASC,
        YEAR_DESC,
        RATE_ASC,
        RATE_DESC,
        PRICE_ASC,
        PRICE_DESC
    }

    public class BooksSortViewModel
    {
        public SortEnum NameSort { get; private set; }
        public SortEnum YearSort { get; private set; }
        public SortEnum RateSort { get; private set; }
        public SortEnum PriceSort { get; private set; }
        public SortEnum Current { get; private set; }
        public bool Up { get; set; }
        public BooksSortViewModel(SortEnum sortOrder)
        {
            NameSort = sortOrder == SortEnum.NAME_ASC ? SortEnum.NAME_DESC : SortEnum.NAME_ASC;
            YearSort = sortOrder == SortEnum.YEAR_ASC ? SortEnum.YEAR_DESC : SortEnum.YEAR_ASC;
            RateSort = sortOrder == SortEnum.RATE_ASC ? SortEnum.RATE_DESC : SortEnum.RATE_ASC;
            PriceSort = sortOrder == SortEnum.PRICE_ASC ? SortEnum.PRICE_DESC : SortEnum.PRICE_ASC;

            Up = true;

            if (sortOrder == SortEnum.NAME_DESC 
                || sortOrder == SortEnum.YEAR_DESC
                || sortOrder == SortEnum.PRICE_DESC
                || sortOrder == SortEnum.RATE_DESC)
            {
                Up = false;
            }
            Current = sortOrder;
        }
    }
}
