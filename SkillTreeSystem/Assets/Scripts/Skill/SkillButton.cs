using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{

    public int skillId;

    public Color unlockedColor;

    public SkillHub skillHub;

    private Image _image;
    private Button _button;

    //public Image _branch;

    public string Tittle;
    public string Description;
    public int Cost;
    void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        RefreshState();

        SkillTreeReader.Instance.SetText(skillId, ref Tittle, ref Description, ref Cost);
    }

    public void RefreshState()
    {
        if (SkillTreeReader.Instance.IsSkillUnlocked(skillId))
        {
            _image.color = unlockedColor;
            _button.interactable = false;
            //_branch.color = unlockedColor;
        }
        else if (!SkillTreeReader.Instance.CanSkillBeUnlocked(skillId))
        {
            _button.interactable = false;
            //_branch.color = unlockedColor;
        }
        else
        {
            _image.color = Color.white;
            //_branch.color = Color.white;
            _button.interactable = true;
        }
    }

    public void BuySkill()
    {
        if (SkillTreeReader.Instance.UnlockSkill(skillId))
        {
            PlayerPrefs.SetInt("Point", PlayerAttributes._points);
            PlayerControl.Instance.UpdateStats();
            skillHub.RefreshButtons();
            SkillTreeReader.Instance.SaveSkillTree();
        }
    }
}