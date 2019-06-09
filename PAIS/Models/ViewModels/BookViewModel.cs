using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PAIS.Models.ViewModels
{
    public class BookViewModel
    {
        public int BookID { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Введіть назву книги")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введіть автора")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Введіть формат")]
        public string Format { get; set; }

        [Required(ErrorMessage = "Вкажіть дату публікації")]
        public string PublicationDate { get; set; }

        [Required(ErrorMessage = "Вкажіть тим публікації")]
        public string PublicationType { get; set; }

        [Required(ErrorMessage = "Виберіть обгортку")]
        public string Binder { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Введіть обсяг")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Введіть анотацію")]
        public string Anotation { get; set; }

        [Required(ErrorMessage = "Введіть ISBNCode")]
        public string ISBNCode { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Вкажіть ціну")]
        public decimal Price { get; set; }

        public decimal Rate { get; set; }
        public uint RatesAmount { get; set; }
    }
}
