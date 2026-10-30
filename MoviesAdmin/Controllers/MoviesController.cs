
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

// this will redirect you to login if you try and enter movies, genre etc.

public class MoviesController : Controller
{
    // todo make image required 
    private readonly MoviesAdminContext _context;

    private readonly AzureStorageService _blobService;

    public MoviesController(MoviesAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index()    
    {
        // comment out for troubleshooting 
        var user = User.GetUserId();
        if (user != null)
        {
            Console.WriteLine(user);
        }
        List<Movie> movies = await _context.Movie
            .Include(m => m.Genre)
            .Include(m => m.AudienceRating)

           .OrderByDescending(m => m.ReleaseDate)
            .ToListAsync();
        
        return View(movies);
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        // new element, comment out until scaffolding done?
        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");
        return View();
    }

    // POST: MOVIES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]

    // changed genre to GenreId. Change back if broken

    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Title,GenreId,ReleaseDate,RuntimeMinutes,OriginCountry,Language,Synopsis,AudienceRatingId")] Movie movie, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {

            var extension = Path.GetExtension(imageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string imageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName);
            // replaced to utilize guid unique url
            // string imageUrl = await _blobService.UploadFileAsync(imageFile, imageFile.FileName);
            movie.ImageURL = imageUrl;
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: MOVIES/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,GenreId,ReleaseDate,RuntimeMinutes,OriginCountry,Language,Synopsis,AudienceRatingId,ImageUrl")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Delete/5

    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // make this just for crud operations?
    // [Authorize(Roles = "Admin")]

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
