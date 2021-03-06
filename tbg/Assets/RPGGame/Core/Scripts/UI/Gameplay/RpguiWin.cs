﻿using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class RpguiWin : RpguiDataItem<FinishStageResult>
{
    public const string ANIM_KEY_BATTLE_RATING = "Rating";
    public Animator ratingAnimator;
    public RpguiPlayer RpguiPlayer;
    public Text textRewardPlayerExp;
    public Text textRewardCharacterExp;
    public RpguiItemList RpguiRewardItems;
    public RpguiCurrency RpguiRewardCurrency;
    public Button buttonRestart;
    public Button buttonGoToManageScene;
    public Button buttonGoToNextStage;
    public BaseStage NextStage
    {
        get
        {
            var unlockStages = BaseGamePlayManager.PlayingStage.unlockStages;
            if (unlockStages != null && unlockStages.Length > 0)
                return unlockStages[0];
            return null;
        }
    }

    private BaseGamePlayManager manager;
    public BaseGamePlayManager Manager
    {
        get
        {
            if (manager == null)
                manager = FindObjectOfType<BaseGamePlayManager>();
            return manager;
        }
    }

    public override void Show()
    {
        base.Show();
        buttonRestart.onClick.RemoveListener(OnClickRestart);
        buttonRestart.onClick.AddListener(OnClickRestart);
        buttonGoToManageScene.onClick.RemoveListener(OnClickGoToManageScene);
        buttonGoToManageScene.onClick.AddListener(OnClickGoToManageScene);
        buttonGoToNextStage.onClick.RemoveListener(OnClickGoToNextStage);
        buttonGoToNextStage.onClick.AddListener(OnClickGoToNextStage);
        buttonGoToNextStage.interactable = NextStage != null;

        if (ratingAnimator != null)
            ratingAnimator.SetInteger(ANIM_KEY_BATTLE_RATING, data.rating);
    }

    public override void Clear()
    {
        if (RpguiPlayer != null)
            RpguiPlayer.Clear();

        if (textRewardPlayerExp != null)
            textRewardPlayerExp.text = "0";

        if (textRewardCharacterExp != null)
            textRewardCharacterExp.text = "0";

        if (RpguiRewardItems != null)
            RpguiRewardItems.ClearListItems();

        if (RpguiRewardCurrency != null)
        {
            var currencyData = IPlayerCurrency.SoftCurrency.Clone().SetAmount(0, 0);
            RpguiRewardCurrency.SetData(currencyData);
        }
    }

    public override bool IsEmpty()
    {
        return data == null;
    }

    public override void UpdateData()
    {
        //if (RpguiPlayer != null)
        //    RpguiPlayer.SetData(data);

        if (textRewardPlayerExp != null)
            textRewardPlayerExp.text = data.rewardPlayerExp.ToString("N0");

        if (textRewardCharacterExp != null)
            textRewardCharacterExp.text = data.rewardCharacterExp.ToString("N0");

        if (RpguiRewardItems != null)
        {
            RpguiRewardItems.selectable = false;
            RpguiRewardItems.multipleSelection = false;
            //RpguiRewardItems.SetListItems(data.rewardItems);
        }

        if (RpguiRewardCurrency != null)
        {
            var currencyData = IPlayerCurrency.SoftCurrency.Clone().SetAmount(data.rewardSoftCurrency, 0);
            RpguiRewardCurrency.SetData(currencyData);
        }
    }

    public void OnClickRestart()
    {
        Manager.Restart();
    }

    public void OnClickGoToManageScene()
    {
        RPGSceneManager.LoadScene(RPGSceneManager.ManagerScene);
    }

    public void OnClickGoToNextStage()
    {
        var nextStage = NextStage;
        if (nextStage != null)
            BaseGamePlayManager.StartStage(nextStage);
    }
}
