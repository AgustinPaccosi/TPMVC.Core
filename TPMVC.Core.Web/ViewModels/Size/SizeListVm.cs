using System.ComponentModel;

namespace TPMVC.Core.Web.ViewModels.Size
{
    public class SizeListVm
    {
        public int SizeId { get; set; }
        [DisplayName("Talles")]
        public string? SizeNumber { get; set; }
        [DisplayName("Zapatillas Por Talle")]
        public int CantidadZapatillas { get; set; }
    }
}
