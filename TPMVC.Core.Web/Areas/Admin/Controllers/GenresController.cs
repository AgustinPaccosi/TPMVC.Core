using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Core.Services.Interfaces;
using TPMVC.Core.Entities;
using TPMVC.Core.Web.ViewModels.Genre;
using X.PagedList.Extensions;

namespace TPMVC.Core.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class GenresController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IGenresService? _services;

        private readonly IMapper? _mapper;

        public GenresController(IGenresService? services, IMapper mapper)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 8;
            var genres = _services.GetAll().ToPagedList(pageNum, pageSize);
            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GenreEditViewModel genreVM)
        {
            if (!ModelState.IsValid)
            {
                return View(genreVM);
            }
            Genre genre = _mapper.Map<Genre>(genreVM);
            if (genre is null)
            {
                ModelState.AddModelError(string.Empty, "El genero es nulo");
                return View(genreVM);
            }
            if (_services.Existe(genre))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(genreVM);
            }
            _services.Guardar(genre);
            TempData["success"] = "Registro agregado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Genre genre = _services.Get(filter: b => b.GenreId == id);
            if (genre is null)
            {
                return NotFound();
            }
            GenreEditViewModel genreVM = _mapper.Map<GenreEditViewModel>(genre);
            return View(genreVM);
        }

        [HttpPost]
        public IActionResult Edit(GenreEditViewModel genreVM)
        {
            if (!ModelState.IsValid)
            {
                return View(genreVM);
            }
            Genre genre = _mapper.Map<Genre>(genreVM);
            if (genre is null)
            {
                ModelState.AddModelError(string.Empty, "El genero es nula");
                return View(genreVM);
            }
            if (_services.Existe(genre))
            {
                ModelState.AddModelError(string.Empty, "El registro ya existe");
                return View(genreVM);
            }
            _services.Guardar(genre);
            TempData["success"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Genre genre = _services.Get(filter: b => b.GenreId == id);
            if (genre is null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Genre genre = _services.Get(filter: b => b.GenreId == id);
            if (genre is null)
            {
                return NotFound();
            }
            if (_services.EstaRelacionado(genre.GenreId))
            {
                ModelState.AddModelError(string.Empty, "El registro no puede borrarse, está relacionado");
                return View(genre);
            }
            _services.Eliminar(genre);
            TempData["success"] = "Registro eliminado";
            return RedirectToAction("Index");
        }

    }
}
