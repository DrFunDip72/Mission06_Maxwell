using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission__6_Assignment.Models;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Movies(Movie movie)
        {
            ViewBag.Categories = _context.Categories.ToList();

            // normalize optional strings
            movie.LentTo = movie.LentTo ?? "";
            movie.Notes = movie.Notes ?? "";

            // enforce required fields server-side
            if (string.IsNullOrWhiteSpace(movie.Title)) ModelState.AddModelError("Title", "Title is required.");
            if (movie.Year < 1888) ModelState.AddModelError("Year", "Year must be 1888 or later.");

            if (ModelState.IsValid)
            {
                try
                {
                    if (movie.MovieId > 0)
                    {
                        _context.Movies.Update(movie);
                        _context.SaveChanges();
                        TempData["SuccessMessage"] = "Movie updated successfully!";
                        return RedirectToAction("ListMovies");
                    }
                    else
                    {
                        _context.Movies.Add(movie);
                        _context.SaveChanges();
                        TempData["SuccessMessage"] = "Movie added successfully!";
                        return RedirectToAction("Movies");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the movie. Please try again.");
                }
            }

            // Collect ModelState errors for debugging display
            if (!ModelState.IsValid)
            {
                var errors = new System.Text.StringBuilder();
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        errors.AppendLine($"{key}: {error.ErrorMessage}");
                    }
                }
                TempData["ModelErrors"] = errors.ToString();

                // Capture posted form values for debugging
                try
                {
                    var posted = new System.Text.StringBuilder();
                    foreach (var k in Request.Form.Keys)
                    {
                        posted.AppendLine($"{k} = {Request.Form[k]}");
                    }
                    TempData["PostedValues"] = posted.ToString();
                }
                catch { }
            }

            return View(movie);
        }

        public IActionResult ListMovies()
        {
            // Linq
            var movies = _context.Movies
                .Include(m => m.Category)
                .OrderBy(x => x.Title).ToList(); // orders the applications by title

            return View(movies);
        }

        // Edit action that takes the user from the waitlist page to the Movies page with the selected movie's information pre-filled in the form
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();

            Movie recordToEdit = _context.Movies
                .Single(x => x.MovieId == id);

            return View("Movies", recordToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            // not used; handled by Movies POST
            return RedirectToAction("Movies", movie);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Movie recordToDelete = _context.Movies
                .Single(x => x.MovieId == id);
            return View(recordToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var recordToDelete = _context.Movies
                    .SingleOrDefault(x => x.MovieId == id);

                if (recordToDelete == null)
                {
                    TempData["ErrorMessage"] = "Movie not found.";
                    return RedirectToAction("ListMovies");
                }

                _context.Movies.Remove(recordToDelete);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Movie deleted successfully!";
                return RedirectToAction("ListMovies");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the movie. Please try again.";
                var movie = _context.Movies.SingleOrDefault(x => x.MovieId == id);
                return View(movie);
            }

        }
    }
}
