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
        private readonly TransactionRepository transactionRepository;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            repository = new BookRepository(context);
            transactionRepository = new TransactionRepository(context);
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
        public IActionResult Details(Guid id)
        {
            // Get the book details (you may need to modify this based on your data structure)
            var book = repository.GetBookByID(id);

            if (book == null)
            {
                return NotFound();
            }

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
        public IActionResult MyTransactions()
        {
            // Retrieve the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user's transactions
            var transactions = transactionRepository.GetTransactionByMember(userId);

            // Pass the transactions to the view
            return View(transactions);
        }


    }
}
