using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
   
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Repository.AuthorRepository _repository;

        public AuthorController (ApplicationDbContext dbContext)
        {
            _repository = new Repository.AuthorRepository(dbContext);
        }

        // GET: AuthorController
        public ActionResult Index()
        {
            var authors = _repository.GetAuthors();
            return View("Index",authors);
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetAuthorByID(id);
            return View("DetailsAuthor",model);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View("CreateAuthor");
        }

        // POST: AuthorController/Create
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.AuthorModel model = new Models.AuthorModel();
                var task=TryUpdateModelAsync(model);
                task.Wait();
                if(task.Result)
                {
                    _repository.InsertAuthor(model);
                }
                return View("CreateAuthor");
            }
            catch
            {
                return View("CreateAuthor");
            }
            return RedirectToAction(nameof(Index));

        }*/
        // GET: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BirthDate,Nationality")] AuthorModel model)
        {
            try
            {
                _repository.InsertAuthor(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateAuthor");
            }

            return RedirectToAction(nameof(Index));

        }
        public ActionResult Edit(Guid id)
        {
            var author = _repository.GetAuthorByID(id);
            if (author == null)
            {
                return NotFound();
            }

            return View("EditAuthor", author);
        }


        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idauthor, Name, BirthDate, Nationality")] Models.AuthorModel model)
        {
            try
            {

                model.Idauthor = id;

                _repository.UpdateAuthor(model);
                return RedirectToAction("Index");


            }
            catch
            {
                return RedirectToAction("Index", id);
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: AuthorController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var author = _repository.GetAuthorByID(id);
            if (author == null)
            {
                return NotFound();
            }

            return View("DeleteAuthor", author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteAuthor(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteAuthor");
            }
        }
       

    }
}
