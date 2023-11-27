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
                Title= bookEntity.Title,
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

            // Create a list to store the transformed transactions
            var transactionModels = new List<TransactionModel>();

            foreach (var transaction in transactions)
            {
                // Retrieve book information for each transaction
                var bookEntity = _context.Books
                    .Where(b => b.Idbook == transaction.Idbook)
                    .Include(b => b.IdauthorNavigation)
                    .Include(b => b.IdgenreNavigation)
                    .FirstOrDefault();

                if (bookEntity != null)
                {
                    // Map the book entity to your view model (BookModel)
                    var bookModel = new BookModel
                    {
                        Idbook = bookEntity.Idbook,
                        Title=bookEntity.Title,
                        ImagePath = bookEntity.ImagePath,
                        IdauthorNavigation = bookEntity.IdauthorNavigation,
                        IdgenreNavigation = bookEntity.IdgenreNavigation
                    };

                    // Map the transaction entity to your view model (TransactionModel)
                    var transactionModel = new TransactionModel
                    {
                        Idtransaction = transaction.Idtransaction,
                        Idbook = transaction.Idbook,
                        Date = transaction.Date,
                        Retrun = transaction.Retrun,
                        Status = transaction.Status,
                        Book = bookModel  // Assuming you have a navigation property in the TransactionModel for Book
                    };

                    // Add the transactionModel to the list
                    transactionModels.Add(transactionModel);
                }
            }

            // Pass the transactions to the view
            return View(transactionModels);
        }



    }
}
