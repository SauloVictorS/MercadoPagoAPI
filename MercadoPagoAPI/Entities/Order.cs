
using MercadoPagoAPI.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MercadoPagoAPI.Entities;
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
    public string QrCode { get; set; }
    public string QrCodeBase64 { get; set; }
    public long? PixId { get; set; }
    public decimal Amount { get; set; }
    public Status Status { get; set; }
    [BsonIgnore]
    public User? User { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> ProductsIds { get; set; }
    [BsonIgnore]
    public List<Product> Products { get; set; }
}
