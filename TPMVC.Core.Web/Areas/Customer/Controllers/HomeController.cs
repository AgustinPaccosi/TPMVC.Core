using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using System.Diagnostics;
using System.Security.Claims;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.Models;
using TPMVC.Core.Web.ViewModels.Shoe;
using TPMVC.Core.Web.ViewModels.ShoppingCart;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        //private readonly IShoesService shoesService;
        //private readonly IShoesSizesService shoesSizesService;
        //private readonly ISizesService sizesService;
        //private readonly IMapper mapper;
        //public HomeController(ILogger<HomeController> logger, IShoesService ShoeService, IShoesSizesService ShoesSizesService,
        //    ISizesService SizesService, IMapper Mapper)
        //{
        //    _logger = logger;
        //    shoesService = ShoeService ?? throw new ApplicationException("Dependencies not set");
        //    shoesSizesService=ShoesSizesService ?? throw new ApplicationException("Dependencies not set");
        //    mapper = Mapper ?? throw new ApplicationException("Dependencies not set");
        //    sizesService = SizesService ?? throw new ApplicationException("Dependencies not set");
        //}
        private readonly IShoesService? _shoeService;
        private readonly IMapper? _mapper;
        private readonly IShoesSizesService? _shoeSizeService;
        private readonly ISizesService? _sizeService;
        private readonly IShoppingCartService? _shoppingCartService;

        public HomeController(IShoesService? shoeService, IMapper? mapper, IShoesSizesService? shoeSizeService, ISizesService? sizeService, IShoppingCartService? shoppingCartService)
        {
            _shoeService = shoeService;
            _mapper = mapper;
            _shoeSizeService = shoeSizeService;
            _sizeService = sizeService;
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 12;
            IEnumerable<Shoe> listaShoe = _shoeService!.GetAll(orderBy: o => o.OrderBy(p => p.Price), propertiesNames: "Sport,Brand,Colour,Genre")!;
            var listaShoePaginada = _mapper!.Map<IEnumerable<ShoeViewModel>>(listaShoe);
            ViewBag.currentPage = pageNumber;
            return View(listaShoePaginada.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Details(int id, int Page, string? returnurl = null)
        {
            List<ShoeSizesDetailsVM> list = new List<ShoeSizesDetailsVM>();
            var shoe = _shoeService!.Get(s => s.ShoeId == id, propertiesNames: "Sport,Brand,Colour,Genre");
            var listaSizes = _shoeService.GetAllShoesSizes(shoe!.ShoeId);
            for (int i = 0; i < listaSizes!.Count; i++)
            {
                ShoeSizesDetailsVM shoeSizes = new ShoeSizesDetailsVM();
                shoeSizes.Shoe = _mapper!.Map<ShoeViewModel>(shoe);
                shoeSizes.ShoeId = shoe.ShoeId;
                shoeSizes.Size = listaSizes[i].Size;//_sizeService.Get(filter: d=>d.SizeId==listaSizes[i].SizeId);
                shoeSizes.SizeId = listaSizes[i].SizeId;
                shoeSizes.AvailableStock = listaSizes[i].QuantityInStock; // _shoeSizeService!.Get(filter: s => s.SizeId == listaSizes[i].SizeId && s.ShoeId == shoe.ShoeId)!.QuantityInStock;
                shoeSizes.Page = Page;
                list.Add(shoeSizes);
            }
            ViewBag.ReturnUrl = returnurl;
            return View(list);
        }
        [Authorize(Roles = "Customer")]
        public IActionResult AgregarAlCarrito(int id, int sizeid, int Page)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userid = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var shoe = _shoeService!.Get(filter: s => s.ShoeId == id, propertiesNames: "Sport,Brand,Colour,Genre");
            var size = _sizeService!.Get(filter: s => s.SizeId == sizeid);
            if (shoe is null)
            {
                ModelState.AddModelError(string.Empty, "Shoe doesn´t found");
            }
            if (shoe is null)
            {
                ModelState.AddModelError(string.Empty, "Size doesn´t found");
            }

            ShoeSizesDetailsVM shoeSizes = new ShoeSizesDetailsVM();
            shoeSizes.Shoe = _mapper!.Map<ShoeViewModel>(shoe);
            shoeSizes.ShoeId = shoe!.ShoeId;
            shoeSizes.Size = size!;
            shoeSizes.SizeId = size!.SizeId;
            shoeSizes.AvailableStock = _shoeService.GetShoeSizeByIds(size!.SizeId, shoe.ShoeId).QuantityInStock;

            ShoppingCartDetailVm sh = new ShoppingCartDetailVm()
            {
                ShoeSizeId = (int)_shoeService.GetShoeSizeByIds(size!.SizeId, shoe.ShoeId).ShoeSizeId,
                Quantity = 1,
                ShoeSizeDetails = shoeSizes,
                ApplicationUserId = userid,
            };
            ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(sh);

            var cartInDb = _shoppingCartService!.Get(filter: s => s.ShoeSizeId == shoppingCart.ShoeSizeId &&
                    s.ApplicationUserId == shoppingCart.ApplicationUserId);

            var RelacionShoeSize = _shoeSizeService.Get(filter: s => s.ShoeId == shoe.ShoeId && s.SizeId == size.SizeId, propertiesNames: "Shoe,Size");
            if (RelacionShoeSize!.AvailableStock >= shoppingCart.Quantity)
            {
                if (cartInDb == null)
                {
                    RelacionShoeSize.StockInCarts += shoppingCart.Quantity;
                    _shoppingCartService!.Save(shoppingCart);
                }
                else
                {
                    RelacionShoeSize.StockInCarts += shoppingCart.Quantity;
                    cartInDb.Quantity += shoppingCart.Quantity;
                    _shoppingCartService.Save(cartInDb);
                }
                _shoeSizeService.Guardar(RelacionShoeSize);
                TempData["success"] = "Shoe added successfully to shopping cart";

            }
            else
            {
                TempData["error"] = "Not enuogh available";
            }

            return RedirectToAction("Index", new { page = Page });
        }
        public IActionResult Hero()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public IActionResult Index()
        //{

        //    return View();
        //}
        //public IActionResult Hero()
        //{
        //    return View();
        //}
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
