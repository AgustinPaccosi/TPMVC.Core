﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IOrderHeadersService? _orderHeadersService;
        private readonly IShoesService? _shoeService;
        private readonly ISizesService? _sizeService;
        private readonly IMapper? _mapper;

        public ShoppingCartController(IShoppingCartService? cartsService,
            ICountriesService countriesService,
            IStatesService? statesService,
            ICitiesService? citiesService,
            IApplicationUserService? applicationUsersService,
            IShoesSizesService shoeSizeService,
            IOrderHeadersService? orderHeadersService,
            IShoesService shoeService,
            ISizesService sizeService,
            IMapper? mapper)
        {
            _cartsService = cartsService;
            _countriesService = countriesService ?? throw new ApplicationException("Dependencies not set");
            _statesService = statesService ?? throw new ApplicationException("Dependencies not set");
            _citiesService = citiesService ?? throw new ApplicationException("Dependencies not set");
            _applicationUsersService = applicationUsersService ?? throw new ApplicationException("Dependencies not set");
            _orderHeadersService = orderHeadersService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper;
            _shoeSizeService = shoeSizeService ?? throw new ApplicationException("Dependencies not set");
            _shoeService = shoeService ?? throw new ApplicationException("Dependencies not set");
            _sizeService = sizeService ?? throw new ApplicationException("Dependencies not set");
        }
        public IActionResult Index(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            if (claimsIdentity == null || !User.Identity.IsAuthenticated)
            {
                // Usuario no autenticado, manejarlo correctamente
                return RedirectToAction("Login", "Account");
            }
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

        public IActionResult Plus(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id, tracked: false,
                propertiesNames: "ShoeSize");
            if (cartInDb!.ShoeSize!.AvailableStock >= 1)
            {
                cartInDb.ShoeSize.StockInCarts += 1;
                _shoeSizeService!.Guardar(cartInDb.ShoeSize);
                cartInDb!.Quantity += 1;
                cartInDb.LastUpdate = DateTime.Now;
                _cartsService.Save(cartInDb);
            }
            else
            {
                TempData["error"] = "Not enough stock available!!";
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult Minus(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id,
                propertiesNames: "ShoeSize");
            cartInDb!.Quantity -= 1;
            cartInDb.ShoeSize!.StockInCarts -= 1;
            _shoeSizeService!.Guardar(cartInDb.ShoeSize);
            cartInDb.LastUpdate = DateTime.Now;
            if (cartInDb.Quantity == 0)
            {
                _cartsService.Delete(cartInDb);
            }
            else
            {
                _cartsService.Save(cartInDb);

            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult Remove(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id,
                    propertiesNames: "ShoeSize");
            cartInDb!.ShoeSize!.StockInCarts -= cartInDb!.Quantity;
            _shoeSizeService!.Guardar(cartInDb.ShoeSize);
            _cartsService.Delete(cartInDb);
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Summary(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "ShoeSize")!.ToList();
            ShoppingCartListVm shoppingVm = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderHeader = new OrderHeaderEditVm()
                {
                    OrderTotal = CalculateTotal(cartList),
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.AddDays(3),
                    OrderDetails = _mapper!.Map<List<OrderDetail>>(cartList),
                    Countries = GetCountries(),
                    States = GetCountryStates(),
                    Cities = GetStateCities(),

                }

            };
            foreach (var cart in cartList)
            {
                cart.ShoeSize.Shoe = _shoeService!.Get(filter: s => s.ShoeId == cart.ShoeSize.ShoeId)!;
                cart.ShoeSize.Size = _sizeService!.Get(filter: s => s.SizeId == cart.ShoeSize.SizeId)!;
            }
            var user = _applicationUsersService!.Get(filter: u => u.Id == claims!.Value);
            shoppingVm.OrderHeader.ApplicationUserId = user!.Id;
            shoppingVm.OrderHeader.FirstName = user.FirstName;
            shoppingVm.OrderHeader.LastName = user.LastName;
            shoppingVm.OrderHeader.Address = user.Address;
            shoppingVm.OrderHeader.ZipCode = user.Zipcode;
            shoppingVm.OrderHeader.CountryId = user.CountryId;
            shoppingVm.OrderHeader.StateId = user.StateId;
            shoppingVm.OrderHeader.CityId = user.CityId;
            shoppingVm.OrderHeader.Phone = user.Phone;

            ViewBag.ReturnUrl = returnUrl;

            return View(shoppingVm);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartListVm shoppingVm)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "ShoeSize")!.ToList();
            shoppingVm.OrderHeader!.OrderTotal = CalculateTotal(cartList);
            shoppingVm.OrderHeader.OrderDate = DateTime.Now;
            shoppingVm.OrderHeader.ShippingDate = DateTime.Now.AddDays(3);
            shoppingVm.OrderHeader.OrderDetails = _mapper!.Map<List<OrderDetail>>(cartList);
            shoppingVm.OrderHeader.Countries = GetCountries();
            shoppingVm.OrderHeader.States = GetCountryStates();
            shoppingVm.OrderHeader.Cities = GetStateCities();
            for (int i = 0; i < cartList.Count(); i++)
            {
                shoppingVm.OrderHeader.OrderDetails[i].ShoeSizeId = cartList[i].ShoeSizeId;
            }
            if (!ModelState.IsValid)
            {
                return View(shoppingVm);
            }
            OrderHeader orderHeader = _mapper.Map<OrderHeader>(shoppingVm.OrderHeader);
            try
            {
                _orderHeadersService!.Guardar(orderHeader);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(shoppingVm);
            }
            return RedirectToAction("OrderConfirmed", new { id = orderHeader.OrderHeaderId });
        }
        public IActionResult OrderConfirmed(int id)
        {
            return View(id);
        }
        public IActionResult OrderCancelled()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "ShoeSize")!.ToList();
            foreach (var item in cartList)
            {
                item.ShoeSize.StockInCarts -= item.Quantity;
                _cartsService.Delete(item);
            }
            return View();
        }
        private List<SelectListItem> GetCountries()
        {
            return _countriesService!.GetAll(
                                    orderBy: o => o.OrderBy(c => c.CountryName))
                                .Select(c => new SelectListItem
                                {
                                    Text = c.CountryName,
                                    Value = c.CountryId.ToString()
                                }).ToList();
        }
        private List<SelectListItem> GetCountryStates(int? countryId = null) //Preguntar por qué es mejor pasarle este tipo de lista, que una lista de Countries, States y Cities?????????
        {
            IEnumerable<State>? states;
            if (countryId is null)
            {
                states = _statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName));
            }
            else
            {
                states = _statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName),
                    filter: s => s.CountryId == countryId);

            }
            return states.Select(c => new SelectListItem
            {
                Text = c.StateName,
                Value = c.StateId.ToString()
            }).ToList();

        }
        private List<SelectListItem> GetStateCities(int? stateId = null)
        {
            IEnumerable<City>? cities;
            if (stateId is null)
            {
                cities = _citiesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.CityName));
            }
            else
            {
                cities = _citiesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.CityName),
                    filter: c => c.StateId == stateId);

            }
            return cities
                .Select(c => new SelectListItem
                {
                    Text = c.CityName,
                    Value = c.CityId.ToString()
                }).ToList();

        }


    }
}
