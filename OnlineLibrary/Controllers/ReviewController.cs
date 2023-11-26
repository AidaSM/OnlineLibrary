using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReviewController : Controller
    {
        public Repository.ReviewRepository _repository;

        public ReviewController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.ReviewRepository(dbContext);
        }
        // GET: ReviewController
        [AllowAnonymous]
        public ActionResult Index()
        {
            var reviews = _repository.GetReviews();
            return View("Index", reviews);
        }

        // GET: ReviewController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetReviewByID(id);
            return View("DetailsReview", model);
        }
        [Authorize(Roles = "User, Admin")]
        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View("CreateReview");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create([Bind("Idmember,Idbook,Rating,Text,Date")] ReviewModel model)
        {
            try
            {
                _repository.InsertReview(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateMember");
            }

            return RedirectToAction(nameof(Index));

        }
        public ActionResult Edit(Guid id)
        {
            var review = _repository.GetReviewByID(id);
            if (review == null)
            {
                return NotFound();
            }

            return View("EditReview", review);
        }

        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idreview, Idmember, Idbook, Rating, Text, Date")] Models.ReviewModel model)
        {
            try
            {

                model.Idreview = id;

                _repository.UpdateReview(model);
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
            var review = _repository.GetReviewByID(id);
            if (review == null)
            {
                return NotFound();
            }

            return View("DeleteReview", review);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteReview(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteReview");
            }
        }
    }
}
