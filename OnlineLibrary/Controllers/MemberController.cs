using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class MemberController : Controller
    {
        public Repository.MemberRepository _repository;

        public MemberController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.MemberRepository(dbContext);
        }
        // GET: MemberController
        public ActionResult Index()
        {
            var members = _repository.GetMembers();
            return View("Index", members);
        }

        // GET: MemberController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetMemberByID(id);
            return View("DetailsMember", model);
        }

        // GET: MemberController/Create
        public ActionResult Create()
        {
            return View("CreateMember");
        }

        // POST: MemberController/Create
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
        public async Task<IActionResult> Create([Bind("Username,Password,Email,RegistrationDate")] MemberModel model)
        {
            try
            {
                _repository.InsertMember(model);
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
            var member = _repository.GetMemberByID(id);
            if (member == null)
            {
                return NotFound();
            }

            return View("EditMember", member);
        }


        // POST: TvaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("Idmember, Username, Password, Email, RegistrationDate")] Models.MemberModel model)
        {
            try
            {

                model.Idmember = id;

                _repository.UpdateMember(model);
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
            var member = _repository.GetMemberByID(id);
            if (member == null)
            {
                return NotFound();
            }

            return View("DeleteMember", member);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteMember(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeleteMember");
            }
        }
    }
}
