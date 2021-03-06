﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill001 : CustomSkill
{
    public Skill001()
    {
        skillName = "排山倒海";
        des = "攻击敌人,同时增加自己气血上限";
    }

    public override IEnumerator ApplyBuffLogic()
    {
        CustomBuff buff001 = SkillUtils.MakeCustomBuff("Buff001");
        buff001.SetGiver(selfOnly);
        GetSelf().ApplyCustomBuff(buff001);
        Debug.Log("执行了 添加自定义buff 001");
        yield return null;
    }

    public override IEnumerator DoSkillLogic()
    {
        selfOnly.Manager.SpawnCombatCustomText(selfOnly, skillName);
        yield return MoveToTarget();
        var attackDamage = new SkillAttackDamage();
        selfOnly.Attack(selfOnly.ActionTarget, attackDamage.GetPAtkDamageRate(), attackDamage.GetMAtkDamageRate(), attackDamage.hitCount, (int)attackDamage.GetFixDamage());
        yield return ApplyBuffLogic();
        yield return MoveToSelfPos();
        yield return null;
    }
}
