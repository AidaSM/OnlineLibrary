using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // Retrieve the book entity from the database
            var bookEntity = _context.Books
                .Where(b => b.Idbook == id)
                .Include(b => b.IdauthorNavigation) // Include related author
                .Include(b => b.IdgenreNavigation)  // Include related genre
                .FirstOrDefault();

            // Check if the book entity is found
            if (bookEntity == null)
            {
                return NotFound();
            }

            // Map the entity to your view model (BookModel)
            var bookModel = new BookModel
            {
                Idbook = bookEntity.Idbook,
                ImagePath= bookEntity.ImagePath,
                IdauthorNavigation = bookEntity.IdauthorNavigation,
                IdgenreNavigation = bookEntity.IdgenreNavigation
            };

            return View(bookModel);
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
