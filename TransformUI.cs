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

public class TransformUI : MonoBehaviour
{
    public Material highlightMaterial;
    public Material selectionMaterial;
    public GameObject inputPosXGameObj;
    public GameObject inputPosYGameObj;
    public GameObject inputPosZGameObj;
    public GameObject inputRotationXGameObj;
    public GameObject inputRotationYGameObj;
    public GameObject inputRotationZGameObj;
    public GameObject inputScaleXGameObj;
    public GameObject inputScaleYGameObj;
    public GameObject inputScaleZGameObj;
    public GameObject axisDropDownGameObj;
    public GameObject scaleDropDownGameObj;

    private Material originalMaterialHighlight;
    private Material originalMaterialSelection;
    private Transform highlight;
    private Transform selection;
    private Ray ray;
    private RaycastHit raycastHit;
    private TMP_InputField inputPosX;
    private TMP_InputField inputPosY;
    private TMP_InputField inputPosZ;
    private TMP_InputField inputRotationX;
    private TMP_InputField inputRotationY;
    private TMP_InputField inputRotationZ;
    private TMP_InputField inputScaleX;
    private TMP_InputField inputScaleY;
    private TMP_InputField inputScaleZ;
    private float posX;
    private float posY;
    private float posZ;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private float scaleX;
    private float scaleY;
    private float scaleZ;
    private TMP_Dropdown axisDropDown;
    private TMP_Dropdown scaleDropDown;
    private bool isAxisGlobal = true;
    private bool isScaleEven = true;

