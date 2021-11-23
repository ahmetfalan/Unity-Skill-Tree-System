using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    public Tooltip tooltip;
    void Start()
    {
        Instance = this;
    }

    public static void Show(int cost, string tittle = "", string description= "")
    {
        Instance.tooltip.SetText(cost, tittle, description);
        Instance.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        Instance.tooltip.gameObject.SetActive(false);
    }
}
