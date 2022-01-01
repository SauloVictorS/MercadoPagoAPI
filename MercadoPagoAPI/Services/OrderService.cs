
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPagoAPI.Context;
using MercadoPagoAPI.Entities;
using MercadoPagoAPI.Enums;
using MercadoPagoAPI.Helpers;
using MercadoPagoAPI.Models.Order;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoPagoAPI.Services;
public interface IOrderService
{
    Task<List<Order>> GetAll();
    Task<Order> GetById(string id);
    Task<Order> Create(Order order);
    Task Refaund(RefaundRequest request);
    Task Cancel(string orderId);
    Task Update(string id, Order order);
    Task Delete(string id);
}

public class OrderService : IOrderService
{
    private readonly IMongoCollection<Order> _orderCollection;
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IUserService _userService;
    public OrderService(IDataBaseSettings settings, IUserService userService)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _orderCollection = database.GetCollection<Order>(settings.OrdersCollectionName);
        _productCollection = database.GetCollection<Product>(settings.ProductsCollectionName);

        _userService = userService;
    }


    public async Task<Order> Create(Order order)
    {


        List<Product> products = await _productCollection
            .Find(e => order.ProductsIds.Contains(e.Id))
            .ToListAsync();

        order.Status = Status.Pending;
        order.Amount = products
            .Select(e => e.Price)
            .Sum();

        await _orderCollection.InsertOneAsync(order);

        MercadoPagoConfig.AccessToken = Constants.MercadoPagoAccessToken;
        User user = await _userService.GetById(order.UserId);

        var request = new PaymentCreateRequest
        {
            TransactionAmount = order.Amount,
            Description = order.Id,
            PaymentMethodId = "pix",
            Payer = new PaymentPayerRequest
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Identification = new IdentificationRequest
                {
                    Type = user.DocumentType.ToString(),
                    Number = user.CpfCnpj,
                },
            },
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(request);

        order.PixId = payment.Id;
        order.QrCode = payment.PointOfInteraction.TransactionData.QrCode;
        order.QrCodeBase64 = payment.PointOfInteraction.TransactionData.QrCodeBase64;
        await Update(order.Id, order);
        return order;
    }

    public async Task Delete(string id)
    {
        await _orderCollection.DeleteOneAsync(e => e.Id == id);
    }

    public async Task<List<Order>> GetAll()
    {
        return await _orderCollection.Find(e => true).ToListAsync();
    }

    public async Task<Order> GetById(string id)
    {
        Order order = await _orderCollection.Find(e => e.Id == id).SingleOrDefaultAsync();

        order.Products = await _productCollection
            .Find(e => order.ProductsIds.Contains(e.Id))
            .ToListAsync();

        order.User = await _userService.GetById(order.UserId);

        return order;
    }

    public async Task Refaund(RefaundRequest request)
    {
        Order order = await GetById(request.OrderId);
        if (order == null )
        {
            throw new Exception("Order not found or not approved!");
        }
        MercadoPagoConfig.AccessToken = Constants.MercadoPagoAccessToken;
        var client = new PaymentClient();
        await client.RefundAsync(request.PixId, request.Amount);

        order.Status = Status.Refunded;
        await Update(order.Id, order);
    }

    public async Task Cancel(string orderId)
    {
        Order order = await _orderCollection.Find(e => e.Id == orderId).FirstOrDefaultAsync();
        if(order == null )
        {
            throw new Exception("Order not found or not approved!");
        }

        MercadoPagoConfig.AccessToken = Constants.MercadoPagoAccessToken;
        var client = new PaymentClient();
        await client.CancelAsync(order.PixId ?? 0);

        order.Status = Status.Canceled;
        await Update(order.Id, order);
    }


    public async Task Update(string id, Order order)
    {
        await _orderCollection.ReplaceOneAsync(e => e.Id == order.Id, order);
    }
}
