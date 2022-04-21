// using Social.DTOs;

namespace Social.Models;

public record Comment
{
    public int Id { get; set; }
    public string Commenttext { get; set; }
    public int UserId { get; set; }

    public int PostId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }



    // public CommentDTO asDto => new CommentDTO
    // {
    //     Commenttext = Commenttext,
    //     CreatedAt = CreatedAt,
    //     UpdatedAt = UpdatedAt,
    //     UserId = UserId,
    //     PostId = PostId,
    // };
}