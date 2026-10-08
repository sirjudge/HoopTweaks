using System.Text;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace HoopTweaks.Quest;

public class QuestClientCommands : IHoopClientCommand
{
    private ICoreClientAPI Api { init; get; }

    public QuestClientCommands(ICoreClientAPI api)
    {
        Api = api;
    }

    public override void RegisterClientCommands()
    {
    }

    public override void Dispose()
    {
        Api.Logger.Debug("Disposed of quest client command");
    }
}
