using Guid = System.Guid;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace HoopTweaks;

public class HoopTweaksClient : IHoopClientCommand
{
    private readonly ILogger _logger;
    private readonly ICoreClientAPI _api;
    public System.Collections.Generic.Dictionary<Guid, Quest.Quest>? QuestCache { set; get; }
    public HoopTweaksClient(ICoreClientAPI api)
    {

        _api = api;
        _logger = api.Logger;
        _logger.Debug("Initialized client logger for hoop tweaks client");
    }

    public override void RegisterClientCommands()
    {
        _logger.Debug("Successfully registered client commands");
    }

    public override void Dispose()
    {
        QuestCache = null;
        _logger.Debug("Successfully disposed hoops tweaks client");
    }
}
