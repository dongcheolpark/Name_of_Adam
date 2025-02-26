using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeckButton : UI_Scene
{
    private bool _isBattle = true;

    public void Set(bool isBattle)
    {
        _isBattle = isBattle;
    }

    public void OnDeckButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(_isBattle);
    }
}