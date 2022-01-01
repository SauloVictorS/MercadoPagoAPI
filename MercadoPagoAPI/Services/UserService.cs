
using MercadoPagoAPI.Context;
using MercadoPagoAPI.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MercadoPagoAPI.Services;
public interface IUserService
{
    Task <List<User>> GetAll();
    Task <User> GetById(string id);
    Task<User> Create(User user);
    Task Update(string id, User user);
    Task Delete(string id);
}

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _userCollection;
    public UserService(IDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _userCollection = database.GetCollection<User>(settings.UsersCollectionName);
    }

    public async Task<User> Create(User user)
    {
        await _userCollection.InsertOneAsync(user);
        return user;
    }

    public async Task Delete(string id)
    {
        await _userCollection.DeleteOneAsync(e => e.Id == id);
    }

    public async Task<List<User>> GetAll()
    {
        return await _userCollection.Find(e => true).ToListAsync();
    }

    public async Task<User> GetById(string id)
    {
        return await _userCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task Update(string id, User user)
    {
        await _userCollection.ReplaceOneAsync(e => e.Id == user.Id,user);
    }
}
