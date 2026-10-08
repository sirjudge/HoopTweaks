using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace HoopTweaks;

public class HoopTweaksServer : IHoopServerCommand
{
    private bool _disposing;
    private readonly HoopTweaksModSystem _mod;
    public ICoreServerAPI Sapi { get; }

    public string ModId => _mod.Mod.Info.ModID;

    public string ModVersion => _mod.Mod.Info.Version;

    public HoopTweaksServer(HoopTweaksModSystem mod, ICoreServerAPI api)
    {
        Sapi = api;
        _mod = mod;
        Sapi.Logger.Debug("initialized hoops tweak server");
    }

    public override void RegisterServerCommands()
    {
        Sapi.Logger.Debug("Registering server command");
        Sapi.ChatCommands
            .Create("testServer")
            .WithDescription("test server command")
            .WithAlias("ts")
            .HandleWith((args) => {
                int argCount = args.ArgCount;
                Sapi.Logger.Debug($"Arg Count: {argCount}");
                Sapi.Logger.Debug($"Args: {args}");
                return TextCommandResult.Success();
            });
    }

    public override void Dispose()
    {
        if (_disposing)
        {
            return;
        }

        _disposing = true;
        Sapi.Logger.Debug("Disposed Hoop Tweaks server");
        _disposing = false;
    }
}
