using System.Data;
using Dapper;

namespace Framework.OutBoxPublisher;

public static  class DbUtil
{
    public static IEnumerable<OutBoxItem> GetOutBoxItems(this IDbConnection connection)
    {
        var sql = $@"SELECT * FROM  Outbox WHERE PublishedAt IS NULL";
        return connection.Query<OutBoxItem>(sql);
    }


    public static void UpdatePublishedDate(this IDbConnection connection,IEnumerable<long> outBoxItemIds)
    {
        var sql = $@"UPDATE  Outbox SET PublishedAt=@PublishAt WHERE Id IN @ids ";
        connection.Execute(sql, new { PublishAt=DateTime.Now, Ids = outBoxItemIds });
    }
}