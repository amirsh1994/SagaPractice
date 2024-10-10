using Microsoft.EntityFrameworkCore;

namespace Framework.Persistence.Ef;

public static class OutboxDbUtil
{
    
    public static void CreateTableIfNotExist(DbContext context)
    {
        var sql = @"SET ANSI_NULLS ON
   SET QUOTED_IDENTIFIER ON
   
   IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Outbox]') AND type in (N'U'))
   BEGIN
       CREATE TABLE [dbo].[Outbox]
       (
           [Id] [bigint] IDENTITY(1,1) NOT NULL,
           [EventType] [nvarchar](500) NOT NULL,
           [EventBody] [nvarchar](1000) NOT NULL,
           [PublishedAt] [datetime] NULL,
           [Created] [datetime] NOT NULL,
           [EventId] [uniqueidentifier] NOT NULL
       ) ON [PRIMARY]
   
       ALTER TABLE [dbo].[Outbox] ADD CONSTRAINT [DF_Outbox_Created] DEFAULT (getdate()) FOR [Created]
   END";
        context.Database.ExecuteSqlRaw(sql);
    }

}