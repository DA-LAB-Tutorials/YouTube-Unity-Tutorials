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

public class UI : MonoBehaviour
{
    public GameObject fileMenuBody;
    public GameObject fileMenuExpand;
    public GameObject fileMenuCollapse;

    // Update is called once per frame
    void Update()
    {
        if (fileMenuCollapse.activeSelf)
        {
            if (!IsMouseOverUI(fileMenuBody) && !IsMouseOverUI(fileMenuCollapse))
            {
                fileMenuCollapseButton.onClick.Invoke();
            }
        }
    }

    private bool IsMouseOverUI(GameObject ui)
    {
        if (ui.activeSelf)
        {
            RectTransform rectTrasform = ui.GetComponent<RectTransform>();
            Vector2 localMousePos = rectTrasform.InverseTransformPoint(Input.mousePosition); 
            if (rectTrasform.rect.Contains(localMousePos)) 
            {
                return true;
            }
        }
        return false;
    }

}
