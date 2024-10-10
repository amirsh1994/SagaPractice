using System.Text;
using Framework.Core.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Framework.Persistence.Ef;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TransactionalBoxInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
}

public class TransactionalBoxInterceptor:SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context;
        if (context is null)
        {
            return result;
        }
        var outBox = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var domainEvents = x.GetChanges();
                return domainEvents;
            }).ToList();

        OutboxDbUtil.CreateTableIfNotExist(context);

        var sb = new StringBuilder();
        sb.Append($"INSERT INTO OUTBOX(EventId,EventBody,EventType) VALUES");
        var paramItems = new List<SqlParameter>();
        for (var i = 0; i < outBox.Count; i++)
        {
            paramItems.Add(new SqlParameter($"@EventId{i}", outBox[i].EventId.ToString()));
            paramItems.Add(new SqlParameter($"@EventBody{i}", JsonConvert.SerializeObject(outBox[i])));
            paramItems.Add(new SqlParameter($"@EventType{i}", outBox[i].GetType().AssemblyQualifiedName));
            sb.AppendLine($"(@EventId{i} , @EventBody{i}, @EventType{i})");
            if (i != outBox.Count - 1)
            {
                sb.Append(",");
            }
        }

        context.Database.ExecuteSqlRaw(sb.ToString(), paramItems.ToArray());
        return base.SavedChanges(eventData, result);
    }
}