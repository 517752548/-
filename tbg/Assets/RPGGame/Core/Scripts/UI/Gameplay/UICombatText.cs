﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFollowWorldObject))]
[RequireComponent(typeof(Text))]
public class UICombatText : MonoBehaviour
{
    public float lifeTime;

    private UIFollowWorldObject tempObjectFollower;
    public UIFollowWorldObject TempObjectFollower
    {
        get
        {
            if (tempObjectFollower == null)
                tempObjectFollower = GetComponent<UIFollowWorldObject>();
            return tempObjectFollower;
        }
    }

    private Text tempText;
    public Text TempText
    {
        get
        {
            if (tempText == null)
            {
                tempText = GetComponent<Text>();
                tempText.text = "";
            }
            return tempText;
        }
    }

    private string customstring;
    public string CustomStr
    {
        get { return customstring; }
        set
        {
            customstring = value;
            TempText.text += customstring;
        }
    }

    private int amount;
    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;
            TempText.text += amount.ToString("N0");
        }
    }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}
