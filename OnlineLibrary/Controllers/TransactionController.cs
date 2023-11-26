using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class TransactionController : Controller
    {
        public Repository.TransactionRepository _repository;

        public TransactionController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.TransactionRepository(dbContext);
        }
        // GET: TransactionController
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
        public ActionResult Create()
        {
            return View("CreateTransaction");
        }

        // POST: TransactionController/Create
        /* [HttpPost]
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
    }
}
