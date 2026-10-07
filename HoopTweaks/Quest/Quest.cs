using System;

namespace HoopTweaks.Quest;
public class Quest(string title, string description)
{
    public string Title { init; get; } = title;
    public string Description { init; get; } = description;
    public bool IsComplete { set; get; }

    public Guid Id { init; get; } = Guid.NewGuid();
    public void Complete() {
        IsComplete = true;
    }
}
