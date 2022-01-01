
using MercadoPagoAPI.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace MercadoPagoAPI.Entities;
public class User
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DocumentType DocumentType { get; set; }
    public string CpfCnpj { get; set; }
    public string Email { get; set; }
}
