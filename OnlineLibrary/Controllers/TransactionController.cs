using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.Repository;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles = "admin")]
    public class TransactionController : Controller
    {
        public Repository.TransactionRepository _repository;

        public TransactionController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.TransactionRepository(dbContext);
        }
        // GET: TransactionController
        [AllowAnonymous]
        public ActionResult Index()
        {
            var transactions = _repository.GetTransactions();
            return View("Index", transactions);
        }

        // GET: TransactionController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetTransactionByID(id);
            return View("DetailsTransaction", model);
        }

        // GET: TransactionController/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            return View("CreateTransaction");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create([Bind("Idmember,Idbook,Date,Retrun,Status")] TransactionModel model)
        {
            try
            {
                _repository.InsertTransaction(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateTransaction");
            }

            return RedirectToAction(nameof(Index));

        }

        public ActionResult Edit(Guid id)
        {
            var transaction = _repository.GetTransactionByID(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View("EditTransaction", transaction);
        }

        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idtransaction, Idmember, Idbook, Date, Retrun, Status")] Models.TransactionModel model)
        {
            try
            {

                model.Idtransaction = id;

                _repository.UpdateTransaction(model);
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
            var transaction = _repository.GetTransactionByID(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View("DeleteTransaction", transaction);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteTransaction(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteTransaction");
            }
        }
        [HttpPost]
        public IActionResult ReserveBook(Guid idbook)
        {
            // Get the current user's ID
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a new transaction
            var newTransaction = new TransactionModel
            {
                Idbook = idbook,
                Idmember = userId,
                Date = DateTime.Now,
                Status = "Reserved" // You might want to adjust this based on your logic
            };

            // Insert the transaction into the database
            _repository.InsertTransaction(newTransaction);

            // You can redirect to a success page or return a different view
            return RedirectToAction("Index", "Home");
        }
    }
}
