using Microsoft.Data.Sqlite;
using Guid = System.Guid;


namespace HoopTweaks.Quest;

public enum QuestStatus {
    Active = 1,
    Accepted = 2,
    Unaccepted = 3,
    Complete = 4
}

public class Quest(string title, string description, QuestStatus questStatus = QuestStatus.Unaccepted)
{
    public Guid Id { init; get; } = Guid.NewGuid();
    public string Title { init; get; } = title;
    public string Description { init; get; } = description;
    public QuestStatus Status { set; get; } = questStatus;

    public Quest(SqliteDataReader reader){
    }

    public void Complete() {
        Status = QuestStatus.Complete;
        //TODO: need to eventually reward the player
    }
    public bool IsComplete() => Status == QuestStatus.Complete;
    public override string ToString()
    {
        var isCompleteString = IsComplete() ? "[complete]" : "[incomplete]";
        return $"{Title} {isCompleteString} - {Description}";
    }
}
