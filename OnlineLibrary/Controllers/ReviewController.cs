using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    
    public class ReviewController : Controller
    {
        public Repository.ReviewRepository _repository;

        public ReviewController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.ReviewRepository(dbContext);
        }
        // GET: ReviewController
      
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
        
        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View("CreateReview");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
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
        [HttpPost]
        public IActionResult InsertReview(Guid idbook, int rating, string text)
        {
            try
            {
                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a new review
                var newReview = new ReviewModel
                {
                    Idbook = idbook,
                    Idmember = userId,
                    Rating = rating,
                    Text = text,
                    Date = DateTime.Now
                };

                // Insert the review into the database
                _repository.InsertReview(newReview);

                // You can redirect to a success page or return a different view
                return RedirectToAction("Details", "Book", new { id = idbook }); // Redirect to the book details page
            }
            catch
            {
                // Handle the exception or return an error view
                return RedirectToAction("Index", "Home"); // Redirect to the home page for now
            }
        }

    }
}
