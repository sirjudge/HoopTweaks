using System;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace HoopTweaks.Quest;

public class HoopsQuestSystem : IHoopServerSystem
{
    private ICoreServerAPI ServerApi { set; get; }
    private QuestBook? QuestBook { set; get; }

    public override void StartServerSide(ICoreServerAPI api)
    {
        ServerApi = api;
        //BUG: Something with these three lines is borking things 
        // InitalizeQuestbook();
        // QuestBook.RefreshQuestDictionary();
        // RegisterServerCommands();
    }

    internal void InitalizeQuestbook(){
        if (QuestBook is not null) return;

        QuestBook = new QuestBook(ServerApi);
    }

    public override void RegisterServerCommands()
    {
        ServerApi.Logger.Debug("Registering server commands");
        ServerApi.ChatCommands.Create("hoopQuest")
            .WithDescription("adds new quest to quest list")
            .RequiresPrivilege(Privilege.chat)
            .WithArgs(ServerApi.ChatCommands.Parsers.Word("cmd", ["add", "remove","list"]))
            .HandleWith(
                (args)=>
                {
                    switch (args[0] as string)
                    {
                        case "add":
                            ServerApi.Logger.Debug("Adding new quest to list");
                            break;
                        case "remove":
                            ServerApi.Logger.Debug("removing mod from list list");
                            break;
                        case "list":
                            ServerApi.Logger.Debug("list of all registered quests");
                            break;
                        default:
                            return TextCommandResult.Error("/quest [add|remove|list]");
                    }
                    return TextCommandResult.Success();
                }
            );
    }

    public Quest ParseQuestFromArgs(TextCommandCallingArgs args){
        if (args.ArgCount <= 0){
            ServerApi.Logger.Error("Could not parse quest arugments, argument count is 0, returning fake test instead");
            return GenerateTestQuest();
        }

        return GenerateTestQuest();
    }
    public Quest GenerateTestQuest() {
        string title = $"title{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Hour}";
        string description = $"description{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Hour}";
        return new Quest(title, description);
    }

    public override void Dispose()
    {
        if (ServerApi is not null){
            ServerApi.Logger.Debug("Disposed Quest Server Command");
            ServerApi = null;
        }

    }
}
