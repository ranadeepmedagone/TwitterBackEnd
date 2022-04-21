using Social.Repositories;
using Microsoft.AspNetCore.Mvc;
using Social.DTOs;
using Microsoft.AspNetCore.Authorization;
using Social.Utilities;
using System.Security.Claims;
using Social.Models;

namespace Social.Controllers;

[ApiController]
[Authorize]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository _post;

    public PostController(ILogger<PostController> logger,
    IPostRepository post)
    {
        _logger = logger;
        _post = post;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == Constants.Id).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var post = await _post.GetPostsByUserId(userId);
        if (post.Count >= 5)
            return BadRequest("You can't create more then five posts");


        var toCreateItem = new Post
        {
            Title = Data.Title.Trim(),
            UserId = userId,
        };

        var createdItem = await _post.Create(toCreateItem);

        return StatusCode(201, createdItem);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<Post>> GetById([FromRoute] int id)
    {
        var post = await _post.GetById(id);
        return Ok(post);
    }

    [HttpPut("{post_id}")]
    public async Task<ActionResult> UpdatePost([FromRoute] int post_id,
    [FromBody] PostUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _post.GetById(post_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other's TODO");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
        };

        await _post.Update(toUpdateItem);

        return NoContent();
    }

    [HttpDelete("{post_id}")]
    public async Task<ActionResult> DeletePost([FromRoute] int post_id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _post.GetById(post_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Post");

        await _post.Delete(post_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAllPost()
    {
        var allPost = await _post.GetAll();
        return Ok(allPost);
    }
}