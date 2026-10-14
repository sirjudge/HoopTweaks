using System;
using System.Collections.Generic;
using System.Text;
using HoopTweaks.Infrastructure;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

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
            if (value is null){
                value = [];
                Logger.Audit("Attempting to set quest dictionary to null");
            }

            if (value.Count == 0)
            {
                Logger.Warning(
                    "Setting Quest Dictionary to an empty dictionary. This is may not be an error but can indeicate something is not loading properly"
                );
            }

            _questDictionary = value;
            Logger.Audit("Added new Quest to quest book:{_questDictionary}");
        }
    }

    public void AddQuest(Quest quest){
        if (QuestDictionary.ContainsKey(quest.Id)){
            Logger.Warning($"Quest already added to quest book, bailing early. Quest:{quest}");
            return;
        }
        _questDictionary.Add(quest.Id, quest);
        Save();
        Logger.Audit($"Quest saved:{quest}");

    }

    private void InializeQuestDb(){

    }
    private void Save(){
        Logger.Audit($"Questbook saved:{this}");
        using (var databse = new QuestDatabase(Logger)){
            //TODO: Save quest here
        }
    }

    /// <summary>
    /// Queries the currently open quest dictionary for the given quest guid id
    /// </summary>
    public Quest? GetQuest(Guid questId)
    {
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
            return null;
        }

        var returnQuest = QuestDictionary[questId];
        Logger.Audit($"queried Quest Dictionary:{returnQuest}");
        return  returnQuest;
    }

    /// <summary>
    /// Re-queries the save data for the currently registered quest
    /// list and refreshes the internal data with the latest
    /// </summary>
    /// <returns>true if successful and false if failure occurred</returns>
    public bool RefreshQuestDictionary()
    {
        try
        {
            using (var databse = new QuestDatabase(Logger)){
                //TODO: Read quest here 
            }
        }
        catch (Exception e)
        {
            Logger.Error(
                $"Exception occurred trying to refresh the quest dictionary:{e.Message} {e.StackTrace}"
            );
        }
        return true;
    }

    public override string ToString()
    {
        StringBuilder outputStringBuilder = new(
            $"Quest book has {QuestDictionary.Count} Keys. {Environment.NewLine}"
        );

        foreach (KeyValuePair<Guid, Quest> questPair in QuestDictionary)
        {
            var quest = questPair.Value;
            outputStringBuilder.Append($"QuestId:{questPair.Key} Quest title: {quest.Title}");
        }

        return outputStringBuilder.ToString();
    }

}
