﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEquipDialog : Dialog
{

    public UIAttributeShow AttributeShow;
	// Use this for initialization
	void Start () {
		
	}

    public override void Init()
    {
        //AttributeShow.SetupInfo(GameInstance.GameDatabase.equipments[shopItemData.equipId].GetTotalAttributes());
    }
}
