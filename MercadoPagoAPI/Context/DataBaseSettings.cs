
namespace MercadoPagoAPI.Context;

public interface IDataBaseSettings
{
    public string UsersCollectionName { get; set; }
    public string ProductsCollectionName { get; set; }
    public string OrdersCollectionName  { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }

}

public class DataBaseSettings : IDataBaseSettings
{
    public string UsersCollectionName {get;set;}
    public string ProductsCollectionName { get; set; }
    public string OrdersCollectionName {get;set;}
    public string ConnectionString {get;set;}
    public string DatabaseName {get;set;}
}
