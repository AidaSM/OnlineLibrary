using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles = "admin")]
    public class BookController : Controller
    {
        public Repository.BookRepository _repository;

        public BookController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.BookRepository(dbContext);
        }

        // GET: BookController
        public ActionResult Index()
        {
            var books = _repository.GetBooks();
            return View("Index", books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetBookByID(id);
            return View("DetailsBook", model);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            return View("CreateBook");
        }

        // POST: BookController/Create
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
        public async Task<IActionResult> Create([Bind("Title,Idauthor,Isbn,PublicationYear,Idgenre,Language,TotalCopies,AvailableCopies")] BookModel model)
        {
            try
            {
                _repository.InsertBook(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateBook");
            }

            return RedirectToAction(nameof(Index));

        }
        public ActionResult Edit(Guid id)
        {
            var book = _repository.GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View("EditBook", book);
        }


        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idbook, Title, Idauthor, Isbn, PublicationYear, Idgenre, Language, TotalCopies, AvailableCopies")] Models.BookModel model)
        {
            try
            {

                model.Idbook = id;

                _repository.UpdateBook(model);
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
            var book = _repository.GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View("DeleteBook", book);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteBook(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteBook");
            }
        }

    }
}
