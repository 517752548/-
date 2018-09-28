﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{

    public GameObject item;
    public Transform content;
    public GameObject SelectedGameObject;
    private CharacterListItem currentSelectedItem;
    public AttributeShow AttributeShowScript;
    public SkillListUI skillListUi;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreatItem());
    }

    IEnumerator CreatItem()
    {
        yield return null;
        GameObject items = null;
        foreach (var key in PlayerItem.characterDataMap.Keys)
        {
            yield return items = Instantiate(item);
            items.GetComponent<CharacterListItem>().Init(this, PlayerItem.characterDataMap[key]);
            items.transform.SetParent(content, false);
        }

    }

    public void SelectedItem(CharacterListItem currentSelectedItem, PlayerItem selectedItem)
    {
        this.currentSelectedItem = currentSelectedItem;
        AttributeShowScript.SetupInfo(selectedItem.Attributes);
        skillListUi.SetData(selectedItem.CharacterData.GetCustomSkills());
    }
}
