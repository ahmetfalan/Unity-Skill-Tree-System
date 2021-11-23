using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public Text tittleField;
    public Text descriptionField;
    public Text costField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    public RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(int cost, string tittle = "", string description = "")
    {
        if (string.IsNullOrEmpty(tittle))
        {
            tittleField.gameObject.SetActive(false);
        }
        else
        {
            tittleField.gameObject.SetActive(true);
            tittleField.text = tittle;
        }

        if (string.IsNullOrEmpty(description))
        {
            descriptionField.gameObject.SetActive(false);
        }
        else
        {
            descriptionField.gameObject.SetActive(true);
            descriptionField.text = description;
        }


        costField.text = "Cost: " + cost.ToString();

        LayoutShowHide();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            LayoutShowHide();
        }


        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    void LayoutShowHide()
    {
        int tittleLength = tittleField.text.Length;
        int descriptionLength = descriptionField.text.Length;

        layoutElement.enabled = (tittleLength > characterWrapLimit || descriptionLength > characterWrapLimit) ? true : false;
    }
}
