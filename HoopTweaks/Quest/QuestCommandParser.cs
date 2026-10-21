using System;
using Vintagestory.API.Common;

namespace HoopTweaks.Quest;

public enum QuestAction
{
    List,
    Add,
    Delete,
}

public class QuestParser : ArgumentParserBase
{
    public QuestAction QuestAction { get; set; }
    public Quest? Quest { get; set; }
    private ILogger Logger { get; set; }

    public QuestParser(string argName, bool isMandatoryArg, ILogger logger)
        : base(argName, isMandatoryArg)
    {
        Quest = null;
        Logger = logger;
    }

    public override object GetValue()
        => new {
            QuestAction,
            Quest
        };

    public override void SetValue(object data)
    {
        throw new NotImplementedException();
    }

    public override EnumParseResult TryProcess(
        TextCommandCallingArgs args,
        Action<AsyncParseResults>? onReady = null
    )
    {
        if (args.ArgCount == 0)
            return EnumParseResult.Bad;

        for (int i = 0; i < args.ArgCount; i++){
            var arg = args[i];
            Logger.Debug($"arg in quest parser:{arg}");
        }

        return EnumParseResult.Good;
    }
}
