using Flunt.Validations;

namespace StoreAppStudy.Domain.Products; 
public class Category : Entity {

    public string name { get; set; }
    public string createdBy { get; set; }
    public string editedBy { get; set; }

    public Category(string name, string createdBy, string editedBy) {

        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "name");
        AddNotifications(contract);

        this.name = name;
        this.editedBy = editedBy;
        this.createdBy = createdBy;
    }

}
