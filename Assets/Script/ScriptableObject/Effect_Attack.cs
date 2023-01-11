using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    targeting,
    rangeAttack,

    none
}

[CreateAssetMenu(fileName = "Effect_Attack", menuName = "Scriptable Object/Effect_Attack", order = 3)]
public class Effect_Attack : EffectSO
{
    [SerializeField] public AttackType attackType;
    [SerializeField] RangeSO range;    // 공격 범위
    [SerializeField] float DMG;        // 데미지 배율


    // 공격 실행
    public override void Effect(BattleUnit caster)
    {
        FieldManager _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG;
        List<Vector2> RangeList = GetRange();

        if (attackType == AttackType.rangeAttack)
        {
            List<BattleUnit> _BattleUnits = new List<BattleUnit>();

            // 공격 범위를 향해 공격
            for (int i = 0; i < RangeList.Count; i++)
            {
                BattleUnit _unit = null;
                int x = caster.UnitMove.LocX - (int)RangeList[i].x;
                int y = caster.UnitMove.LocY - (int)RangeList[i].y;
                
                _unit = _FieldMNG.RangeCheck(caster, x, y);

                if (_unit != null)
                    _BattleUnits.Add(_unit);
            }

            caster.UnitAction.OnAttackRange(_BattleUnits);
        }
        else if (attackType == AttackType.targeting)
        {

            int x = (int)caster.SelectTile.x;
            int y = (int)caster.SelectTile.y;

            if (x == -1 && y == -1)
            {
                x = caster.UnitMove.LocX;
                y = caster.UnitMove.LocY;
            }

            BattleUnit _unit = null;
            
            _unit = _FieldMNG.RangeCheck(caster, x, y);

            caster.UnitAction.OnAttackTarget(_unit);
        }
    }

    public List<Vector2> GetRange() => range.GetRange();
}
