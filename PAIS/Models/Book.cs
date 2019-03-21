namespace PAIS.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Format { get; set; }
        public string PublicationDate { get; set; }
        public string Binder { get; set; }
        public int Amount { get; set; }
        public string Anotation { get; set; }
        public string ISBNCode { get; set; }
        public decimal Price { get; set; }
    }
}
