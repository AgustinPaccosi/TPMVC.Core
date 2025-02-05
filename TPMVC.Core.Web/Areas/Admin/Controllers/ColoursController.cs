using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Colour;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ColoursController : Controller
    {
        private readonly IColoursService? _services;

        private readonly IMapper? _mapper;

        public ColoursController(IColoursService? services, IMapper mapper)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 8;
            var colours = _services.GetAll().ToPagedList(pageNum, pageSize);
            return View(colours);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ColourEditViewModel colourVM)
        {
            if (!ModelState.IsValid)
            {
                return View(colourVM);
            }
            Colour colour = _mapper.Map<Colour>(colourVM);
            if (colour is null)
            {
                ModelState.AddModelError(string.Empty, "Color es nulo");
                return View(colourVM);
            }
            if (_services.Existe(colour))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(colourVM);
            }
            _services.Guardar(colour);
            TempData["success"] = "Registro agregado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Colour colour = _services.Get(filter: c => c.ColourId == id);
            if (colour is null)
            {
                return NotFound();
            }
            ColourEditViewModel colourVM = _mapper.Map<ColourEditViewModel>(colour);
            return View(colourVM);
        }

        [HttpPost]
        public IActionResult Edit(ColourEditViewModel colourVM)
        {
            if (!ModelState.IsValid)
            {
                return View(colourVM);
            }
            Colour colour = _mapper.Map<Colour>(colourVM);
            if (colour is null)
            {
                ModelState.AddModelError(string.Empty, "Color es nulo");
                return View(colourVM);
            }
            if (_services.Existe(colour))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(colourVM);
            }
            _services.Guardar(colour);
            TempData["success"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Colour colour = _services.Get(filter: b => b.ColourId == id);
            if (colour is null)
            {
                return NotFound();
            }
            return View(colour);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Colour colour = _services.Get(filter: b => b.ColourId == id);
            if (colour is null)
            {
                return NotFound();
            }
            if (_services.EstaRelacionado(colour.ColourId))
            {
                ModelState.AddModelError(string.Empty, "El registro no puede borrarse, está relacionado");
                return View(colour);
            }
            _services.Eliminar(colour);
            TempData["success"] = "Registro eliminado correctamente";
            return RedirectToAction("Index");
        }

    }
}
