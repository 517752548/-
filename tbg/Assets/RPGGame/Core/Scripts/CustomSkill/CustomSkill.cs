﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum SkillType
{
    active,
    passive
}
public class CustomSkill
{

    #region 面板中显示的
    public string skillName = "技能名称";
    public string des = "这里是自定义技能的描述!";
    //冷却 技能CD
    public int CoolDownTurns = 10;
    #endregion
    protected List<BaseCharacterEntity> selfs, enemys;
    protected CharacterEntity selfOnly;
    public SkillUsageScope usageScope = SkillUsageScope.Enemy;


    public int id;

    //主动技能，被动技能,默认主动
    public SkillType skilltype = SkillType.active;
    //被动技能的属性加成
    public CalculationAttributes SelfAttributes = new CalculationAttributes();

    #region 一堆方法
    public virtual void Init()
    {
        //初始可用
        TurnsCount = CoolDownTurns;
    }
    /// <summary>
    /// 部分增加属性的被动技能使用
    /// </summary>
    public virtual void BattleStart()
    {

    }


    public virtual void DoSkill()
    {
    }

    public virtual void BeforeFight()
    {

    }

    public virtual void Fight()
    {
    }

    public virtual void Afterfight()
    {
    }
    public virtual void ReceiveDamage()
    {
    }
    public virtual void Beibaoji()
    {
    }
    public virtual void Beigedang()
    {
    }
    public virtual void Beimiss()
    {
    }
    public virtual void Gobaoji()
    {
    }
    public virtual void Gogedang()
    {
    }
    public virtual void Gomiss()
    {
    }

    public virtual void Trigger(TriggerType type)
    {
        switch (type)
        {
            case TriggerType.beforeFight:
                BeforeFight();
                break;
            case TriggerType.fight:
                Fight();
                break;
            case TriggerType.afterfight:
                Afterfight();
                break;
            case TriggerType.receiveDamage:
                ReceiveDamage();
                break;
            case TriggerType.beibaoji:
                Beibaoji();
                break;
            case TriggerType.beigedang:
                Beigedang();
                break;
            case TriggerType.beimiss:
                Beimiss();
                break;
            case TriggerType.gobaoji:
                Gobaoji();
                break;
            case TriggerType.gogedang:
                Gogedang();
                break;
            case TriggerType.gomiss:
                Gomiss();
                break;
            default:
                break;
        }


    }

    #endregion



    public int TurnsCount;

    public bool IsReady()
    {
        return TurnsCount >= CoolDownTurns;
    }
    public void OnUseSkill()
    {
        TurnsCount = 0;
    }

    /// <summary>
    /// 获取还有几回合冷却
    /// </summary>
    /// <returns></returns>
    public string GetCoolDownDuration()
    {
        return CoolDownTurns - TurnsCount > 0 ? (CoolDownTurns - TurnsCount) + "回合" : "可用";
    }

    /// <summary>
    /// 获取回合冷却百分比
    /// </summary>
    /// <returns></returns>
    public float GetCDFloat()
    {
        return (float)TurnsCount / CoolDownTurns;
    }


    public void SetNewEntitys(CharacterEntity selfOnly, List<BaseCharacterEntity> selfs, List<BaseCharacterEntity> enemys)
    {
        this.selfOnly = selfOnly;
        this.selfs = selfs;
        this.enemys = enemys;
    }

    //增加回合
    public void IncreaseTurnsCount()
    {
        TurnsCount++;
    }


    public virtual IEnumerator ApplyBuffLogic()
    {
        yield return null;
    }
    public virtual IEnumerator DoSkillLogic()
    {
        yield return null;
    }

    protected CharacterEntity GetSelf()
    {
        return GamePlayManager.Singleton.ActiveCharacter;
    }

    protected IEnumerator MoveToTarget()
    {
        yield return selfOnly.MoveTo(selfOnly.Manager.MapCenterPosition, selfOnly.Manager.doActionMoveSpeed);
    }

    protected IEnumerator MoveToSelfPos()
    {
        yield return selfOnly.MoveTo(selfOnly.Container.position, selfOnly.Manager.actionDoneMoveSpeed);
    }
    public enum SkillUsageScope
    {
        Self,
        Ally,
        Enemy,
        All,
    }

    public enum AttackScope
    {
        SelectedTarget,
        SelectedAndOneRandomTargets,
        SelectedAndTwoRandomTargets,
        SelectedAndThreeRandomTargets,
        OneRandomEnemy,
        TwoRandomEnemies,
        ThreeRandomEnemies,
        FourRandomEnemies,
        AllEnemies,
    }

    public enum BuffScope
    {
        Self,
        SelectedTarget,
        SelectedAndOneRandomTargets,
        SelectedAndTwoRandomTargets,
        SelectedAndThreeRandomTargets,
        OneRandomAlly,
        TwoRandomAllies,
        ThreeRandomAllies,
        FourRandomAllies,
        AllAllies,
        OneRandomEnemy,
        TwoRandomEnemies,
        ThreeRandomEnemies,
        FourRandomEnemies,
        AllEnemies,
        All,
    }

    public enum TriggerType
    {
        beforeFight,
        fight,
        afterfight,
        receiveDamage,
        beibaoji,
        beigedang,
        beimiss,
        gobaoji,
        gogedang,
        gomiss,

    }

}

