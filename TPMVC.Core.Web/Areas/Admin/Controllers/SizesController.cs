using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using TPMVC.Core.Entities;
using TPMVC.Core.Entities.Dtos;
using TPMVC.Core.Web.ViewModels.Shoe;
using TPMVC.Core.Web.ViewModels.Size;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizesController : Controller
    {

        private readonly ISizesService _service;
        private readonly IShoesSizesService _shoesSizesService;
        private readonly IMapper _mapper;
        public SizesController(ISizesService? service, IMapper? mapper, IShoesSizesService serviceS)
        {
            _service = service;
            _shoesSizesService = serviceS;
            _mapper = mapper;
        }
        public IActionResult Index(int? page, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            var Sizes = _service?.GetAll
                (orderBy: o => o.OrderBy(c => c.SizeNumber));
            var SizesVm = _mapper?.Map<List<SizeListVm>>(Sizes);
            //.ToPagedList(pageNumber, pageSize);
            foreach (var size in SizesVm!)
            {
                size.CantidadZapatillas = (int)(_service?.ContarZapatillasPorTalle(size.SizeId))!;
            }
            return View(SizesVm.OrderByDescending(o => o.CantidadZapatillas).
                ToPagedList(pageNumber, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            SizeEditVm SizeVm;
            if (id == null || id == 0)
            {
                SizeVm = new SizeEditVm();
            }
            else
            {
                try
                {
                    Size? size = _service.Get(filter: g => g.SizeId == id);
                    if (size == null)
                    {
                        return NotFound();
                    }
                    SizeVm = _mapper.Map<SizeEditVm>(size);
                    return View(SizeVm);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(SizeVm);

        }



        [HttpPost]
        public IActionResult UpSert(SizeEditVm SizeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(SizeVm);
            }

            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Size size = _mapper.Map<Size>(SizeVm);

                if (_service.Existe(size))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(SizeVm);
                }

                _service.Guardar(size);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(SizeVm);
            }
        }

        //TODO HACER RELACION

        [HttpGet]
        public IActionResult Details(int id)
        {
            var shoes = _service?.GetShoesForSize(id);
            foreach (var shoe in shoes)
            {
                var shoeSizeBD = _shoesSizesService!
                .Get(filter: ss => ss.ShoeId == shoe.ShoeId && ss.SizeId == id);
                shoe.ShoesSizes.Add(shoeSizeBD);
            }
            //var ShoeSize = _shoesSizesService.Get();
            //var shoeSizes = _shoesSizesService.GetAll();
            //var shoeDto = MapToDtoList(shoe, shoeSizes);
            if (shoes == null || shoes.Count == 0)
            {
                ViewData["Mensaje"] = "No hay zapatillas asociadas a este talle.";
            }
            return View(shoes);
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Size? size = _service?.Get(filter: g => g.SizeId == id);
            if (size is null)
            {
                return NotFound();
            }
            try
            {
                if (_service == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                if (_service.EstaRelacionado(size))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _service.Eliminar(size);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;
            }
        }

        //public static List<ShoeDto> MapToDtoList(IEnumerable<Shoe> shoes, IEnumerable<ShoeSize> shoeSizes)
        //{
        //    return shoes.Select(shoe => new ShoeDto
        //    {
        //        ShoeId = shoe.ShoeId,
        //        Brand = shoe.Brand,
        //        Sport = shoe.Sport,
        //        Genre = shoe.Genre,
        //        Colour = shoe.Colour,
        //        Model = shoe.Model,
        //        Description = shoe.Description,
        //        Price = shoe.Price,
        //        // Calculamos el stock sumando los valores asociados al ShoeId
        //        Stock = shoeSizes
        //  .Where(size => size.ShoeId == shoe.ShoeId)
        //  .Sum(size => size.QuantityInStock)
        //    }).ToList();
        //}
        [HttpPost]
        public IActionResult UpdateStock(int shoeId, int sizeId, int stock)
        {
            ShoeSize shoeSize = _shoesSizesService.Get(filter: ss => ss.ShoeId == shoeId && ss.SizeId == sizeId);

            if (shoeSize != null)
            {
                if (stock >= 0)
                {
                    shoeSize.QuantityInStock = stock;
                    _shoesSizesService.Guardar(shoeSize);
                }
            }
            //else
            //{
            //    var size = _shoesSizesService.Get(filter: s => s.SizeNumber == sizeNumber);
            //    shoeSize = new ShoeSize
            //    {
            //        ShoeId = shoeId,
            //        SizeId = size.SizeId,
            //        QuantityInStock = stock
            //    };

            //    _services.InsertShoeSize(shoeSize);

            //}
            return RedirectToAction("Details", new { id = sizeId });


        }
    }
}
