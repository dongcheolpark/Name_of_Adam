using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum 낙인
{
    고양, 자애, 강림, // 소환 시
    가학, 흡수, 처형, 대죄, // 공격 후
}

public abstract class Passive
{
    private PassiveType type;
    public PassiveType PassiveType => type;

    public abstract PassiveType GetPassiveType();

    public abstract void Use(BattleUnit caster, BattleUnit receiver);
}

public class 가학 : Passive
{
    private bool isApplied = false;

    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (isApplied)
            return;

        caster.ChangedStat.ATK += 3;
        isApplied = true;
    }
}

public class 흡수 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        double heal = caster.Stat.ATK * 0.3;
        caster.ChangeHP(((int)heal));
    }
}

public class 처형 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (receiver.HP.GetCurrentHP() <= 10)
        {
            receiver.ChangeHP(-receiver.HP.GetCurrentHP());
        }
    }
}

public class 대죄 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        receiver.ChangeFall(1);
    }
}

public class 고양 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);

        List<BattleUnit> targetUnits = GameManager.Battle.GetArroundUnits(targetCoords);

        foreach(BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
                unit.ChangedStat.ATK += 5;
        }
    }
}

public class 자애 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);
        targetCoords.Add(caster.Location + new Vector2(-1, -1));
        targetCoords.Add(caster.Location + new Vector2(-1, 1));
        targetCoords.Add(caster.Location + new Vector2(1, 1));
        targetCoords.Add(caster.Location + new Vector2(1, -1));

        List<BattleUnit> targetUnits = GameManager.Battle.GetArroundUnits(targetCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
                unit.ChangeHP(20);
        }
    }
}

public class 강림 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);

        List<BattleUnit> targetUnits = GameManager.Battle.GetArroundUnits(targetCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team != caster.Team)
                unit.ChangeHP(-15);
        }
    }
}