using Social.Models;
using Social.Repositories;
using Microsoft.AspNetCore.Mvc;
using Social.DTOs;
using Microsoft.AspNetCore.Authorization;
using Social.Utilities;
using System.Security.Claims;

namespace Social.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _Comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository Comment)
    {
        _logger = logger;
        _Comment = Comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == Constants.Id).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment([FromQuery] int id, [FromBody] CommentCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        int PostId = id;

        var toCreateItem = new Comment
        {
            Commenttext = Data.Commenttext.Trim(),
            UserId = userId,
            PostId = PostId
        };
        var createdItem = await _Comment.Create(toCreateItem);
        return StatusCode(201, createdItem);
    }


    [HttpDelete("id")]
    public async Task<ActionResult> DeleteComment([FromQuery] int id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _Comment.GetById(id);
        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Comment");

        await _Comment.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetAllComments([FromQuery] int id)
    {
        var allComment = await _Comment.GetAll(id);
        return Ok(allComment);
    }
}

