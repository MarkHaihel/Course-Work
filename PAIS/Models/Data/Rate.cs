namespace PAIS.Models
{
    public class Rate
    {
        public int RateId { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public decimal Value { get; set; }
    }
}
