using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPMVC.Core.Web.ViewModels.Shoe
{
    public class ShoeViewModel
    {
        public int ShoeId { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "No se admiten más de 50 caracteres")]
        [DisplayName("Marca")]
        public string? BrandName { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "No se admiten más de 50 caracteres")]
        [DisplayName("Deporte")]
        public string? SportName { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "No se admiten más de 50 caracteres")]
        [DisplayName("Genero")]
        public string? GenreName { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "No se admiten más de 50 caracteres")]
        [DisplayName("Color")]
        public string? ColourName { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(150, ErrorMessage = "No se admiten más de 150 caracteres")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "{0} es requerido")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "{0} es requerido")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public bool Active { get; set; }
        public string imageURL { get; set; }
        public decimal PrecioEfectivo { get; set; }
    }
}
