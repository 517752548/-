﻿using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class RpguiPlayer : RpguiDataItem<ICharacter>
{
    public Text textProfileName;
    public UILevel uiLevel;
    public override void UpdateData()
    {
        SetupInfo(data);
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(ICharacter data)
    {
        if (data == null)
            data = new ICharacter();

        //if (textProfileName != null)
            //textProfileName.text = data.profileName;

        // Stats
        if (uiLevel != null)
        {
            //uiLevel.level = data.Level;
            //uiLevel.maxLevel = data.MaxLevel;
            //uiLevel.collectExp = data.CollectExp;
            //uiLevel.nextExp = data.NextExp;
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.guid);
    }
}
