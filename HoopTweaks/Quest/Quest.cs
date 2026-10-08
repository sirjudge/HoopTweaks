using System;
using Guid = System.Guid;


namespace HoopTweaks.Quest;

public enum QuestStatus {
    Active,
    Accepted,
    Unaccepted,
    Complete
}

public class Quest(string title, string description)
{
    public string Title { init; get; } = title;
    public string Description { init; get; } = description;
    public bool IsComplete { set; get; }

    public Guid Id { init; get; } = Guid.NewGuid();
    public void Complete() {
        IsComplete = true;
    }
    public override string ToString()
    {
        var isCompleteString = IsComplete ? "[complete]" : "[incomplete]";
        return $"{Title} {isCompleteString} - {Description}";
    }

    public byte[] ToByteArray(){
        throw new NotImplementedException();
    }
}
