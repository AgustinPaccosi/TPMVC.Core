using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Shoe;
using X.PagedList.Extensions;
using X.PagedList;

namespace TPMVC.Core.Web.Controllers
{
    public class ShoesController : Controller
    {
        private readonly IShoesService? _service;
        private readonly IBrandsService? _servicioBrand;
        private readonly ISportsService? _servicioSport;
        private readonly IGenresService? _servicioGenre;
        private readonly IColoursService? _servicioColour;
        private readonly ISizesService? _servicioSize;
        //private readonly IServiceShoeSize? _servicioShoeSize;
        private readonly IMapper? _mapper;

        public ShoesController(IShoesService? service,
            IBrandsService? servicioBrand, ISportsService? servicioSport,
            IGenresService? servicioGenre, IColoursService? servicioColour,
            ISizesService? servicioSize//, IServiceShoeSize servicioShoeSize
            , IMapper? mapper)
        {
            _service = service ?? throw new ArgumentException("Dependencies not set");
            _servicioBrand = servicioBrand ?? throw new ArgumentNullException(nameof(servicioBrand));
            _servicioSport = servicioSport ?? throw new ArgumentException("Dependencies not set");
            _servicioGenre = servicioGenre ?? throw new ArgumentException("Dependencies not set");
            _servicioColour = servicioColour ?? throw new ArgumentException("Dependencies not set");
            _servicioSize = servicioSize ?? throw new ArgumentException("Dependencies not set");
            //_servicioShoeSize = servicioShoeSize ?? throw new ArgumentException("Dependencies not set");
            _mapper = mapper ?? throw new ArgumentException("Dependencies not set");
        }
        public IActionResult Index(int? page, int? FilterBrandId, int pageSize = 10, bool viewAll = false)
        {
            var pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Shoe>? shoes;
            if (FilterBrandId is null || viewAll)
            {
                shoes = _service!
                    .GetAll(orderBy: o => o.OrderBy(s => s.Brand!.BrandName),
                    propertiesNames: "Brand,Genre,Sport,Colour");
            }
            else
            {
                shoes = _service!
                     .GetAll(orderBy: o => o.OrderBy(s => s.Brand!.BrandName),
                             filter: s => s.BrandId == FilterBrandId,
                     propertiesNames: "Brand,Genre,Sport,Colour");
                ViewBag.currentFilterBrandId = FilterBrandId;
            }
            var shoesVm = _mapper!
                .Map<List<ShoeViewModel>>(shoes);
            var shoeFilterVm = new ShoeFilterVM
            {
                Shoes = shoesVm.ToPagedList(pageNumber, pageSize),
                Brands = _servicioBrand!.GetAll(orderBy: o => o.OrderBy(c => c.BrandName))!
                    .Select(c => new SelectListItem
                    {
                        Text = c.BrandName,
                        Value = c.BrandId.ToString()
                    })
                    .ToList()
            };
            return View(shoeFilterVm);
        }


        public IActionResult UpSert(int? id)
        {
            ShoeEditVm shoeEditVm;
            if (id == null || id == 0)
            {
                shoeEditVm = new ShoeEditVm();
                CargarComboBoxs(shoeEditVm); //TODO
            }
            else
            {
                try
                {
                    Shoe? shoe = _service!.Get(filter: c => c.ShoeId == id);
                    if (shoe == null)
                    {
                        return NotFound();
                    }
                    shoeEditVm = _mapper!.Map<ShoeEditVm>(shoe);
                    CargarComboBoxs(shoeEditVm);
                    return View(shoeEditVm);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(shoeEditVm);

        }

        private void CargarComboBoxs(ShoeEditVm shoeEditVm)
        {
            shoeEditVm.Brands = _servicioBrand
            .GetAll(orderBy: o => o.OrderBy(c => c.BrandName))
            .Select(c => new SelectListItem
            { Text = c.BrandName, Value = c.BrandId.ToString() }).ToList();

            shoeEditVm.Sports = _servicioSport
            .GetAll(orderBy: o => o.OrderBy(c => c.SportName))
            .Select(c => new SelectListItem
            { Text = c.SportName, Value = c.SportId.ToString() }).ToList();

            shoeEditVm.Genres = _servicioGenre
            .GetAll(orderBy: o => o.OrderBy(c => c.GenreName))
            .Select(c => new SelectListItem
            { Text = c.GenreName, Value = c.GenreId.ToString() }).ToList();

            shoeEditVm.Colours = _servicioColour
            .GetAll(orderBy: o => o.OrderBy(c => c.ColourName))
            .Select(c => new SelectListItem
            { Text = c.ColourName, Value = c.ColourId.ToString() }).ToList();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ShoeEditVm ShoeEditVm)
        {
            if (!ModelState.IsValid)
            {
                CargarComboBoxs(ShoeEditVm);
                return View(ShoeEditVm);
            }

            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Shoe Shoe = _mapper.Map<Shoe>(ShoeEditVm);

                if (_service.Existe(Shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    CargarComboBoxs(ShoeEditVm);
                    return View(ShoeEditVm);
                }

                _service.Guardar(Shoe);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                CargarComboBoxs(ShoeEditVm);

                return View(ShoeEditVm);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Shoe? Shoe = _service?.Get(filter: g => g.ShoeId == id);
            if (Shoe is null)
            {
                return NotFound();
            }
            try
            {
                if (_service == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                if (_service.EstaRelacionado(Shoe.ShoeId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _service.Eliminar(Shoe);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;
            }
        }

    }
}
