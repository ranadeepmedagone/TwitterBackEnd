using Social.Models;
using Dapper;
using Social.Utilities;
using Social.Repositories;

namespace Social.Repositories;

public interface IPostRepository
{
    Task<Post> Create(Post Item);
    Task Update(Post Item);
    Task Delete(int Id);
    Task<List<Post>> GetAll();
    Task<Post> GetById(int UserId);

    Task<List<Post>> GetPostsByUserId(int userId);
}

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Post> Create(Post Item)
    {
        var query = $@"INSERT INTO {TableNames.post} (title,user_id)
        VALUES (@Title, @UserId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, Item);
    }

    public async Task Delete(int Id)
    {
        var query = $@"DELETE FROM {TableNames.post} WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<Post>> GetAll()
    {
        var query = $@"SELECT * FROM {TableNames.post} ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query)).AsList();
    }

    public async Task<Post> GetById(int PostId)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE id = @PostId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, new { PostId });
    }

    public async Task<List<Post>> GetPostsByUserId(int userId)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE user_id = @UserId ORDER BY id";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query, new { userId })).ToList();
    }

    public async Task Update(Post Item)
    {
        var query = $@"UPDATE {TableNames.post} SET title = @Title,
        updated_at =now() WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }
}