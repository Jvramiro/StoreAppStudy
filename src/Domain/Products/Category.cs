using Flunt.Validations;

namespace StoreAppStudy.Domain.Products; 
public class Category : Entity {

    public string name { get; set; }

    public Category(string name) {

        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "name");
        AddNotifications(contract);

        this.name = name;
    }

}
