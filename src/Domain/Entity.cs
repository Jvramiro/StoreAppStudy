using Flunt.Notifications;

namespace StoreAppStudy.Domain;

public abstract class Entity : Notifiable<Notification> {
    public Entity() { 
        id = new Guid();
    }
    public Guid id { get; set; }
}
