using System.Data;
using System.Reflection;
using Framework.Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.OutBoxPublisher;

public class OutBoxManager(IDbConnection connection, IEventBus eventBus)
{
    public void Start(Assembly[] assemblies)
    {

        var outBoxes = connection.GetOutBoxItems().ToList();
        foreach (var item in outBoxes)
        {
            eventBus.Publish(ToEvent(item));
            Console.WriteLine($"Publish Outbox :{item.EventBody}");
        }
        var ids = outBoxes.Select(x => x.Id);
        connection.UpdatePublishedDate(ids);
    }

    private object? ToEvent(OutBoxItem item)
    {
        var type = Type.GetType(item.EventType);
        var @event = JsonConvert.DeserializeObject(item.EventBody, type, new JsonSerializerSettings
        {
            ContractResolver = new NonPublicPropertiesResolver()
        });
        return @event;
    }
}

public class NonPublicPropertiesResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {

        var prop = base.CreateProperty(member, memberSerialization);

        if (member is not PropertyInfo pio) return prop;
        prop.Readable = (pio.GetMethod != null);
        prop.Writable = (pio.SetMethod != null);
        return prop;
    }
}