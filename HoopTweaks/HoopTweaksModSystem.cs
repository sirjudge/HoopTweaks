using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;

namespace HoopTweaks;

public class HoopTweaksModSystem : ModSystem
{
    private bool _disposing;
    private HoopTweaksClient? _client;
    private HoopTweaksServer? _server;

    // Called on server and client
    // Useful for registering block/entity classes on both sides
    public override void Start(ICoreAPI api)
    {
        Mod.Logger.Notification("Hello from template mod: " + api.Side);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("hooptweaks:hello"));
        _server = new HoopTweaksServer(this,api);
        _server.RegisterServerCommands();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        _client = new HoopTweaksClient(api);
        _client.RegisterClientCommands();
    }

    public override void Dispose() {
        if (_disposing){
            return;
        }
        _disposing = true;

        _client?.Dispose();
        _client = null;

        _server?.Dispose();
        _server = null;

        _disposing = false;
    }
}
