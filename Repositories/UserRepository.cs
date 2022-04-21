using Social.Models;
using Dapper;
using Social.Utilities;

namespace Social.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmail(string Email);
    Task<User> Create(User Item);
    Task<bool> Update(User Item);
    Task<User> GetUserById(int Id);


}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<User> Create(User Item)
    {
        var query = @$"INSERT INTO {TableNames.users} (full_name, email, password)
        VALUES (@FullName, @Email, @Password) RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, Item);
            return res;
        }
    }

    public async Task<User> GetByEmail(string Email)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE email = @Email";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
    }


    public async Task<User> GetUserById(int Id)
    {
        var query = $@"SELECT * FROM {TableNames.users} WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Id });
    }

    public async Task<bool> Update(User Item)
    {
        var query = $@"UPDATE ""{TableNames.users}"" SET full_name = @FullName WHERE id = @Id";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;

        }
    }


}