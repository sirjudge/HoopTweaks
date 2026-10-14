using System;
using HoopTweaks.Infrastructure;
using Microsoft.Data.Sqlite;
using Guid = System.Guid;


namespace HoopTweaks.Quest;

public enum QuestStatus {
    Active = 1,
    Accepted = 2,
    Unaccepted = 3,
    Complete = 4
}

//TODO: Need to fill these two out with protobuff and such
public class QuestMessage : HoopMessage {}
public class QuestRequest : HoopRequest {}


public class Quest
{
    public Guid Id { init; get; } = Guid.NewGuid();
    public string Title { init; get; }
    public string Description { init; get; }
    public QuestStatus QuestStatus { set; get; }

    public Quest(string title, string description, QuestStatus questStatus = QuestStatus.Unaccepted){
        Title = title;
        Description = description;
        QuestStatus = questStatus;
    }
    public QuestStatus ParseStatus(int statusInt)
        => statusInt switch
        {
            1 => QuestStatus.Active,
            2 => QuestStatus.Accepted,
            3 => QuestStatus.Unaccepted,
            5 => QuestStatus.Complete,
            _ => throw new DataMisalignedException($"Could not find questStatusInt as enum:{statusInt}"),
        };
    public Quest(SqliteDataReader reader){
        Id = new Guid(reader.GetString(reader.GetOrdinal("id")));
        Title = reader.GetString(reader.GetOrdinal("title"));
        Description = reader.GetString(reader.GetOrdinal("description"));

        int questStatusInt = reader.GetInt32(reader.GetOrdinal("status"));
        QuestStatus = ParseStatus(questStatusInt);
    }

    public void Complete() {
        QuestStatus = QuestStatus.Complete;
    }
    public bool IsComplete() => QuestStatus == QuestStatus.Complete;
    public override string ToString()
    {
        var isCompleteString = IsComplete() ? "[complete]" : "[incomplete]";
        return $"{Title} {isCompleteString} - {Description}";
    }
}
