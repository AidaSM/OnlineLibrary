using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

public class StatisticsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatisticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var genresWithFees = _context.Genres
            .Include(g => g.Books)
            .ThenInclude(b => b.Fees)
            .ToList();

        var genreModels = genresWithFees.Select(g => new GenreModel
        {
            Idgenre = g.Idgenre,
            Name = g.Name,
            TotalFees = g.Books.SelectMany(b => b.Fees).Sum(f => f.Amount),
            TotalCopies = g.Books.Sum(b => b.TotalCopies)
        }).ToList();


        return View(genreModels);
    }
}
