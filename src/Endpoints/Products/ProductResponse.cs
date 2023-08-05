namespace StoreAppStudy.Endpoints.Products;

public record ProductResponse(string Name, Guid CategoryId, string Description, bool HasStock, decimal Price);
