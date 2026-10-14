using System;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Common;
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
        // TranslationService? translationService = new("en", Logger);
        TranslationService? translationService = new TranslationService(
            "en",
            Logger,
            new AssetManager("hooptweaks", EnumAppSide.Universal)
        );
        Logger?.Debug("Successfully registered client commands");

        Api?.ChatCommands.Create("whoUp")
            .WithDescription("lists out who is currently online")
            .RequiresPlayer()
            .HandleWith(
                (args) =>
                {
                    var onlinePlayersString = string.Join(",", Api.World.AllOnlinePlayers);
                    var outputString = $"The following people are online:{onlinePlayersString}";
                    Api.ShowChatMessage(outputString);
                    Logger.Debug(outputString);
                    return TextCommandResult.Success();
                }
            );

        /*
         14.10.2026 12:17:49 [Error] Ran into exception translating affirmation with key:affirmation-6 and exception:System.ArgumentNullException: Value cannot be null. (Parameter 'source')
         at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument argument)
         at System.Linq.Enumerable.SelectMany[TSource,TResult](IEnumerable`1 source, Func`2 selector)
         at Vintagestory.API.Config.TranslationService.Load(Boolean lazyLoad) in C:\Users\office-desktop2\Documents\game\VintagestoryApi\Localization\TranslationService.cs:line 80
         at Vintagestory.API.Config.TranslationService.Get(String key, Object[] args) in C:\Users\office-desktop2\Documents\game\VintagestoryApi\Localization\TranslationService.cs:line 463
         at HoopTweaks.HoopTweaksClient.<>c__DisplayClass13_0.<RegisterClientCommands>b__1(TextCommandCallingArgs args) in /home/nico/solutions/vintageStory/HoopTweaks/HoopTweaks/HoopTweaksClient.cs:line 72
         */

        Api?.ChatCommands.Create("hoop")
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
                            //https://wiki.vintagery.at/Modding:Simple_Particles
                            string affirmationkey = $"affirmation-{new Random().Next(0, 10)}";
                            try
                            {
                                var affirmationText = Lang.Get($"hooptweaks:{affirmationkey}");
                                Api.TriggerIngameDiscovery(player, "error text", affirmationText);
                            }
                            catch (Exception ex)
                            {
                                Logger?.Error(
                                    $"Ran into exception translating affirmation with key:{affirmationkey} and exception:{ex.Message} and stacktrace: {ex.StackTrace}"
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
