using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using System.Data;

namespace OnlineLibrary.Controllers
{
   
    public class FeeController : Controller
    {
        public Repository.FeeRepository _repository;

        public FeeController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.FeeRepository(dbContext);
        }

        // GET: FeeController
        public ActionResult Index()
        {
            var fees = _repository.GetFees();
            return View("Index", fees);
        }

        // GET: FeeController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetFeeByID(id);
            return View("DetailsFee", model);
        }

        // GET: FeeController/Create
        public ActionResult Create()
        {
            return View("CreateFee");
        }

        // POST: FeeController/Create
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
        public async Task<IActionResult> Create([Bind("Idbook,Idmember,Description,IsPaid,Amount")] FeeModel model)
        {
            try
            {
                _repository.InsertFee(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("CreateFee");
            }

            return RedirectToAction(nameof(Index));

        }
        public ActionResult Edit(Guid id)
        {
            var fee = _repository.GetFeeByID(id);
            if (fee == null)
            {
                return NotFound();
            }

            return View("EditFee", fee);
        }


        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idfee, Idbook, Idmember, Description, IsPaid, Amount")] Models.FeeModel model)
        {
            try
            {

                model.Idfee = id;

                _repository.UpdateFee(model);
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
            var fee = _repository.GetFeeByID(id);
            if (fee == null)
            {
                return NotFound();
            }

            return View("DeleteFee", fee);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteFee(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteFee");
            }
        }
    }
}
