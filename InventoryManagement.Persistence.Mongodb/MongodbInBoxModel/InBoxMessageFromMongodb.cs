using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryManagement.Persistence.Mongodb.MongodbInBoxModel;

public class InBoxMessageFromMongodb
{
    [BsonId]
    public Guid _id { get; set; }
    public DateTime HandledTime { get; set; }


}
