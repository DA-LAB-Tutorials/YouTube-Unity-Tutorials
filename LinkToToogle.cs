using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.UI;

public class LinkToToogle : MonoBehaviour
{
    public GameObject tmpGameOBJ;
    private TMP_Text tmpText;
    private bool isToggleOn = true;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = tmpGameOBJ.GetComponent<TMP_Text>();
        tmpText.text = "Toggle is On!";
    }

    // Update is called once per frame void Update() {}

    public void OnToggleChange(bool tickOn)
    {
        if(tickOn)
        {
            tmpText.text = "Toggle is On!";
        }
        else
        {
            tmpText.text = "Toggle is Off!";
        }
        isToggleOn = tickOn; //Pass boolean to another variable if needed
    }
}
