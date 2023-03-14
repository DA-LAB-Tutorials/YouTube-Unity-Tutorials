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

public class DrawMeshLines : MonoBehaviour
{
    public Camera currentCamera;
    public Color lineColor;
    private float thresholdDist = 0.001f;
    private List<GameObject> linesMeshesObjs = new List<GameObject>();
    private List<Vector3> linesVertices = new List<Vector3>();
    private List<int> linesIndices = new List<int>();
    private bool isNewLine = false;
    private Vector3 endWorldPos = Vector3.one * float.MaxValue; //Highest value
    private int indexCount = 0;


    void Start()
    {
        currentCamera = Camera.main;
        Mesh newLinesMesh = new Mesh();
        newLinesMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; //By default mesh support 16-bit index format which support buffer of 65535
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshFilter>().mesh = newLinesMesh;
        gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        linesMeshesObjs.Add(gameObject);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Check for mouse left click
        {
            Vector3 mousePxlPos = Input.mousePosition;
            mousePxlPos.z = currentCamera.nearClipPlane + currentCamera.nearClipPlane * 0.01f; //Add a bit to near clip plane to make lines visible
            Vector3 mouseWorldPos = currentCamera.ScreenToWorldPoint(mousePxlPos);
            float distToEndPoint = Vector3.Distance(endWorldPos, mouseWorldPos);
            if (distToEndPoint <= thresholdDist)
            {
                return;
            }
            endWorldPos = mouseWorldPos;
            linesVertices.Add(mouseWorldPos);
            if (indexCount > 1) //Because MeshTopology.Lines needs two indices to draw each line
            {
                linesIndices.Add(indexCount - 1);
                linesIndices.Add(indexCount);
            }
            else
            {
                linesIndices.Add(indexCount);
            }
            UpdateLinesMesh();
            indexCount++;
        }

        if (Input.GetMouseButtonDown(1)) //Check for mouse right click
        {
            StartNewLinesMesh();
        }
    }


    private void UpdateLinesMesh()
    {
        GameObject currentGameObj = linesMeshesObjs[linesMeshesObjs.Count - 1];
        Mesh linesMesh = currentGameObj.GetComponent<MeshFilter>().mesh;
        linesMesh.vertices = linesVertices.ToArray();
        linesMesh.normals = new Vector3[linesVertices.Count]; //Populate with Zero normals to have lines with constant colour
        linesMesh.SetIndices(linesIndices.ToArray(), MeshTopology.Lines, 0); //Select lines for mesh topology
        currentGameObj.GetComponent<MeshRenderer>().material.color = lineColor;
        isNewLine = false;
    }


    private void StartNewLinesMesh()
    {
        if (!isNewLine)
        {
            GameObject newLinesGameObj = new GameObject();
            Mesh newLinesMesh = new Mesh();
            newLinesMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; //By default mesh support 16-bit index format which support buffer of 65535
            linesVertices = new List<Vector3>(); //Reset vertices list
            linesIndices = new List<int>(); //Reset indices list
            indexCount = 0; //Reset index count
            newLinesGameObj.AddComponent<MeshFilter>();
            newLinesGameObj.AddComponent<MeshRenderer>();
            newLinesGameObj.GetComponent<MeshFilter>().mesh = newLinesMesh;
            newLinesGameObj.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            linesMeshesObjs.Add(newLinesGameObj);
            isNewLine = true;
        }

    }
}




