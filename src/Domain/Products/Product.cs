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
}
