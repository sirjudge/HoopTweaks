using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace HoopTweaks;

public class HoopTweaksServer : IHoopSystem
{
    private bool _disposing;
    public ICoreServerAPI Sapi { set; get; }

    public override void StartServerSide(ICoreServerAPI api)
    {
        Sapi = api;
        RegisterServerCommands();
    }

    public void RegisterServerCommands()
    {
        Sapi.Logger.Debug("Registering server command");
        Sapi.ChatCommands.Create("testServer")
            .WithDescription("test server command")
            .WithAlias("ts")
            .HandleWith(
                (args) =>
                {
                    int argCount = args.ArgCount;
                    Sapi.Logger.Debug($"Arg Count: {argCount}");
                    Sapi.Logger.Debug($"Args: {args}");
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
        _disposing = false;
    }
}
