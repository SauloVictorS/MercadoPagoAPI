
using MercadoPagoAPI.Context;
using MercadoPagoAPI.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MercadoPagoAPI.Services;
public interface IProductService
{
    Task <List<Product>> GetAll();
    Task <Product> GetById(string id);
    Task<Product> Create(Product procuct);
    Task Update(string id, Product procuct);
    Task Delete(string id);
}

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _productCollection;
    public ProductService(IDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _productCollection = database.GetCollection<Product>(settings.ProductsCollectionName);
    }

    public async Task<Product> Create(Product procuct)
    {
        await _productCollection.InsertOneAsync(procuct);
        return procuct;
    }

    public async Task Delete(string id)
    {
        await _productCollection.DeleteOneAsync(e => e.Id == id);
    }

    public async Task<List<Product>> GetAll()
    {
        return await _productCollection.Find(e => true).ToListAsync();
    }

    public async Task<Product> GetById(string id)
    {
        return await _productCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task Update(string id, Product procuct)
    {
        await _productCollection.ReplaceOneAsync(e => e.Id == procuct.Id, procuct);
    }
}
