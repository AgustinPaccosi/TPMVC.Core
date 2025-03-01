using TPMVC.Core.Web.ViewModels.Shoe;

namespace TPMVC.Core.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartDetailVm
    {
        public int ShoppingCartId { get; set; }
        public int ShoeSizeId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ShoeSizesDetailsVM ShoeSizeDetails { get; set; } = null!;
    }
}
