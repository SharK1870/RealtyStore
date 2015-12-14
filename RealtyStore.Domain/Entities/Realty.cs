using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RealtyStore.Domain.Entities
{
    public class Realty
    {
        [HiddenInput(DisplayValue = false)]    
        public int RealtyId { get; set; }

        [Display(Name = "Название")]
        //[Required(ErrorMessage = "Пожалуйста, введите название недвижимости")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        //[Required(ErrorMessage = "Пожалуйста, введите описание для недвижимости")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        //[Required(ErrorMessage = "Пожалуйста, укажите категорию для недвижимости")]
        public string Category { get; set; }

        [Display(Name = "Цена ($)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        //public byte[] ImageData { get; set; }
        //public string ImageMineType { get; set; }
    }
}
