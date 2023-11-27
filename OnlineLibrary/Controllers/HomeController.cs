using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.Models.DBObjects;
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
        private readonly FeeRepository feeRepository;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            repository = new BookRepository(context);
            transactionRepository = new TransactionRepository(context);
            feeRepository = new FeeRepository(context);
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

        /*public IActionResult Details(Guid id)
        {
            // Retrieve the book entity from the database
            var bookEntity = _context.Books
                .Where(b => b.Idbook == id)
                .Include(b => b.IdauthorNavigation) // Include related author
                .Include(b => b.IdgenreNavigation)  // Include related genre
                .Include(b => b.Reviews)  // Include related reviews
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
                Title = bookEntity.Title,
                ImagePath = bookEntity.ImagePath,
                IdauthorNavigation = bookEntity.IdauthorNavigation,
                IdgenreNavigation = bookEntity.IdgenreNavigation,
                Reviews = bookEntity.Reviews.ToList()  // Assuming Reviews is a navigation property in BookModel
            };

            return View(bookModel);
        }
        */
        public IActionResult Details(Guid id)
        {
            // Retrieve the book entity from the database
            var bookEntity = _context.Books
                .Where(b => b.Idbook == id)
                .Include(b => b.IdauthorNavigation)
                .Include(b => b.IdgenreNavigation)
                .Include(b => b.Reviews)
                    .ThenInclude(r => r.IdmemberNavigation) // Include related member for each review
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
                Title = bookEntity.Title,
                ImagePath = bookEntity.ImagePath,
                IdauthorNavigation = bookEntity.IdauthorNavigation,
                IdgenreNavigation = bookEntity.IdgenreNavigation,
                Reviews = bookEntity.Reviews.ToList()
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
                        Return = transaction.Return,
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
        public IActionResult Search(string query)
        {
            var searchResults = _context.Books
                .Where(b => b.Title.Contains(query))
                .Include(b => b.IdauthorNavigation) 
                .Include(b => b.IdgenreNavigation)  
                .Select(b => new BookModel
                {
                    Idbook = b.Idbook,
                    Title = b.Title,
                    Idauthor = b.Idauthor,
                    Isbn = b.Isbn,
                    PublicationYear = b.PublicationYear,
                    Idgenre = b.Idgenre,
                    Language = b.Language,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    ImagePath = b.ImagePath,
                    IdauthorNavigation = b.IdauthorNavigation, // Populate the Author navigation property
                    IdgenreNavigation = b.IdgenreNavigation,
                    // Add other necessary property mappings
                })
                .ToList();

            return View(searchResults);
        }

        public IActionResult MyFees()
        {
            // Retrieve the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user's fees
            var fees = feeRepository.GetFeeByMember(userId);

            // Create a list to store the transformed fees
            var feeModels = new List<FeeModel>();

            foreach (var fee in fees)
            {
                // Retrieve book information for each fee
                var bookEntity = _context.Books
                    .Where(b => b.Idbook == fee.Idbook)
                    .Include(b => b.IdauthorNavigation)
                    .Include(b => b.IdgenreNavigation)
                    .FirstOrDefault();

                if (bookEntity != null)
                {
                    // Map the book entity to your view model (BookModel)
                    var bookModel = new BookModel
                    {
                        Idbook = bookEntity.Idbook,
                        Title = bookEntity.Title,
                        ImagePath = bookEntity.ImagePath,
                        IdauthorNavigation = bookEntity.IdauthorNavigation,
                        IdgenreNavigation = bookEntity.IdgenreNavigation
                    };

                    // Map the fee entity to your view model (FeeModel)
                    var feeModel = new FeeModel
                    {
                        Idfee = fee.Idfee,
                        Idbook = fee.Idbook,
                        Idmember = fee.Idmember,
                        Description = fee.Description,
                        IsPaid = fee.IsPaid,
                        Amount = fee.Amount,
                        IdbookNavigation = bookModel,  // Update to use BookModel
                        IdmemberNavigation = fee.IdmemberNavigation
                    };

                    // Add the feeModel to the list
                    feeModels.Add(feeModel);
                }
            }

            // Pass the fees to the view
            return View(feeModels);
        }


    }
}
