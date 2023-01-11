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
using UnityEngine.EventSystems;
using TMPro;

public class SelectMove : MonoBehaviour
{
    public Material highlightMaterial;
    public Material selectionMaterial;
    public GameObject inputPosXGameObj;
    public GameObject inputPosYGameObj;
    public GameObject inputPosZGameObj;

    private Material originalMaterial;
    private Transform highlight;
    private Transform selectedTransform;
    private RaycastHit raycastHit;
    private TMP_InputField inputPosX;
    private TMP_InputField inputPosY;
    private TMP_InputField inputPosZ;
    private float posX;
    private float posY;
    private float posZ;


    private void Start()
    {
        // Get InputField components from the GameObjects
        inputPosX = inputPosXGameObj.GetComponent<TMP_InputField>();
        inputPosY = inputPosYGameObj.GetComponent<TMP_InputField>();
        inputPosZ = inputPosZGameObj.GetComponent<TMP_InputField>();
    }


    void Update()
    {
        // Highlight an object on mouse-over if it has a Selectable tag using Raycast
        if (highlight != null)
        {
            highlight.GetComponent<MeshRenderer>().material = originalMaterial;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Ensure you have EventSystem in the Editor hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selectedTransform)
            {
                if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                {
                    originalMaterial = highlight.GetComponent<MeshRenderer>().material;
                    highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Select an object on the mouse Click if it has a Selectable tag using Raycast 
        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (selectedTransform != null)
            {
                selectedTransform.GetComponent<MeshRenderer>().material = originalMaterial;
                selectedTransform = null;
            }
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
            {
                selectedTransform = raycastHit.transform;
                if (selectedTransform.CompareTag("Selectable"))
                {
                    selectedTransform.GetComponent<MeshRenderer>().material = selectionMaterial;
                    GetSelectedPos();
                }
                else
                {
                    selectedTransform = null;
                    GetSelectedPos();
                }
            }
            else
            {
                selectedTransform = null;
                GetSelectedPos();
            }
        }
    }


    // Assign the positions X, Y, and Z of the selected Object to input fields. Hide the input fields if no object is selected
    private void GetSelectedPos()
    {
        if (selectedTransform)
        {
            inputPosXGameObj.SetActive(true);
            inputPosYGameObj.SetActive(true);
            inputPosZGameObj.SetActive(true);
            inputPosX.text = selectedTransform.position.x.ToString();
            inputPosY.text = selectedTransform.position.y.ToString();
            inputPosZ.text = selectedTransform.position.z.ToString();
        }
        else
        {
            inputPosXGameObj.SetActive(false);
            inputPosYGameObj.SetActive(false);
            inputPosZGameObj.SetActive(false);
        }
    }


    // Attach to OnValueChanged event of corresponding input fields. Get value from the field and call the SetSelectedPos method
    public void SetPosX()
    {
        if (float.TryParse(inputPosX.text, out posX))
        {
            SetSelectedPos();
        }
    }
    public void SetPosY()
    {
        if (float.TryParse(inputPosY.text, out posY))
        {
            SetSelectedPos();
        }
    }
    public void SetPosZ()
    {
        if (float.TryParse(inputPosZ.text, out posZ))
        {
            SetSelectedPos();
        }
    }


    // Set the position of the selected object to match the user input
    public void SetSelectedPos()
    {
        if (selectedTransform && (inputPosX.isFocused || inputPosY.isFocused || inputPosZ.isFocused))
        {

            selectedTransform.position = new Vector3(posX, posY, posZ);
            inputPosX.text = selectedTransform.position.x.ToString();
            inputPosY.text = selectedTransform.position.y.ToString();
            inputPosZ.text = selectedTransform.position.z.ToString();
        }
        else
        {
            inputPosXGameObj.SetActive(false);
            inputPosYGameObj.SetActive(false);
            inputPosZGameObj.SetActive(false);
        }
    }
}











