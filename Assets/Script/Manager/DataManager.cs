using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    #region BattleCharList

    #region BattleCharList  
    List<Character> _BattleCharList = new List<Character>();
    public List<Character> BattleCharList => _BattleCharList;
    #endregion  

    // 리스트에 캐릭터를 추가 / 제거
    #region CharEnter / Exit
    public void BCL_CharEnter(Character ch)
    {
        BattleCharList.Add(ch);
    }
    public void BCL_CharExit(Character ch)
    {
        BattleCharList.Remove(ch);
    }
    #endregion

    #region OrderSort

    public void BattleOrderReplace()
    {
        BCL_SpeedSort();
    }

    // 일단 선택 정렬으로 정렬, 나중에 바꾸기
    void BCL_SpeedSort()
    {
        for (int i = 0; i < BattleCharList.Count; i++)
        {
            Character max = null;
            for (int j = i; j < BattleCharList.Count; j++)
            {
                if (i == j)
                {
                    max = BattleCharList[j];
                }
                else if (BattleCharList[j].GetSpeed() > max.GetSpeed())
                {
                    CharSwap(i, j);
                }
                else if (BattleCharList[j].GetSpeed() == max.GetSpeed())
                {
                    if (BattleCharList[j].LocX < max.LocX)
                    {
                        CharSwap(i, j);
                    }
                    else if (BattleCharList[j].LocX == max.LocX)
                    {
                        if (BattleCharList[j].LocY < max.LocY)
                        {
                            CharSwap(i, j);
                        }
                    }
                }
            }
        }
    }

    void CharSwap(int a, int b)
    {
        Character dump = BattleCharList[a];
        BattleCharList[a] = BattleCharList[b];
        BattleCharList[b] = dump;
    }

    #endregion

    #endregion

    #region FieldData

    const int MaxFieldX = 8;
    const int MaxFieldY = 3;

    public Tile[,] TileArray = new Tile[MaxFieldY, MaxFieldX];


    public void FieldSet(Transform trans, GameObject TilePrefabs)
    {
        TileArray = new Tile[MaxFieldY, MaxFieldX];

        Vector3 vec = trans.position;

        float disX = trans.localScale.x / MaxFieldX;
        float disY = trans.localScale.y / MaxFieldY;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                float x = (disX * j) + (disX * 0.5f);
                float y = disY * i;

                GameObject tile = GameObject.Instantiate(TilePrefabs, trans);
                tile.transform.position = new Vector3(vec.x + x, vec.y + y);

                TileArray[i + 1, j + 4] = tile.GetComponent<Tile>();
                TileArray[i + 1, j + 4].GetLocate(i + 1, j + 4);
            }
        }
    }

    public Vector3 GetTileLocate(int x, int y)
    {
        Vector3 vec = TileArray[y, x].transform.position;

        float sizeX = TileArray[y, x].transform.localScale.x * 0.5f;
        float sizeY = TileArray[y, x].transform.localScale.y * 0.5f;

        vec.x += sizeX;
        vec.y += sizeY;

        return vec;
    }

    public void CanSelectClear()
    {
        for (int i = 0; i < MaxFieldY; i++)
        {
            for (int j = 0; j < MaxFieldX; j++)
            {
                TileArray[i, j].SetCanSelect(false);
            }
        }
    }

    #endregion

    #region ManaGuage

    const int _MaxManaCost = 10;
    public int MaxManaCost => _MaxManaCost;
    public int ManaCost = 0;

    public void InitMana()
    {
        ManaCost = 0;
    }

    public void AddMana(int value)
    {
        if (10 <= ManaCost + value)
            ManaCost = 10;
        else
            ManaCost += value;
    }

    #endregion
}
