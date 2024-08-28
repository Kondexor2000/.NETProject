using Microsoft.AspNetCore.Mvc;
using NetflixSharp.Data;
using MyApp.ViewModels;
using MyApp.Models;
using System.Security.Claims;


[Route("Comments")]
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;

    public CommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    private IActionResult RequestUser(out string userId)
    {
        userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
        {
            return RedirectToAction("Login", "AccountController");
        }
        return null;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var images = _context.Comments
            .Select(o => new { o.Id, o.Title })
            .ToList();

        return View(images);
    } 

    [HttpGet("ReadComment/{id}")]
    public IActionResult ReadComment(int id)
    {
        var comment = _context.Comments
            .Where(o => o.Id == id)
            .Select(o => new { o.Id, o.UserId, o.Title })
            .SingleOrDefault(); 

        if (comment == null)
        {
            return RedirectToAction("AddComment");
        }

        return View(comment);
    }

    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment(Comments comment)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        comment.UserId = userId;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("ReadComment", new { id = comment.Id });
    }

    [HttpGet("UpdateComment/{id}")]
    public IActionResult UpdateComment(int id)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var comment = _context.Comments
            .FirstOrDefault(o => o.Id == id && o.UserId == userId);

        if (comment == null)
        {
            return RedirectToAction("AddComment");
        }

        var viewModel = new CommentUpdateView
        {
            Title = comment.Title,
        };

        return View(viewModel);
    }

    [HttpPost("UpdateComment")]
    public async Task<IActionResult> UpdateComment(CommentUpdateView model)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var comment = _context.Comments
            .FirstOrDefault(o => o.Id == model.SelectedPostId && o.UserId == userId);

        if (comment == null)
        {
            return RedirectToAction("AddComment");
        }

        if (ModelState.IsValid)
        {
            comment.Title = model.Title;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReadComment", new { id = comment.Id });
        }

        return View(model);
    }

    [HttpPost("DeleteComment/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var result = RequestUser(out string userId);
        if (result != null) return result;

        var comment = _context.Comments
            .FirstOrDefault(o => o.Id == id && o.UserId == userId);

        if (comment == null)
        {
            return RedirectToAction("Index");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}