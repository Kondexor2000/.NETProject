using Microsoft.AspNetCore.Mvc;
using NetflixSharp.Data;
using MyApp.Models;
using MyApp.ViewModels;
using System.Security.Claims;


[Route("")]
public class PostController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostController(ApplicationDbContext context)
    {
        _context = context;
    }

    private IActionResult RequestUser(out string userId)
    {
        userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        return null;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var posts = _context.Posts
            .Select(o => new { o.Id, o.Title, o.Description, o.Comments })
            .ToList();

        return View(posts);
    }

    [HttpGet("ReadPost/{id}")]
    public IActionResult ReadPost(int id)
    {
        var post = _context.Posts
            .Where(o => o.Id == id)
            .Select(o => new { o.Id, o.Title, o.Description, o.Comments })
            .SingleOrDefault();

        if (post == null)
        {
            return RedirectToAction("Index");
        }

        return View(post);
    }

    [HttpPost("AddPost")]
    public async Task<IActionResult> AddPost(PostUpdateView model)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var post = new Posts
        {
            UserId = userId,
            Title = model.Title,
            Description = model.Description,
            CategoryId = model.SelectedCategoryId
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("ReadPost", new { id = post.Id });
    }

    [HttpGet("UpdatePost/{id}")]
    public IActionResult UpdatePost(int id)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var post = _context.Posts
            .FirstOrDefault(o => o.Id == id && o.UserId == userId);

        if (post == null)
        {
            return RedirectToAction("AddPost");
        }

        var viewModel = new PostUpdateView
        {
            SelectedPostId = post.Id,
            Title = post.Title,
            Description = post.Description
        };

        return View(viewModel);
    }

    [HttpPost("UpdatePost/{id}")]
    public async Task<IActionResult> UpdatePost(int id, PostUpdateView model)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var post = _context.Posts
            .Where(o => o.Id == id && o.UserId == userId)
            .SingleOrDefault();

        if (post == null)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            post.Id = model.SelectedPostId;
            post.Title = model.Title;
            post.Description = model.Description;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReadPost", new { id = post.Id });
        }

        return View(model);
    }

    [HttpPost("DeletePost/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var post = _context.Posts
            .Where(o => o.Id == id && o.UserId == userId)
            .SingleOrDefault();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}