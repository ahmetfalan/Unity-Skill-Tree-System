using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointManager : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    void Start()
    {
        text.text = "Point: " + PlayerAttributes._points;
    }

    void Update()
    {
        text.text = "Point: " + PlayerAttributes._points;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Point", PlayerAttributes._points);
    }
}