﻿using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;

public class BaseGamePlayFormation : MonoBehaviour
{
    public Transform[] containers;
    public readonly Dictionary<int, BaseCharacterEntity> Characters = new Dictionary<int, BaseCharacterEntity>();


    public virtual void SetFormationCharacters()
    {
        var formationName = IPlayer.CurrentPlayer.selectedFormation;
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            PlayerFormation playerFormation = null;
            if (PlayerFormation.TryGetData(formationName, i, out playerFormation))
            {
                var characterGuid = playerFormation.characterGuid;
                IPlayerHasCharacters item = null;
                if (!string.IsNullOrEmpty(characterGuid) && IPlayerHasCharacters.DataMap.TryGetValue(characterGuid, out item))
                    SetCharacter(i, item);
            }
        }
    }

    public virtual void SetCharacters(IPlayerHasCharacters[] items)
    {
        ClearCharacters();
        for (var i = 0; i < containers.Length; ++i)
        {
            if (items.Length <= i)
                break;
            var item = items[i];
            SetCharacter(i, item);
        }
    }

    public virtual BaseCharacterEntity SetCharacter(int position, IPlayerHasCharacters item)
    {
        //if (position < 0 || position >= containers.Length || item == null || item.CharacterData == null)
            //return null;


        var container = containers[position];
        container.RemoveAllChildren();

        var character = Instantiate(GameInstance.Singleton.model);
        character.SetFormation(this, position);
        character.Item = item;
        Characters[position] = character;
        character.transform.rotation = Quaternion.Euler(0, 0, 0);
        character.transform.localRotation = Quaternion.Euler(0, 0, 0);
        (character as CharacterEntity).customSkillActionLogic.InitBattleCustomSkill();
        return character;
    }

    public virtual void ClearCharacters()
    {
        foreach (var container in containers)
        {
            container.RemoveAllChildren();
        }
        Characters.Clear();
    }
}
