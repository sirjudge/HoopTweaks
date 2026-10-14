using System;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace HoopTweaks;

public class HoopTweaksServer : IHoopSystem
{
    private bool _disposing;
    public ICoreServerAPI? ServerApi { get; set; }

    public ILogger? Logger { get; set; }

    public override void StartServerSide(ICoreServerAPI api)
    {
        ServerApi = api;
        RegisterServerCommands();
    }

    public void RegisterServerCommands()
    {
        if (ServerApi is null)
        {
            throw new NullReferenceException(
                "Server Api should have already been initialized at this point"
            );
        }

        ServerApi
            .ChatCommands.Create("hoopDebug")
            .WithDescription("test server command")
            .WithAlias("hd")
            .RequiresPrivilege(Privilege.chat)
            .HandleWith(
                (args) =>
                {
                    ServerApi.Logger.Debug("HoopDebug check. Status: Healhty");
                    return TextCommandResult.Success();
                }
            );
    }

    public override void Dispose()
    {
        if (_disposing)
        {
            return;
        }

        _disposing = true;
        Logger = null;
        ServerApi = null;
        _disposing = false;
    }
}
