using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject skillTreePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            skillTreePanel.SetActive(true);
        }
        if (Input.GetKeyUp("e"))
        {
            skillTreePanel.SetActive(false);
        }
    }
}
