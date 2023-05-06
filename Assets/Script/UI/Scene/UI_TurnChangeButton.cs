using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_TurnChangeButton : UI_Scene
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TurnChange()
    {
        PhaseController _phase = BattleManager.Phase;

        if (_phase.Current == _phase.Prepare)
            _phase.ChangePhase(_phase.Engage);
        else if (_phase.Current == _phase.Move)
            _phase.ChangePhase(_phase.Action);
        else
        {
            BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnit());
            _phase.ChangePhase(_phase.Engage);
        }
    }
}
