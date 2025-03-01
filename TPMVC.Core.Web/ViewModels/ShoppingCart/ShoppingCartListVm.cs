using TPMVC.Core.Web.ViewModels.OrderHeaderVista;

namespace TPMVC.Core.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartListVm
    {
        public List<TPMVC.Core.Entities.ShoppingCart>? ShoppingCarts { get; set; }
        public OrderHeaderEditVm? OrderHeader { get; set; }
    }
}
