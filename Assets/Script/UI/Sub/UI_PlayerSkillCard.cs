using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_PlayerSkillCard : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private TextMeshProUGUI _text;

    private UI_PlayerSkill _playerSkill;
    private PlayerSkill _skill;
    public bool IsSelected = false;

    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void Set(UI_PlayerSkill ps, PlayerSkill skill)
    {
        _playerSkill = ps;
        _skill = skill;
        _text.text = skill.GetName();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected)
            return;
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerSkill.OnClickHand(this);
    }

    public void ChangeSelectState(bool b)
    {
        IsSelected = b;
        _highlight.SetActive(b);
    }

    public PlayerSkill GetSkill() => _skill;
}
