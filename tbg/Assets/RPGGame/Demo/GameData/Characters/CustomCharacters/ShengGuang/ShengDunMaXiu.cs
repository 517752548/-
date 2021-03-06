﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShengDunMaXiu
{

}

public class TieBiTongQiang : CustomSkill
{
    public TieBiTongQiang()
    {
        skillName = "铁壁铜墙";
        spendPower = 18;
        des = "";
    }

    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        CustomBuff buff = SkillUtils.MakeCustomBuff("TieBiTongQiangBuff");
        List<BaseCharacterEntity> enemys = GetRandomEnemy(9);
        for (int i = 0; i < enemys.Count; i++)
        {
            (enemys[i] as CharacterEntity).ApplyCustomBuff(buff);
        }
        yield return MoveToSelfPos();
    }
}

public class TieBiTongQiangBuff : CustomBuff
{
    public TieBiTongQiangBuff()
    {
        buffText = "C";
        turns = 2;
        guid = "TieBiTongQiangBuff";
    }

    public override void SetSelf(CharacterEntity selfOnly)
    {
        base.SetSelf(selfOnly);
        MustCharacterEntity = selfOnly.ActionTarget;
    }

    public override bool CanUseSkill()
    {
        return false;
    }
}



public class MuBiaoJiZhong : CustomSkill
{
    public MuBiaoJiZhong()
    {
        spendPower = 10;
        des = "";
        skillName = "";
    }
    public override IEnumerator DoSkillLogic()
    {
        yield return MoveToTarget();
        CustomBuff buff = SkillUtils.MakeCustomBuff("MuBiaoJiZhongBuff");

        selfOnly.ActionTarget.ApplyCustomBuff(buff);

        yield return MoveToSelfPos();
    }
}
public class MuBiaoJiZhongBuff : CustomBuff
{
    public MuBiaoJiZhongBuff()
    {
        buffText = "C";
        turns = 2;
        guid = "MuBiaoJiZhongBuff";
    }

    public override void SetSelf(CharacterEntity selfOnly)
    {
        base.SetSelf(selfOnly);
        MustCharacterEntity = selfOnly.ActionTarget;
    }

    public override bool CanUseSkill()
    {
        return true;
    }
}
