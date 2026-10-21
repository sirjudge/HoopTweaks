#nullable disable
using System;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace HoopTweaks.Quest;

public class HoopsQuestServerSystem : IHoopServerSystem
{
    private ICoreServerAPI ServerApi { set; get; }
    private QuestBook QuestBook { set; get; }
    private bool _disposing;

    public override void StartServerSide(ICoreServerAPI api)
    {
        ServerApi = api;
        try
        {
            InitalizeQuestbook();
            ServerApi.Logger.Audit("initialized QuestBook");
            ServerApi.Logger.Audit(QuestBook.ToString());
            RegisterServerCommands();
        }
        catch (Exception ex)
        {
            ServerApi.Logger.Error(
                $"Exception occurred during quest initialization:{ex.Message} with stack trace:{ex.StackTrace}"
            );
        }
    }

    internal void InitalizeQuestbook()
    {
        if (QuestBook is not null)
            return;

        try
        {
            QuestBook = new QuestBook(ServerApi);
            QuestBook.RefreshQuestDictionary();
        }
        catch (Exception ex)
        {
            ServerApi.Logger.Error(
                $"Exception ocurred initializing quest book:{ex.Message} with stack trace:{ex.StackTrace}"
            );
        }
    }

    public override void RegisterServerCommands()
    {
        ServerApi.Logger.Debug("Registering server commands");
        var questCommandParser = new QuestParser("hoopquest", true, ServerApi.Logger);

        ServerApi
            .ChatCommands.Create("hoopquestList")
            .WithAlias("hql")
            .WithDescription("list all current quests")
            .WithExamples("/hoopquest list")
            .RequiresPrivilege(Privilege.chat)
            .HandleWith(
                (_) =>
                {
                    ServerApi.Logger.Audit(
                        $"list of all registered quests. Quest List:{QuestBook}"
                    );
                    return TextCommandResult.Success();
                }
            );

        ServerApi
            .ChatCommands.Create("hoopquestAdd")
            .WithAlias("hqa")
            .WithDescription("adds new quest to quest list")
            .WithExamples("/hoopquest add \"title\" \"super cool description\"")
            .RequiresPrivilege(Privilege.chat)
            .WithArgs([
                ServerApi.ChatCommands.Parsers.WordRange("title"),
                ServerApi.ChatCommands.Parsers.Word("description"),
            ])
            .HandleWith(
                (args) =>
                {
                    ServerApi.Logger.Debug(
                        $"Args arg count:{args.ArgCount} parser count:{args.Parsers.Count}"
                    );

                    if (args.ArgCount == 2)
                    {
                        ServerApi.Logger.Debug(
                            $"Expected 2 args but found {args.ArgCount} instead"
                        );
                    }
                    if (args.Parsers[0].IsMissing)
                    {
                        ServerApi.Logger.Debug("Title is missing");
                    }
                    if (args.Parsers[1].IsMissing)
                    {
                        ServerApi.Logger.Debug("description is missing");
                    }

                    bool failure = false;
                    if (args.Parsers[0].TryProcess(args) != EnumParseResult.Good)
                    {
                        ServerApi.Logger.Error("enum parse result bad for add quest parser 0");
                        failure = true;
                    }

                    if (args.Parsers[1].TryProcess(args) != EnumParseResult.Good)
                    {
                        ServerApi.Logger.Error("enum parse result bad for add quest parser 0");
                        failure = true;
                    }

                    if (failure)
                    {
                        return TextCommandResult.Error("failed to parse something in quest add");
                    }

                    //TODO: Move this quest to ve added elsewhere
                    var questToAdd = GenerateTestQuest();
                    QuestBook.AddQuest(questToAdd);
                    ServerApi.Logger.Audit($"Added new quest to list. Args: {questToAdd}");
                    return TextCommandResult.Success();
                }
            );
    }

    public Quest GenerateTestQuest()
    {
        string title = $"title{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Hour}";
        string description =
            $"description{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Hour}";
        return new Quest(title, description);
    }

    public override void Dispose()
    {
        if (_disposing)
            return;

        _disposing = true;
        QuestBook = null;
        ServerApi = null;
        _disposing = false;
    }
}
