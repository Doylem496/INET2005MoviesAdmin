
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

// this will redirect you to login if you try and enter movies, genre etc.
[Authorize]
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
            .Include(m => m.CriticReviews)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }
        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");
        return View(movie);
    }

    // GET: MOVIES/Create
    // make this just for crud operations?
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        
        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");
        return View();
    }

    // POST: MOVIES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Title,GenreId,ReleaseDate,RuntimeMinutes,OriginCountry,Language,Synopsis,AudienceRatingId")] Movie movie, IFormFile? imageFile)
    {
        if (imageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Image of movie poster is required");
        }
        // for validation 
        ModelState.Remove(nameof(Movie.ImageURL));

        if (ModelState.IsValid)
        {

            var extension = Path.GetExtension(imageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string imageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName);
            movie.ImageURL = imageUrl;
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    // make this just for crud operations?
    [Authorize(Roles = "Admin")]
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

        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");
        return View(movie);
    }

    // POST: MOVIES/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    // added imagefile argument 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,GenreId,ReleaseDate,RuntimeMinutes,OriginCountry,Language,Synopsis,AudienceRatingId,ImageURL")] Movie movie, IFormFile? imageFile)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        // for validation 
        ModelState.Remove(nameof(Movie.ImageURL));

        // doesn't track returned entities in change tracker
        var currentMovie = await _context.Movie
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == movie.Id);

        if (currentMovie == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // added for editing images
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                movie.ImageURL = await _blobService.UploadFileAsync(imageFile,uniqueFileName);
            }
            else
            {
                movie.ImageURL = currentMovie.ImageURL;
            }

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
        
        var audienceRatings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(audienceRatings, "Id", "Title");
        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        return View(movie);
    }

    // GET: MOVIES/Delete/5

    
    [Authorize(Roles = "Admin")]
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
    

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
