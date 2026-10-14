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
        ServerApi
            .ChatCommands.Create("hoopQuest")
            .WithDescription("adds new quest to quest list")
            .RequiresPrivilege(Privilege.chat)
            .WithArgs(ServerApi.ChatCommands.Parsers.Word("cmd", ["add", "list"]))
            .HandleWith(
                (args) =>
                {
                    string argsString = string.Join(',', args);
                    switch (args[0] as string)
                    {
                        //TODO: Should eventually come back  and change this to better paramaterize 
                        //but fine to use generative test for now
                        case "add":
                            var questToAdd = GenerateTestQuest();
                            QuestBook.AddQuest(questToAdd);
                            ServerApi.Logger.Audit($"Added new quest to list. Args: {questToAdd}");
                            break;
                        case "list":
                            ServerApi.Logger.Audit($"list of all registered quests. Quest List:{QuestBook}");
                            break;
                        default:
                            return TextCommandResult.Error($"/quest [add|remove|list].Args:{argsString}");
                    }
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
