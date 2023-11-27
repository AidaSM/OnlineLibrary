using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    
    public class GenreController : Controller
    {
        public Repository.GenreRepository _repository;

        public GenreController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.GenreRepository(dbContext);
        }

        // GET: GenreController
        public ActionResult Index()
        {
            var genre = _repository.GetGenres();
            return View("Index", genre);
        }

        // GET: GenreController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetGenreByID(id);
            return View("DetailsGenre", model);
        }

        // GET: GenreController/Create
        public ActionResult Create()
        {
            return View("CreateGenre");
        }

        // POST: GenreController/Create
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] GenreModel model)
        {
            try
            {
                _repository.InsertGenre(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateGenre");
            }

            return RedirectToAction(nameof(Index));

        }

        public ActionResult Edit(Guid id)
        {
            var genre = _repository.GetGenreByID(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View("EditGenre", genre);
        }


        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Name")] Models.GenreModel model)
        {
            try
            {

                model.Idgenre = id;

                _repository.UpdateGenre(model);
                return RedirectToAction("Index");


            }
            catch
            {
                return RedirectToAction("Index", id);
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Delete(Guid id)
        {
            var genre = _repository.GetGenreByID(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View("DeleteGenre", genre);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteGenre(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteGenre");
            }
        }
    }
}
