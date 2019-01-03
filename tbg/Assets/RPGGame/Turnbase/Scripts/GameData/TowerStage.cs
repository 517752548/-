﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerStage : BaseStage
{
    [Header("Battle")]
    public StageWave[] waves;
    public StageRandomFoe[] randomFoes;

    public StageRandomFoe RandomFoes()
    {
        var weight = new Dictionary<StageRandomFoe, int>();
        foreach (var randomFoe in randomFoes)
        {
            weight.Add(randomFoe, randomFoe.randomWeight);
        }
        return WeightedRandomizer.From(weight).TakeOne();
    }

    public override List<PlayerItem> GetCharacters()
    {
        var dict = new Dictionary<string, PlayerItem>();
        foreach (var randomFoe in randomFoes)
        {
            foreach (var foe in randomFoe.foes)
            {
                
                if (!string.IsNullOrEmpty(foe.characterId))
                {
                    var newEntry = PlayerItem.CreateActorItemWithLevel(DBManager.instance.GetConfigCharacters()[foe.characterId], foe.level,Const.StageType.Tower,false);
                    newEntry.GUID = DBManager.instance.GetConfigCharacters()[foe.characterId].guid + "_" + foe.level;
                    dict[DBManager.instance.GetConfigCharacters()[foe.characterId].guid + "_" + foe.level] = newEntry;
                }
            }
        }
        foreach (var wave in waves)
        {
            if (wave.useRandomFoes)
                continue;

            var foes = wave.foes;
            foreach (var foe in foes)
            {
                var item = DBManager.instance.GetConfigCharacters()[foe.characterId];
                if (item != null)
                {
                    var newEntry = PlayerItem.CreateActorItemWithLevel(item, foe.level,Const.StageType.Tower,false);
                    newEntry.GUID = DBManager.instance.GetConfigCharacters()[foe.characterId].guid + "_" + foe.level;
                    dict[DBManager.instance.GetConfigCharacters()[foe.characterId].guid + "_" + foe.level] = newEntry;
                }
            }
        }
        return new List<PlayerItem>(dict.Values);
    }
}