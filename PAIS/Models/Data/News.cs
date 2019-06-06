using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace PAIS.Models
{
    public class News
    {
        public int NewsID { get; set; }

        [Required(ErrorMessage = "Будь ласка введіть дату")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Будь ласка введіть назву")]
        public string Name { get; set; }

        public string Author { get; set; }

        [Required(ErrorMessage = "Будь ласка введіть опис")]
        public string Description { get; set; }

        public long? Count { get; set; } = 0;
    }
}
