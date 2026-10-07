using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace HoopTweaks.Quest;

public class QuestClientCommands : IHoopClientCommand
{
    public override void RegisterClientCommands(ICoreClientAPI api)
    {
        api.ChatCommands.Create("listActiveQuests")
            .WithDescription("lists all currently active quests")
            .WithAlias("laq")
            .RequiresPrivilege(Privilege.chat)
            .RequiresPlayer()
            .HandleWith(
                (args) =>
                {
                    var testList = GererateTestQuests();
                    var outputStringBuilder = new StringBuilder();
                    foreach (var entry in testList)
                    {
                        outputStringBuilder.Append($"Title:{entry.Key} {entry.Value}");
                    }
                    api.Logger.Debug($"active quests: {testList}");
                    return TextCommandResult.Success();
                }
            );
    }

    private static ListDictionary<string, Quest> GererateTestQuests()
    {
        var quest1 = new Quest("quest1", "quest 1 desc");
        var quest2 = new Quest("quest2", "quest 2 desc");
        var quest3 = new Quest("quest3", "quest 3 desc");

        return new ListDictionary<string, Quest>
        {
            { quest1.Title, quest1 },
            { quest2.Title, quest2 },
            { quest3.Title, quest3 },
        };
    }
}
