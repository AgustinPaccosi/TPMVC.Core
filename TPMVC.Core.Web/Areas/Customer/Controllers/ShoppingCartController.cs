using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using System.Security.Claims;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.OrderHeaderVista;
using TPMVC.Core.Web.ViewModels.ShoppingCart;

namespace TPMVC.Core.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService? _cartsService;
        private readonly ICountriesService? _countriesService;
        private readonly IStatesService? _statesService;
        private readonly ICitiesService? _citiesService;
        private readonly IApplicationUserService? _applicationUsersService;
        private readonly IShoesSizesService? _shoeSizeService;
        //private readonly IOrderHeadersService? _orderHeadersService;
        private readonly IShoesService? _shoeService;
        private readonly ISizesService? _sizeService;
        private readonly IMapper? _mapper;

        public ShoppingCartController(IShoppingCartService? cartsService,
            ICountriesService countriesService,
            IStatesService? statesService,
            ICitiesService? citiesService,
            IApplicationUserService? applicationUsersService,
            IShoesSizesService shoeSizeService,
            //IOrderHeadersService? orderHeadersService,
            IShoesService shoeService,
            ISizesService sizeService,
            IMapper? mapper)
        {
            _cartsService = cartsService;
            _countriesService = countriesService ?? throw new ApplicationException("Dependencies not set");
            _statesService = statesService ?? throw new ApplicationException("Dependencies not set");
            _citiesService = citiesService ?? throw new ApplicationException("Dependencies not set");
            _applicationUsersService = applicationUsersService ?? throw new ApplicationException("Dependencies not set");
            //_orderHeadersService = orderHeadersService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper;
            _shoeSizeService = shoeSizeService ?? throw new ApplicationException("Dependencies not set");
            _shoeService = shoeService ?? throw new ApplicationException("Dependencies not set");
            _sizeService = sizeService ?? throw new ApplicationException("Dependencies not set");
        }
        public IActionResult Index(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var cartList = _cartsService!.GetAll(
                filter: c => c.ApplicationUserId == userId, propertiesNames: "ShoeSize")!.ToList();
            foreach (var cart in cartList)
            {
                cart.ShoeSize.Size = _sizeService!.Get(filter: s => s.SizeId == cart.ShoeSize.SizeId)!;
            }
            ShoppingCartListVm shoppingVm = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderHeader = new OrderHeaderEditVm()
                {
                    OrderTotal = CalculateTotal(cartList)
                }
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(shoppingVm);
        }
        private decimal CalculateTotal(List<ShoppingCart> cartList)
        {
            var total = 0M;
            foreach (var item in cartList)
            {
                item.ShoeSize.Shoe = _shoeService!.Get(filter: s => s.ShoeId == item.ShoeSize.ShoeId)!;
                total += (item.Quantity == 1 ? item.ShoeSize.Shoe.Price : item.ShoeSize.Shoe.Price * 0.9M) * item.Quantity;
            }
            return total;
        }
    }
}
