﻿
using MongoDB.Bson.Serialization.Attributes;

namespace MercadoPagoAPI.Entities;
public class Product
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

}
