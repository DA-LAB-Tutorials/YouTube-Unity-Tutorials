using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetSetImage : MonoBehaviour
{
    public GameObject imageGameObj;
    public GameObject inputWidthGameObj;
    public GameObject inputHeightGameObj;

    TMP_InputField inputWidth;
    TMP_InputField inputHeight;
    Image image;
    float width;
    float height;
    Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        image = imageGameObj.GetComponent<Image>();
        inputWidth = inputWidthGameObj.GetComponent<TMP_InputField>();
        inputHeight = inputHeightGameObj.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame void Update(){}

    public void GetImageSize()
    {
        rect = image.rectTransform.rect;
        width = rect.width;
        height = rect.height;
        inputWidth.text = width.ToString();
        inputHeight.text = height.ToString();
    }

    public void SetImageSize()
    {
        Debug.Log("inputWidth.text: " + inputWidth.text);
        width = float.Parse(inputWidth.text);
        height = float.Parse(inputHeight.text);
        image.rectTransform.sizeDelta = new Vector2(width, height);
    }
}
