using Flunt.Validations;

namespace StoreAppStudy.Domain.Products;

public class Product : Entity {
    public string name { get; set; }
    public float value { get; set; }
    public string description { get; set; }
    public Guid categoryId { get; set; }
    public Category category { get; set; }
    public bool hasStock { get; set; }
    public string createdBy { get; set; }
    public DateTime createdOn { get; set; }
    public string editedBy { get; set; }
    public DateTime editedOn { get; set; }
    public decimal price { get; set; }

    public Product(string name, Guid categoryId, string description, bool hasStock, string createdBy, string editedBy, decimal price) {

        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "name");
        AddNotifications(contract);

        this.name = name;
        this.categoryId = categoryId;
        this.description = description;
        this.hasStock = hasStock;
        this.editedBy = editedBy;
        this.createdBy = createdBy;
        this.price = price;

        editedOn = DateTime.UtcNow;
    }
}
