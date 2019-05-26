using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PAIS.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "Please enter a book name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a book description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter an author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please enter a format")]
        public string Format { get; set; }

        [Required(ErrorMessage = "Please enter a publication date")]
        public string PublicationDate { get; set; }

        [Required(ErrorMessage = "Please specify a type")]
        public string PublicationType { get; set; }

        [Required(ErrorMessage = "Please enter a binder")]
        public string Binder { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter an amount")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Please enter an anotation")]
        public string Anotation { get; set; }
        
        [Required(ErrorMessage = "Please enter an ISBNCode")]
        public string ISBNCode { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
    }
}
