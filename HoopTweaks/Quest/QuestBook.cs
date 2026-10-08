using System;
using System.Collections.Generic;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace HoopTweaks.Quest;

// Handle saving and loading quests using this tutorial
// https://wiki.vintagestory.at/Modding:SaveGame_ModData
public class QuestBook(ICoreServerAPI api)
{
    private Dictionary<Guid, Quest> QuestDictionary
    {
        set {
            if (value is null){
                Api.Logger.Error("cannot set the Quest dictionary to a nullable value");
            }
            else if (value.Count == 0){
                Api.Logger.Warning("Setting Quest Dictionary to an empty dictionary. This is may not be an error but can indeicate something is not loading properly");
            }
            QuestDictionary = value ?? [];
        }
        get
        {
            if (QuestDictionary is null)
                RefreshQuestDictionary();

            return QuestDictionary;
        }
    }
    private ICoreServerAPI Api { init; get; } = api;

    /// <summary>
    /// Saves the current quest to the save and also runs an async quest refresh
    /// </summary>
    /// <param name="quest"></param>
    public void SaveQuest(Quest quest)
    {
        var questByteArray = quest.ToByteArray();
        Api.Logger.Debug($"Saving Quest:{quest} questByte:{questByteArray}");
    }

    /// <summary>
    /// Queries the currently open quest dictionary for the given quest guid id
    /// </summary>
    public Quest? GetQuest(Guid questId)
    {
        Api.Logger.Debug($"Loading Quest Id:{questId}");
        if (QuestDictionary is null)
        {
            Api.Logger.Warning(
                $"Dictionary was null when quest lookup for {questId} occurred, refreshing"
            );
            return null;
        }

        if (!QuestDictionary.ContainsKey(questId))
        {
            Api.Logger.Error(
                $"Currently loaded Quest book does not have the quest Id listed:{questId}"
            );
        }
        return QuestDictionary.Get(questId);
    }

    /// <summary>
    /// Returns the total list of cached quests currently loaded
    /// </summary>
    /// <returns></returns>
    public int GetTotalQuestCount()
    {
        return QuestDictionary.Count;
    }

    /// <summary>
    /// Re-queries the save data for the currently registered quest
    /// list and refreshes the internal data with the latest
    /// </summary>
    /// <returns>true if successful and false if failure occurred</returns>
    public bool RefreshQuestDictionary()
    {
        QuestDictionary ??= [];
        Console.Write("thing");
        try
        {
            byte[] data = Api.WorldManager.SaveGame.GetData("questbook");
        }
        catch (Exception e)
        {
            Api.Logger.Error(
                $"Exception occurred trying to refresh the quest dictionary:{e.Message} {e.StackTrace}"
            );
        }
        return true;
    }

    public bool SaveQuestDictionary()
    {
        throw new NotImplementedException();
    }

    public bool DeleteQuestFromSave()
    {
        throw new NotImplementedException();
    }
}
