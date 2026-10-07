using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace HoopTweaks.Quest;

public class QuestServerCommand : IHoopServerCommand
{
    public override void RegisterServerCommands(ICoreServerAPI api)
    {
        api.ChatCommands.Create("addQuest")
            .WithDescription("adds new quest to quest list")
            .RequiresPrivilege(Privilege.chat)
            .HandleWith(
                (args) =>
                {
                    api.Logger.Debug("added new quest to list (pretend tho)");
                    return TextCommandResult.Success();
                }
            );
    }
}
