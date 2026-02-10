using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission__6_Assignment.Models;

namespace Mission__6_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private MoviesContext _context;
        public HomeController(MoviesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Movies()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Movies(Movie movie)
        {
            // Set empty strings for optional fields if they're null
            movie.LentTo = movie.LentTo ?? "";
            movie.Notes = movie.Notes ?? "";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Movies.Add(movie);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Movie added successfully!";
                    return RedirectToAction("Movies");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the movie. Please try again.");
                }
            }
            return View(movie);
        }

    }
}
