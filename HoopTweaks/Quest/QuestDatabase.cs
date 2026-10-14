using System.Collections.Generic;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;

namespace HoopTweaks.Quest;

public class QuestDatabase : HoopDatabase
{

    public QuestDatabase(ILogger logger)
        : base(logger)
    {
        //TODO: Check if db quest table exists table
    }

    public QueryEvent InitializeTable()
    {
        const string sqliteQuery = """
            create table quests(
                TEXT id,
                INT status,
                TEXT title,
                TEXT description
            )
            """;

        return new QueryEvent(true, "initTable", sqliteQuery);
    }

    public QueryEvent SaveQuestToDatabase(Quest quest){
        const string insertQuery = 
            """
            insert into quests(id, status, title, description)
            values ($id, $status,$title,$description)
            """;
        var command = Connection.CreateCommand();
        command.CommandText = insertQuery;
        command.Parameters.AddWithValue("$id", quest.Id.ToString());
        command.Parameters.AddWithValue("$status", quest.Status);
        command.Parameters.AddWithValue("$title", quest.Title);
        command.Parameters.AddWithValue("$description", quest.Description);

        int rowsInserted = command.ExecuteNonQuery();
        if (rowsInserted != 1){
            Logger.Audit($"[ERROR] tried to insert quest and expected 1 row change but got {rowsInserted} instead");
            return new QueryEvent(false, "saveQuest", insertQuery);
        }

        return new QueryEvent(true, "saveQuest", insertQuery);
    }

    public bool QuestTableExists(){
        var command = Connection.CreateCommand();
        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='quests';";
        return command.ExecuteReader().HasRows;
    }

    public Dictionary<Guid, Quest>  GetAllQuests(){
        const string selectQuery =
            """
            select id, status, title, description
            from quests
            """;
        var command = Connection.CreateCommand();
        command.CommandText = selectQuery;
        var reader = command.ExecuteReader();
        if (!reader.HasRows){
            Logger.Audit("No quest database rows returned");
            return [];
        }

        while (reader.Read()){
            var quest = new Quest(reader);
        }
    }
}
