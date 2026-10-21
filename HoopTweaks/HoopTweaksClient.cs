using System;
using System.Text;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
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
        RegisterClientCommands();
    }

    public override void RegisterClientCommands()
    {
        if (Logger is null) {
            throw new NullReferenceException("Expected Logger to not be null when registering client commands");
        }
        Api?.ChatCommands.Create("hoop")
            .WithDescription("commands used to interact with the hoop mod")
            .WithArgs(Api.ChatCommands.Parsers.Word("cmd", ["kerchow", "affirmation"]))
            .RequiresPlayer()
            .HandleWith(
                (args) =>
                {
                    var player = args.Caller.Player;
                    var byEntity = args.Caller.Entity;
                    switch (args[0] as string)
                    {
                        case "whoUp":
                            var playersOnline = Api.World.AllOnlinePlayers;
                            StringBuilder onlinePlayersStringBuilder = new ("The following people are online");
                            foreach (var onlinePlayer in playersOnline){
                                onlinePlayersStringBuilder.Append($"{onlinePlayer.PlayerUID}:{onlinePlayer.PlayerName}");
                            }

                            var onlinePlayersString = string.Join(",", Api.World.AllOnlinePlayers);

                            var outputString = onlinePlayersStringBuilder.ToString();
                            Api.ShowChatMessage(outputString);
                            Logger?.Audit($"Player {player.PlayerName} envoked whoup with command output:{outputString}");
                            break;
                        case "kerchow":
                            var kerchowLocation = new AssetLocation(
                                "hoopeffects",
                                "sounds/vroomeffect"
                            );
                            byEntity.World.PlaySoundAt(kerchowLocation, byEntity);
                            Api.TriggerIngameDiscovery(player, "not error text", "kerchow");
                            break;
                        case "affirmation":
                            string affirmationkey = $"affirmation-{new Random().Next(0, 16)}";
                            try
                            {
                                Random _random = new();
                                SimpleParticleProperties myParticles = new(
                                        20,
                                        40,
                                        ColorUtil.ColorFromRgba(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), 255),
                                        new Vec3d(), //minpos
                                        new Vec3d(), //maxpos
                                        new Vec3f(1, 5, 1), // minVelocity
                                        new Vec3f(-1, 10, -1), //maxVelocity
                                        lifeLength: 10,
                                        gravityEffect: (float)0.8,
                                        minSize: 2,
                                        maxSize: 5
                                    )
                                {
                                    MinPos = byEntity
                                        .Pos.XYZ.Add(0, byEntity.LocalEyePos.Y, 0)
                                        .Ahead(2f, byEntity.Pos.Pitch, byEntity.Pos.Yaw),
                                };
                                byEntity.World.SpawnParticles(myParticles);

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
