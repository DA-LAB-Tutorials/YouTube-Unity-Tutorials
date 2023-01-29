//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

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
