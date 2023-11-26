using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.Repository;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly BookRepository repository;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            repository = new BookRepository(context);
        }

        public IActionResult Index()
        {
            var books = repository.GetBooks();
            if (User.IsInRole("admin"))
            {
                return View("IndexAdmin", books);
            }
            return View("Index", books);
        }
        public IActionResult BookDetails(Guid id)
        {
            // Implement logic to retrieve details of the specified book
            var book = repository.GetBookByID(id);

            // Pass the book model to the view
            return View(book);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
