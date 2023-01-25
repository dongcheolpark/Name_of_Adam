using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion
    #region BattleManager
    [SerializeField] BattleManager _battleMNG;
    public BattleManager BattleMNG => _battleMNG;
    #endregion
    #region CutSceneMNG
    [SerializeField] private CutSceneManager _CutSceneMNG;
    public CutSceneManager CutSceneMNG => _CutSceneMNG;
    #endregion
    #region StageManager
    StageManager _stageMNG;
    public StageManager StageMNG => _stageMNG;
    #endregion
    #region FieldMNG
    [SerializeField] private FieldManager _FieldMNG;
    public FieldManager FieldMNG => _FieldMNG;
    #endregion
    #region DataManager
    DataManager _dataMNG;
    public DataManager DataMNG => _dataMNG;
    #endregion
    #region SceneChanger
    SceneChanger _sceneChanger;
    public SceneChanger SceneChanger => _sceneChanger;
    #endregion


    #region InputManager
    [SerializeField] UIManager _UIMNG;
    public UIManager UIMNG => _UIMNG;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        _dataMNG = new DataManager();
        // _stageMNG = new StageManager();
        // _sceneChanger = new SceneChanger();
    }

    private void Update()
    {
        // 디버그용 입력기
        if (Input.GetMouseButtonDown(1))
        {
            BattleMNG.EngageStart();
        }
        else if (Input.GetMouseButtonDown(2))
        {
            BattleMNG.BattleDataMNG.BattleUnitList[0].UnitMove.MoveOnTile();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  SceneChanger.SceneChange("StageSelectScene");
        }
    }
}
