using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace HoopTweaks.Quest;

// Handle saving and loading quests using this tutorial
// https://wiki.vintagestory.at/Modding:SaveGame_ModData
public class QuestBook(ICoreServerAPI api)
{
    private ICoreServerAPI Api { init; get; } = api;
    private ILogger Logger { init; get; } = api.Logger;

    private Dictionary<Guid, Quest> _questDictionary = [];

    public Dictionary<Guid, Quest> QuestDictionary
    {
        get => _questDictionary;
        set
        {
            if (value.Count == 0)
            {
                Logger.Warning(
                    "Setting Quest Dictionary to an empty dictionary. This is may not be an error but can indeicate something is not loading properly"
                );
            }

            _questDictionary = value ?? [];
        }
    }

    /// <summary>
    /// Saves the current quest to the save and also runs an async quest refresh
    /// </summary>
    public void SaveQuest(Quest quest)
    {
        var questByteArray = quest.ToByteArray();
        Logger.Debug($"Saving Quest:{quest} questByte:{questByteArray}");
    }

    /// <summary>
    /// Queries the currently open quest dictionary for the given quest guid id
    /// </summary>
    public Quest? GetQuest(Guid questId)
    {
        Logger.Debug($"Loading Quest Id:{questId}");
        if (QuestDictionary is null)
        {
            Logger.Warning(
                $"Dictionary was null when quest lookup for {questId} occurred, refreshing"
            );
            return null;
        }

        if (!QuestDictionary.ContainsKey(questId))
        {
            Logger.Error(
                $"Currently loaded Quest book does not have the quest Id listed:{questId}"
            );
        }
        return QuestDictionary.Get(questId);
    }

    /// <summary>
    /// Re-queries the save data for the currently registered quest
    /// list and refreshes the internal data with the latest
    /// </summary>
    /// <returns>true if successful and false if failure occurred</returns>
    public bool RefreshQuestDictionary()
    {
        QuestDictionary ??= [];
        try
        {
            byte[] data = Api.WorldManager.SaveGame.GetData("questbook");
        }
        catch (Exception e)
        {
            Logger.Error(
                $"Exception occurred trying to refresh the quest dictionary:{e.Message} {e.StackTrace}"
            );
        }
        return true;
    }
}
