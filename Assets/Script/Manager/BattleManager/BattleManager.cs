using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    #region BattleDataManager
    private BattleDataManager _BattleDataMNG;
    public BattleDataManager BattleDataMNG => _BattleDataMNG;
    #endregion

    private UI_WatingLine _WatingLine;
    private Field _field;
    public Field Field => _field;

    private List<BattleUnit> _BattleUnitOrderList;

    private void Awake()
    {
        _BattleDataMNG = new BattleDataManager();
        
        _BattleUnitOrderList = new List<BattleUnit>();
        _WatingLine = GameManager.UIMNG.WatingLine;
        _field = GameObject.Find("Field").GetComponent<Field>();

        PrepareStart();
    }

    #region StageControl
    public void PrepareStart()
    {
        Debug.Log("Prepare Start");
        //UI 튀어나옴
        //GameManager.InputMNG.Hands.comebackHands();
        //UI가 작동할 수 있게 해줌
    }

    public void PrepareEnd()
    {
        Debug.Log("Prepare End");
        _BattleDataMNG.PhaseChange();     
        //UI 들어감
        //GameManager.InputMNG.Hands.begoneHands();
        //UI 사용 불가
    }

    public void EngageStart()
    {
        Debug.Log("Engage Start");
        
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌

        _BattleUnitOrderList.Clear();

        foreach(BattleUnit unit in _BattleDataMNG.BattleUnitList)
        {
            _BattleUnitOrderList.Add(unit);
        }

        // 턴 시작 전에 다시한번 순서를 정렬한다.
        BattleOrderReplace();
        GameManager.BattleMNG.Field.ClearAllColor();

        _WatingLine.SetBattleUnitList(_BattleUnitOrderList);
        _WatingLine.SetWatingLine();

        UseUnitSkill();
    }

    public void EngageEnd()
    {
        Debug.Log("Engage End");
        _BattleDataMNG.PhaseChange();
        //UI 들어감
        //UI 사용 불가
        _BattleDataMNG.ChangeMana(2);
    }

        // BattleUnitList를 정렬
    // 1. 스피드 높은 순으로, 2. 같을 경우 왼쪽 위부터 오른쪽으로 차례대로
    public void BattleOrderReplace()
    {
        _BattleUnitOrderList = _BattleUnitOrderList.OrderByDescending(unit => unit.GetSpeed())
            .ThenByDescending(unit => unit.LocY)
            .ThenBy(unit => unit.LocX)
            .ToList();
    }

    // BattleUnitList의 첫 번째 요소부터 순회
    // 다음 차례의 공격 호출은 CutSceneMNG의 ZoomOut에서 한다.
    public void UseUnitSkill()
    {
        if (_BattleUnitOrderList.Count <= 0)
        {
            EngageEnd();
            return;
        }

        if (0 < _BattleUnitOrderList[0].CurHP)
        {
            _BattleUnitOrderList[0].SetState(BattleUnitState.Move);
        }
        else
        {
            UseNextUnit();
        }
    }

    public void UseNextUnit()
    {
        _BattleUnitOrderList.RemoveAt(0);
        _WatingLine.SetWatingLine();
        UseUnitSkill();
    }
    #endregion

    public BattleUnit GetNowUnit() => _BattleUnitOrderList[0];

    public void BattleUnitAI()
    {
        List<Vector2> FindTileList = new List<Vector2>();
        List<Vector2> RangedVectorList = new List<Vector2>();

        List<Vector2> AttackRangeList = _BattleUnitOrderList[0].BattleUnitSO.GetRange();

        //전달받은 범위에서 유닛을 찾는다.
        foreach (Vector2 arl in AttackRangeList)
        {
            int vecX = _BattleUnitOrderList[0].LocX + (int)arl.x;
            int vecY = _BattleUnitOrderList[0].LocY + (int)arl.y;

            if (0 <= vecX && vecX < 6/*MaxFieldX*/ && 0 <= vecY && vecY < 3/*MaxFieldY*/)
            {
                Vector2 vec = new Vector2(vecY ,vecX);
                if (_field.TileDict[vec].IsOnTile)
                {
                    FindTileList.Add(vec);
                }
            }
        }

        //찾은 유닛이 있는지 확인하고, 있다면 원거리인지, 근거리인지 확인한다.
        if (FindTileList.Count > 0)
        {
            foreach (Vector2 ftl in FindTileList)
            {
                if (_field.TileDict[ftl].UnitOnTile.BattleUnitSO.RType == RangeType.Ranged)
                {
                    RangedVectorList.Add(ftl);
                }
            }

            if (RangedVectorList.Count > 0)
            {
                //원거리 유닛이 있을 경우
                //Random.Range(0, RangeList.Count);
            }
            else
            {
                //근거리 유닛만 있을 경우
                //Random.Range(0, findUnitList.Count);
            }
        }
        else
        {
            //공격 범위 내에서 찾은 유닛이 없으면 이동하고 공격한다
            SortedSet<Vector3> AttackTileSet = new SortedSet<Vector3>();

            //모든 공격 타일을 AttackTileSet에 저장한다. X, Y는 좌표, Z는 원거리/근거리 유무
            foreach(BattleUnit unit in _BattleDataMNG.BattleUnitList)
            {
                if (unit.BattleUnitSO.MyTeam)
                {
                    foreach (Vector2 arl in AttackRangeList)
                    {
                        int vecX = unit.LocX - (int)arl.x;
                        int vecY = unit.LocY - (int)arl.y;
                        float vecZ;
                        if (unit.BattleUnitSO.RType == RangeType.Ranged)
                            vecZ = 0f;//원거리면 0
                        else
                            vecZ = 0.1f;//근거리면 0.1


                        AttackTileSet.Add(new Vector3(vecX, vecY, vecZ));
                    }
                }
            }

            //유닛을 때릴 수 있는 타일이 이동 범위 내에 있는 지 확인한다.
            //단 위, 아래, 왼, 오른쪽만 이동 가능하다고 가정
            for (int i = -1; i <= 1; i += 2)
            {
                for (float j = 0; j <= 0.1f; j += 0.1f)
                {
                    Vector3 vec1 = new Vector3(_BattleUnitOrderList[0].LocX + i, _BattleUnitOrderList[0].LocY, j);
                    if (AttackTileSet.Contains(vec1))
                    {
                        FindTileList.Add(vec1);
                    }

                    Vector3 vec2 = new Vector3(_BattleUnitOrderList[0].LocX, _BattleUnitOrderList[0].LocY + i, j);
                    if (AttackTileSet.Contains(vec2))
                    {
                        FindTileList.Add(vec2);
                    }
                }
            }


            if (FindTileList.Count > 0)
            {
                //이동해서 갈 수 있는 공격 타일이 있을 경우
                foreach(Vector3 v in FindTileList)
                {
                    if (v.z == 0)
                    {
                        RangedVectorList.Add(new Vector2(v.x, v.y));
                    }
                }

                if (RangedVectorList.Count > 0)
                {
                    //원거리가 있음
                    //Random.Range(0, RangedVectorList.Count);
                }
                else
                {
                    //근거리만 있음
                    //Random.Range(0, FindTileList.Count);
                }
            }
            else
            {
                Vector3 MyPosition =  new Vector3(_BattleUnitOrderList[0].LocX, _BattleUnitOrderList[0].LocY, 0);

                float dis = 100f;
                Vector3 minVec = new Vector3();

                foreach (Vector3 v in RangedVectorList)
                {
                    if (dis > Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y)) 
                    {
                        dis = Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y);
                        minVec = v;
                    }
                }
                //가장 가까운 타일 = minVec으로 이동

                dis = 100f;//재활용
                Vector3 moveVec = new Vector3();
                for (int i = -1; i <= 1; i += 2)
                {
                    Vector3 vec1 = new Vector3(MyPosition.x + i, MyPosition.y, 0);
                    if (dis > (vec1 - minVec).sqrMagnitude) 
                    {
                        dis = (vec1 - minVec).sqrMagnitude;
                        moveVec = vec1;
                    }

                    Vector3 vec2 = new Vector3(MyPosition.x, MyPosition.y + i, 0);
                    if (dis > (vec2 - minVec).sqrMagnitude)
                    {
                        dis = (vec2 - minVec).sqrMagnitude;
                        moveVec = vec2;
                    }
                }
                //moveVec으로 이동
            }
        }
    }
}