    private void Start()
    {
        inputPosX = inputPosXGameObj.GetComponent<TMP_InputField>();
        inputPosY = inputPosYGameObj.GetComponent<TMP_InputField>();
        inputPosZ = inputPosZGameObj.GetComponent<TMP_InputField>();
        inputRotationX = inputRotationXGameObj.GetComponent<TMP_InputField>();
        inputRotationY = inputRotationYGameObj.GetComponent<TMP_InputField>();
        inputRotationZ = inputRotationZGameObj.GetComponent<TMP_InputField>();
        inputScaleX = inputScaleXGameObj.GetComponent<TMP_InputField>();
        inputScaleY = inputScaleYGameObj.GetComponent<TMP_InputField>();
        inputScaleZ = inputScaleZGameObj.GetComponent<TMP_InputField>();
        axisDropDown = axisDropDownGameObj.GetComponent<TMP_Dropdown>();
        scaleDropDown = scaleDropDownGameObj.GetComponent<TMP_Dropdown>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            // Highlight
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (highlight != null)
            {
                highlight.GetComponent<MeshRenderer>().sharedMaterial = originalMaterialHighlight;
                highlight = null;
            }
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
            {
                highlight = raycastHit.transform;
                if (highlight.CompareTag("Selectable") && highlight != selection)
                {
                    if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                    {
                        originalMaterialHighlight = highlight.GetComponent<MeshRenderer>().material;
                        highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                    }
                }
                else
                {
                    highlight = null;
                }
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (highlight)
                {
                    if (selection != null)
                    {
                        selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                    }
                    selection = raycastHit.transform;
                    if (selection.GetComponent<MeshRenderer>().material != selectionMaterial)
                    {
                        originalMaterialSelection = originalMaterialHighlight;
                        selection.GetComponent<MeshRenderer>().material = selectionMaterial;
                    }
                    highlight = null;
                }
                else
                {
                    if (selection)
                    {
                        selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                        selection = null;
                    }
                }
            }
            else
            {
                if (selection)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                    selection = null;
                }
            }
        }

        if (selection && !inputPosX.isFocused && !inputPosY.isFocused && !inputPosZ.isFocused)
        {
            GetSelectedPos();
        }
        if (selection && !inputRotationX.isFocused && !inputRotationY.isFocused && !inputRotationZ.isFocused)
        {
            GetSelectedRotation();
        }
        if (selection && !inputScaleX.isFocused && !inputScaleY.isFocused && !inputScaleZ.isFocused)
        {
            GetSelectedScale();
        }
    }


    /// ----------------------------------------------///
    /// Position
    /// ----------------------------------------------///
    private void GetSelectedPos()
    {
        if (selection)
        {
            Vector3 currPos;
            if (isAxisGlobal)
            {
                currPos = selection.position;
            }
            else
            {
                currPos = selection.InverseTransformPoint(selection.position); //world to local
            }
            inputPosXGameObj.SetActive(true);
            inputPosYGameObj.SetActive(true);
            inputPosZGameObj.SetActive(true);
            posX = currPos.x;
            posY = currPos.y;
            posZ = currPos.z;
            posX = Mathf.Round(posX * 1000f) / 1000f;
            posY = Mathf.Round(posY * 1000f) / 1000f;
            posZ = Mathf.Round(posZ * 1000f) / 1000f;
            inputPosX.text = posX.ToString();
            inputPosY.text = posY.ToString();
            inputPosZ.text = posZ.ToString();
        }
        else
        {
            inputPosXGameObj.SetActive(false);
            inputPosYGameObj.SetActive(false);
            inputPosZGameObj.SetActive(false);
        }
    }

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

    public void SetSelectedPos()
    {
        if (selection)
        {
            if (isAxisGlobal)
            {
                selection.position = new Vector3(posX, posY, posZ);
            }
            else
            {
                selection.position = selection.TransformPoint(new Vector3(posX, posY, posZ)); //local to world
            }
        }
    }


    /// ----------------------------------------------///
    /// Rotation
    /// ----------------------------------------------///
    private void GetSelectedRotation()
    {
        if (selection)
        {
            Vector3 currRotation;
            if (isAxisGlobal)
            {
                currRotation = selection.eulerAngles;
            }
            else
            {
                currRotation = Vector3.zero;
            }
            inputRotationXGameObj.SetActive(true);
            inputRotationYGameObj.SetActive(true);
            inputRotationZGameObj.SetActive(true);
            rotationX = currRotation.x;
            rotationY = currRotation.y;
            rotationZ = currRotation.z;
            rotationX = Mathf.Round(rotationX * 1000f) / 1000f;
            rotationY = Mathf.Round(rotationY * 1000f) / 1000f;
            rotationZ = Mathf.Round(rotationZ * 1000f) / 1000f;
            inputRotationX.text = rotationX.ToString();
            inputRotationY.text = rotationY.ToString();
            inputRotationZ.text = rotationZ.ToString();
        }
        else
        {
            inputRotationXGameObj.SetActive(false);
            inputRotationYGameObj.SetActive(false);
            inputRotationZGameObj.SetActive(false);
        }
    }

    public void SetRotationX()
    {
        if (float.TryParse(inputRotationX.text, out rotationX))
        {
            SetSelectedRotation();
        }
    }
    public void SetRotationY()
    {
        if (float.TryParse(inputRotationY.text, out rotationY))
        {
            SetSelectedRotation();
        }
    }
    public void SetRotationZ()
    {
        if (float.TryParse(inputRotationZ.text, out rotationZ))
        {
            SetSelectedRotation();
        }
    }

    public void SetSelectedRotation()
    {
        if (selection)
        {
            if (isAxisGlobal)
            {
                selection.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
            }
            else
            {
                selection.eulerAngles = selection.eulerAngles + selection.TransformVector(new Vector3(rotationX, rotationY, rotationZ));
                rotationX = 0.0f;
                rotationY = 0.0f;
                rotationZ = 0.0f;
            }
        }
    }


    /// ----------------------------------------------///
    /// Scale
    /// ----------------------------------------------///
    private void GetSelectedScale()
    {
        if (selection)
        {
            Vector3 currScale;
            if (isAxisGlobal)
            {
                currScale = selection.lossyScale;
            }
            else
            {
                currScale = Vector3.one;
            }
            inputScaleXGameObj.SetActive(true);
            inputScaleYGameObj.SetActive(true);
            inputScaleZGameObj.SetActive(true);
            scaleX = currScale.x;
            scaleY = currScale.y;
            scaleZ = currScale.z;
            scaleX = Mathf.Round(scaleX * 1000f) / 1000f;
            scaleY = Mathf.Round(scaleY * 1000f) / 1000f;
            scaleZ = Mathf.Round(scaleZ * 1000f) / 1000f;
            inputScaleX.text = scaleX.ToString();
            inputScaleY.text = scaleY.ToString();
            inputScaleZ.text = scaleZ.ToString();
        }
        else
        {
            inputScaleXGameObj.SetActive(false);
            inputScaleYGameObj.SetActive(false);
            inputScaleZGameObj.SetActive(false);
        }
    }

    public void SetScaleX()
    {
        if (float.TryParse(inputScaleX.text, out scaleX))
        {
            if (isScaleEven)
            {
                float scaleDiff;
                if (isAxisGlobal)
                {
                    scaleDiff = scaleX / selection.localScale.x;
                }
                else
                {
                    scaleDiff = scaleX;
                }
                scaleX = selection.localScale.x * scaleDiff;
                scaleY = selection.localScale.y * scaleDiff;
                scaleZ = selection.localScale.z * scaleDiff;
            }
            else if(!isAxisGlobal)
            {
                scaleX = selection.localScale.x * scaleX;
                scaleY = selection.localScale.y;
                scaleZ = selection.localScale.z;
            }
            SetSelectedScale();
        }
    }
    public void SetScaleY()
    {
        if (float.TryParse(inputScaleY.text, out scaleY))
        {
            if (isScaleEven)
            {
                float scaleDiff;
                if (isAxisGlobal)
                {
                    scaleDiff = scaleY / selection.localScale.y;
                }
                else
                {
                    scaleDiff = scaleY;
                }
                scaleX = selection.localScale.x * scaleDiff;
                scaleY = selection.localScale.y * scaleDiff;
                scaleZ = selection.localScale.z * scaleDiff;
            }
            else if (!isAxisGlobal)
            {
                scaleX = selection.localScale.x;
                scaleY = selection.localScale.y * scaleY;
                scaleZ = selection.localScale.z;
            }
            SetSelectedScale();
        }
    }
    public void SetScaleZ()
    {
        if (float.TryParse(inputScaleZ.text, out scaleZ))
        {
            if (isScaleEven)
            {
                float scaleDiff;
                if (isAxisGlobal)
                {
                    scaleDiff = scaleZ / selection.localScale.z;
                }
                else
                {
                    scaleDiff = scaleZ;
                }
                scaleX = selection.localScale.x * scaleDiff;
                scaleY = selection.localScale.y * scaleDiff;
                scaleZ = selection.localScale.z * scaleDiff;
            }
            else if (!isAxisGlobal)
            {
                scaleX = selection.localScale.x;
                scaleY = selection.localScale.y;
                scaleZ = selection.localScale.z * scaleZ;
            }
            SetSelectedScale();
        }
    }

    public void SetSelectedScale()
    {
        if (selection)
        {
            selection.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
    }

    //Axis drop down menu
    public void OnAxisDropDownChange()
    {
        if (axisDropDown.value == 0)
        {
            isAxisGlobal = true;
        }
        else
        {
            isAxisGlobal = false;
        }
    }

    //Scale drop down menu
    public void OnScaleDropDownChange()
    {
        if (scaleDropDown.value == 0)
        {
            isScaleEven = true;
        }
        else
        {
            isScaleEven = false;
        }
    }

}