using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Shoe;
using X.PagedList.Extensions;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using TPMVC.Core.Web.ViewModels.Size;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ShoesController : Controller
    {
        private readonly IShoesService? _service;
        private readonly IBrandsService? _serviceBrand;
        private readonly ISportsService? _serviceSport;
        private readonly IGenresService? _serviceGenre;
        private readonly IColoursService? _serviceColour;
        private readonly ISizesService? _serviceSize;
        private readonly IShoesSizesService? _serviceShoeSize;
        private readonly IMapper? _mapper;
        private readonly IWebHostEnvironment ? webHostEnviroment;

        public ShoesController(IShoesService? service,
            IBrandsService? servicioBrand, ISportsService? servicioSport,
            IGenresService? servicioGenre, IColoursService? servicioColour,
            ISizesService? servicioSize, IShoesSizesService servicioShoeSize
            , IMapper? mapper, IWebHostEnvironment? webHostEnviroment)
        {
            _service = service ?? throw new ArgumentException("Dependencies not set");
            _serviceBrand = servicioBrand ?? throw new ArgumentNullException(nameof(servicioBrand));
            _serviceSport = servicioSport ?? throw new ArgumentException("Dependencies not set");
            _serviceGenre = servicioGenre ?? throw new ArgumentException("Dependencies not set");
            _serviceColour = servicioColour ?? throw new ArgumentException("Dependencies not set");
            _serviceSize = servicioSize ?? throw new ArgumentException("Dependencies not set");
            _serviceShoeSize = servicioShoeSize ?? throw new ArgumentException("Dependencies not set");
            _mapper = mapper ?? throw new ArgumentException("Dependencies not set");
            this.webHostEnviroment = webHostEnviroment ?? throw new ArgumentException("Dependencies not set");
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
                Brands = _serviceBrand!.GetAll(orderBy: o => o.OrderBy(c => c.BrandName))!
                    .Select(c => new SelectListItem
                    {
                        Text = c.BrandName,
                        Value = c.BrandId.ToString()
                    })
                    .ToList()
            };
            return View(shoeFilterVm);
        }

        public IActionResult UpSert(int? id, string? returnurl = null)
        {
            ShoeEditVm? shoeEditVM;
            if (id is null || id.Value == 0)
            {
                shoeEditVM = new ShoeEditVm();
                RercargarCombos(shoeEditVM);
            }
            else
            {
                try
                {
                    string? wwwWebRoot = webHostEnviroment!.WebRootPath;
                    Shoe? shoe = _service?.Get(s => s.ShoeId == id.Value);
                    if (shoe == null)
                    {
                        return NotFound();
                    }
                    if (!string.IsNullOrEmpty(shoe.imageURL))
                    {
                        string filePath = Path.Combine(wwwWebRoot, shoe.imageURL.TrimStart('/'));
                        ViewData["fileExist"] = System.IO.File.Exists(filePath);
                    }
                    else
                    {
                        ViewData["fileExist"] = false;
                    }
                    shoeEditVM = _mapper?.Map<ShoeEditVm>(shoe);
                    RercargarCombos(shoeEditVM);
                    shoeEditVM!.returnURL = returnurl;

                    return View(shoeEditVM);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(shoeEditVM);
        }
        private void RercargarCombos(ShoeEditVm? shoeEditVM)
        {
            shoeEditVM!.Brands = _serviceBrand!.GetAll()!
                .Select(b => new SelectListItem
                {
                    Text = b.BrandName,
                    Value = b.BrandId.ToString()
                }
                ).ToList();
            shoeEditVM.Colours = _serviceColour!.GetAll()!
              .Select(c => new SelectListItem
              {
                  Text = c.ColourName,
                  Value = c.ColourId.ToString()
              }
              ).ToList();
            shoeEditVM.Genres = _serviceGenre!.GetAll()!
              .Select(g => new SelectListItem
              {
                  Text = g.GenreName,
                  Value = g.GenreId.ToString()
              }
              ).ToList();
            shoeEditVM.Sports = _serviceSport!.GetAll()!
              .Select(s => new SelectListItem
              {
                  Text = s.SportName,
                  Value = s.SportId.ToString()
              }
              ).ToList();
        }
        //public IActionResult UpSert(int? id)
        //{
        //    ShoeEditVm shoeEditVm;
        //    if (id == null || id == 0)
        //    {
        //        shoeEditVm = new ShoeEditVm();
        //        CargarComboBoxs(shoeEditVm); //TODO
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Shoe? shoe = _service!.Get(filter: c => c.ShoeId == id);
        //            if (shoe == null)
        //            {
        //                return NotFound();
        //            }
        //            shoeEditVm = _mapper!.Map<ShoeEditVm>(shoe);
        //            CargarComboBoxs(shoeEditVm);
        //            return View(shoeEditVm);
        //        }
        //        catch (Exception)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
        //        }

        //    }
        //    return View(shoeEditVm);

        //}
        [HttpPost]
        public IActionResult UpSert(ShoeEditVm shoeEditVM)
        {
            string? returnurl = shoeEditVM!.returnURL;
            if (!ModelState.IsValid)
            {
                RercargarCombos(shoeEditVM);
                return View(shoeEditVM);
            }
            try
            {
                string? webroot = webHostEnviroment!.WebRootPath;
                Shoe shoe = _mapper!.Map<Shoe>(shoeEditVM);
                var brand = _serviceBrand!.Get(filter: filter => filter.BrandId == shoe.BrandId);
                var sport = _serviceSport!.Get(filter: filter => filter.SportId == shoe.SportId);
                var genre = _serviceGenre!.Get(filter: filter => filter.GenreId == shoe.GenreId);
                var color = _serviceColour!.Get(filter: filter => filter.ColourId == shoe.ColourId);
                shoe.Colour = color;
                shoe.Brand = brand;
                shoe.Genre = genre;
                shoe.Sport = sport;
                if (shoe == null)
                {
                    ModelState.AddModelError(string.Empty, "No Shoe has been supplied");
                    return View(shoeEditVM);
                }
                if (_service!.Existe(shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    RercargarCombos(shoeEditVM);
                    return View(shoeEditVM);
                }
                //if (!shoeEditVM.RemoveImage)
                //{
                    if (shoeEditVM!.ImageFile != null)
                    {
                        var permittedExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" }; //Estas van a ser las extesiones que voy a permitir subir, si no hay alguna que yo desee, la agrego
                        string fileExtension = Path.GetExtension(shoeEditVM.ImageFile.FileName); //Obtenemos la extension del nombre de nuestro archivo
                        if (!permittedExtensions.Contains(fileExtension)) // si la extension que obtuvimos de nuestro archivo no esta en los permitidos, va a tirar error y volver a la vista
                        {
                            ModelState.AddModelError(string.Empty, "File not allowed");
                            RercargarCombos(shoeEditVM);
                            return View(shoeEditVM);
                        }

                        if (shoeEditVM.imageURL != null)
                        {
                            string? oldPath = Path.Combine(webroot, shoe.imageURL!.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) //Me fijo si existe esta ruta
                            {
                                System.IO.File.Delete(oldPath);//La doy de baja porque voy a ingresar una nueva en el objeto
                            }
                        }
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(shoeEditVM.ImageFile.FileName)}";
                        string pathName = Path.Combine(webroot, "images", fileName); //Nombre de la ruta combinando el webroot que es la ruta web,el nombre del archivo y el string "images"
                        using (var filestream = new FileStream(pathName, FileMode.Create))
                        {
                            shoeEditVM.ImageFile.CopyTo(filestream); // Con esto subo mi imagen a la carpeta images en wwroot
                        }
                        shoe.imageURL = $"/images/{fileName}";
                }
                //}
                //else
                //{
                //    string? oldPath = Path.Combine(webroot, shoe.imageURL!.TrimStart('/'));
                //    if (System.IO.File.Exists(oldPath)) //Me fijo si existe esta ruta
                //    {
                //        System.IO.File.Delete(oldPath);//La doy de baja porque voy a ingresar una nueva en el objeto
                //    }
                //    shoe.imageURL = null;
                //}

                _service.Guardar(shoe);
                TempData["success"] = "Record successfully added/edited";
                return !string.IsNullOrEmpty(returnurl)
           ? Redirect(returnurl)
           : RedirectToAction("Index");
            }
            catch (Exception)
            {
                RercargarCombos(shoeEditVM);
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(shoeEditVM);
            }
        }
        private void CargarComboBoxs(ShoeEditVm shoeEditVm)
        {
            shoeEditVm.Brands = _serviceBrand
            .GetAll(orderBy: o => o.OrderBy(c => c.BrandName))
            .Select(c => new SelectListItem
            { Text = c.BrandName, Value = c.BrandId.ToString() }).ToList();

            shoeEditVm.Sports = _serviceSport
            .GetAll(orderBy: o => o.OrderBy(c => c.SportName))
            .Select(c => new SelectListItem
            { Text = c.SportName, Value = c.SportId.ToString() }).ToList();

            shoeEditVm.Genres = _serviceGenre
            .GetAll(orderBy: o => o.OrderBy(c => c.GenreName))
            .Select(c => new SelectListItem
            { Text = c.GenreName, Value = c.GenreId.ToString() }).ToList();

            shoeEditVm.Colours = _serviceColour
            .GetAll(orderBy: o => o.OrderBy(c => c.ColourName))
            .Select(c => new SelectListItem
            { Text = c.ColourName, Value = c.ColourId.ToString() }).ToList();



        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult UpSert(ShoeEditVm ShoeEditVm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        CargarComboBoxs(ShoeEditVm);
        //        return View(ShoeEditVm);
        //    }

        //    if (_service == null || _mapper == null)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
        //    }

        //    try
        //    {
        //        Shoe Shoe = _mapper.Map<Shoe>(ShoeEditVm);

        //        if (_service.Existe(Shoe))
        //        {
        //            ModelState.AddModelError(string.Empty, "Record already exist");
        //            CargarComboBoxs(ShoeEditVm);
        //            return View(ShoeEditVm);
        //        }

        //        _service.Guardar(Shoe);
        //        TempData["success"] = "Record successfully added/edited";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
        //        CargarComboBoxs(ShoeEditVm);

        //        return View(ShoeEditVm);
        //    }
        //}

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Shoe? shoe = _service?.Get(filter: g => g.ShoeId == id);
            if (shoe == null)
            {
                return NotFound();
            }
            List<ShoeSize> listaDeSizes = new List<ShoeSize>();
            listaDeSizes = _service!.GetAllShoesSizes(shoe.ShoeId);
            foreach (var item in listaDeSizes)
            {
                if (_service?.EstaRelacionado(shoe.ShoeId) ?? true)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Related Record..."
                    });
                }

            }
            if (shoe.imageURL != null)
            {
                string? webroot = webHostEnviroment!.WebRootPath;
                string? oldPath = Path.Combine(webroot, shoe.imageURL!.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) //Me fijo si existe esta ruta
                {
                    System.IO.File.Delete(oldPath);//La doy de baja porque voy a ingresar una nueva en el objeto
                }
            }
            _service.Eliminar(shoe);
            return Json(new { success = true, message = "Record deleted successfully" });


        }
            //public IActionResult Delete(int? id)
            //{
            //    if (id is null || id == 0)
            //    {
            //        return NotFound();
            //    }
            //    Shoe? Shoe = _service?.Get(filter: g => g.ShoeId == id);
            //    if (Shoe is null)
            //    {
            //        return NotFound();
            //    }
            //    try
            //    {
            //        if (_service == null || _mapper == null)
            //        {
            //            return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            //        }
            //        if (_service.EstaRelacionado(Shoe.ShoeId))
            //        {
            //            return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
            //        }
            //        _service.Eliminar(Shoe);
            //        return Json(new { success = true, message = "Record successfully deleted" });
            //    }
            //    catch (Exception)
            //    {
            //        return Json(new { success = false, message = "Couldn't delete record!!! " }); ;
            //    }
            //}

        public IActionResult AddSize(int id)
        {
            var shoe = _service!.Get(filter: s => s.ShoeId == id,
                propertiesNames: "Brand,Genre,Colour,Sport");
            if (shoe == null)
            {
                return NotFound();
            }

            var sizes = _serviceSize!
                .GetAll(orderBy: o => o.OrderBy(s => s.SizeNumber))!
                .Select(s => new SelectListItem
                {
                    Text = s.SizeNumber.ToString(),
                    Value = s.SizeId.ToString()
                }).ToList();

            var viewModel = new ShoeSizeVm
            {
                ShoeId = id,
                Brand = shoe.Brand!,
                Colour = shoe.Colour!,
                Genre = shoe.Genre!,
                Sport = shoe.Sport!,
                Sizes = sizes
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddSize(ShoeSizeVm model)
        {
            if (model.ShoeId <= 0 || model.SizeId <= 0 || model.Stock <= 0)
            {
                var shoe = _service!.Get(filter: s => s.ShoeId == model.ShoeId,
                    propertiesNames: "Brand,Genre,Colour,Sport");
                if (shoe == null)
                {
                    return NotFound();
                }
                model.Brand = _serviceBrand!.Get(filter: b => b.BrandId == shoe.BrandId)!;
                model.Colour = _serviceColour!.Get(filter: c => c.ColourId == shoe.ColourId)!;
                model.Genre = _serviceGenre!.Get(filter: g => g.GenreId == shoe.GenreId)!;
                model.Sport = _serviceSport!.Get(filter: sp => sp.SportId == shoe.SportId)!;
                ModelState.Remove("Brand");
                ModelState.Remove("Colour");
                ModelState.Remove("Genre");
                ModelState.Remove("Sport");
                model.Sizes = _serviceSize!
                    .GetAll(orderBy: o => o.OrderBy(s => s.SizeNumber))!
                    .Select(s => new SelectListItem
                    {
                        Text = s.SizeNumber.ToString(),
                        Value = s.SizeId.ToString()
                    }).ToList();
                if (model.Stock <= 0)
                {
                    ModelState.AddModelError("", "Debes colocar un número de stock ");
                }
                if (model.SizeId <= 0)
                {
                    ModelState.AddModelError("", "Debes colocar un número de talle.");
                }
                return View(model);
            }


            var shoeSizeBD = _serviceShoeSize!
                .Get(filter: ss => ss.ShoeId == model.ShoeId && ss.SizeId == model.SizeId);
            if (shoeSizeBD != null)
            {
                shoeSizeBD.QuantityInStock += model.Stock;
                _serviceShoeSize.Guardar(shoeSizeBD);
                TempData["success"] = "Existing relation found," +
                    " stock has been updated successfully.";
            }
            else
            {
                if (model.Stock > 0)
                {
                    var newShoeSize = new ShoeSize
                    {
                        ShoeId = model.ShoeId,
                        SizeId = model.SizeId,
                        QuantityInStock = model.Stock
                    };
                    _serviceShoeSize.Guardar(newShoeSize);
                    TempData["success"] = "New size relation added successfully!";
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Stock), "El stock debe ser mayor a 0.");
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
