using HoopTweaks.Quest;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace HoopTweaks;

public class HoopTweaksClient
{
    private readonly ICoreClientAPI _api;
    private readonly ILogger _logger;
    private readonly HoopTweaksModSystem _mod;
    public ListDictionary<string, Quest.Quest>? _questCache { set; get; }

    public HoopTweaksClient(HoopTweaksModSystem mod, ICoreClientAPI api)
    {
        _mod = mod;
        _api = api;
        _logger = mod.Mod.Logger;
        _logger.Debug("Initialized client logger for hoop tweaks client");
    }

    public void Dispose()
    {
        _questCache = null;
    }
}
