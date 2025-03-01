using TPMVC.Core.Entities;

namespace TPMVC.Core.Web.ViewModels.Shoe
{
    public class ShoeSizesDetailsVM
    {
        public int ShoeId { get; set; }
        public ShoeViewModel Shoe { get; set; }
        public int SizeId { get; set; }
        public TPMVC.Core.Entities.Size Size { get; set; }

        public int AvailableStock { get; set; }
        public int Page { get; set; }

    }
}
