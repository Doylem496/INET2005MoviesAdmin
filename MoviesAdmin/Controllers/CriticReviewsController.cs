
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;


[Authorize(Roles = "Critic")]
public class CriticReviewsController : Controller
{
    
    private MoviesAdminContext _context;


    
    private string? _userId;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        _userId = User.GetUserId();
        
    }

    // GET: CRITICREVIEWS
    public async Task<IActionResult> Index()    
    {
        
        var reviews = await _context.CriticReviews
            // how users see their own reviews
            .Where(m => m.CreatedBy == _userId)
            .Include(m => m.Movie)
            .ToListAsync();

        // return View(await _context.CriticReviews.ToListAsync());
        return View(reviews);

    }

    // GET: CRITICREVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReviews
            .Where(m => m.CreatedBy == _userId)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (criticReview == null)
        {
            return NotFound();
        }
        // so users can't modify other user's reviews
        if (criticReview.CreatedBy != _userId)
        {
            return NotFound();
        }

        return View(criticReview);
    }

    // GET: CRITICREVIEWS/Create
    // int or int? parameter
    public IActionResult Create(int movieId)
    {
        ViewBag.MovieId = movieId;
        return View();
    }

    // POST: CRITICREVIEWS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Description,StarRating,IsPublished")] CriticReview criticReview)
    {
        // double check this!
        criticReview.CreatedBy = _userId!;
        criticReview.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

        // helps with validation
        ModelState.Remove(nameof(CriticReview.CreatedBy));
        ModelState.Remove(nameof(CriticReview.CreatedDate));

        if (ModelState.IsValid)
        {
            // check if they already reviewed, compare movieId and CreatedBy properties past and current 
            // anyAsync is for checking collections for specific elements
            bool alreadyReviewed = await _context.CriticReviews.AnyAsync(m => m.MovieId == criticReview.MovieId &&
            m.CreatedBy == _userId);

            if (alreadyReviewed)
            {
                ModelState.AddModelError("", "Users can only post one review per movie");
                return View(criticReview);
            }

            _context.Add(criticReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(criticReview);
    }

    // GET: CRITICREVIEWS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReviews.FindAsync(id);
        if (criticReview == null)
        {
            return NotFound();
        }
        // so users can't modify other user's reviews
        if (criticReview.CreatedBy != _userId)
        {
            return NotFound();
        }


        return View(criticReview);
    }

    // POST: CRITICREVIEWS/Edit/5
  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,Description,StarRating,IsPublished")] CriticReview criticReview)
    {
        if (id != criticReview.Id)
        {
            return NotFound();
        }


        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(criticReview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriticReviewExists(criticReview.Id))
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
        return View(criticReview);
    }

    // GET: CRITICREVIEWS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReviews
            .FirstOrDefaultAsync(m => m.Id == id);
        if (criticreview == null)
        {
            return NotFound();
        }

        return View(criticreview);
    }

    // POST: CRITICREVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var criticreview = await _context.CriticReviews.FindAsync(id);
        if (criticreview != null)
        {
            _context.CriticReviews.Remove(criticreview);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CriticReviewExists(int? id)
    {
        return _context.CriticReviews.Any(e => e.Id == id);
    }
}
