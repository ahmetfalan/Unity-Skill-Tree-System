using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHub : MonoBehaviour
{

    public SkillButton[] skillsButton;

    public void RefreshButtons()
    {
        for (int i = 0; i < skillsButton.Length; ++i)
        {
            skillsButton[i].RefreshState();
        }
    }
}