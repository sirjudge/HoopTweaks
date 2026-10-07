using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace HoopTweaks;
public class HoopTweaksServer
{
    private readonly HoopTweaksModSystem _mod;
    public ICoreServerAPI Sapi { get; }

    public string ModId => _mod.Mod.Info.ModID;

    public string ModVersion => _mod.Mod.Info.Version;

    public HoopTweaksServer(HoopTweaksModSystem mod, ICoreServerAPI api) {
        Sapi = api;
        _mod = mod;
        api.Logger.Debug("Loaded server");
    }

    public void RegisterServerCommands(ICoreServerAPI api){
        //TODO: Enter server commands here
    }
    public void Dispose(){
        //TODO: Dispose stuff here
    }
}


