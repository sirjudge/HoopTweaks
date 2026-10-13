using System;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Guid = System.Guid;

namespace HoopTweaks;

public class HoopTweaksClient : IHoopClientSystem
{
    private ILogger? Logger { get; set; }
    private ICoreClientAPI? Api { get; set; }
    public System.Collections.Generic.Dictionary<Guid, Quest.Quest>? QuestCache { set; get; }

    public override void StartClientSide(ICoreClientAPI api)
    {
        Api = api;
        Logger = api.Logger;
        Logger.Debug("Initialized client logger for hoop tweaks client");
        RegisterClientCommands();
    }

    public override void RegisterClientCommands()
    {
        TranslationService? translationService = new("en", Logger);
        Logger?.Debug("Successfully registered client commands");
        Api?.ChatCommands
            .Create("hoop")
            .WithDescription("commands used to interact with the hoop mod")
            .WithArgs(Api.ChatCommands.Parsers.Word("cmd", ["kerchow", "affirmation"]))
            .RequiresPlayer()
            .HandleWith(
                (args) =>
                {
                    var player = args.Caller.Player;
                    switch (args[0] as string)
                    {
                        case "kerchow":
                            var kerchowLocation = new AssetLocation(
                                "hoopeffects",
                                "sounds/vroomeffect"
                            );
                            var byEntity = args.Caller.Entity;
                            byEntity.World.PlaySoundAt(kerchowLocation, byEntity);
                            Api.TriggerIngameDiscovery(player, "not error text", "kerchow");
                            break;
                        case "affirmation":
                            //TODO: Need to add fun particles to this
                            //https://wiki.vintagestory.at/Modding:Simple_Particles
                            string affirmationkey = $"affirmation-{new Random().Next(0, 10)}";
                            try
                            {
                                string affirmation = translationService.Get(affirmationkey);
                                Api.TriggerIngameDiscovery(player, "error text", affirmation);
                            }
                            catch (Exception ex)
                            {
                                Logger?.Error(
                                    $"Ran into exception translating affirmation with key:{affirmationkey} and exception:{ex}"
                                );
                                Api.TriggerIngameDiscovery(
                                    player,
                                    "translation service failed",
                                    "love you dawg <3"
                                );
                            }
                            break;
                        default:
                            Api.Logger.Chat(".hoops [kerchow|affirmation|help]");
                            break;
                    }
                    return TextCommandResult.Success();
                }
            );
    }

    public override void Dispose()
    {
        QuestCache = null;
        Logger?.Debug("Successfully disposed hoops tweaks client");
    }
}
