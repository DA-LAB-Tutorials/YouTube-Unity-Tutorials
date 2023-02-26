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
using System.IO;

public class OpenMesh : MonoBehaviour
{
    public GameObject model;
    // Start is called before the first frame update void Start(){ }
    // Update is called once per frame void Update(){ }


    // Convert string "Vx Vy Vz'v|'Vx Vy Vz'v|'Vx Vy Vz'v|'Vx Vy Vz" to array of Vector3
    public Vector3[] StrToV3Arr(string arrStr, string splitStr)
    {
        string[] v3StrArr = arrStr.Split(splitStr);
        Vector3[] result = new Vector3[v3StrArr.Length];
        for (int i = 0; i < v3StrArr.Length; i++)
        {
            string[] valuesStr = v3StrArr[i].Split(' ');
            if (valuesStr.Length != 3)
            {
                Debug.Log("component count mismatch. Expected 3 components but got " + valuesStr.Length);
            }
            result[i] = new Vector3(float.Parse(valuesStr[0]), float.Parse(valuesStr[1]), float.Parse(valuesStr[2]));
        }
        return result;
    }


    // Convert string "int int int int" to array of int 
    public int[] StrToIntArr(string arrStr)
    {
        string[] intStrArr = arrStr.Split(' ');
        int[] result = new int[intStrArr.Length];
        for (int i = 0; i < intStrArr.Length; i++)
        {
            result[i] = int.Parse(intStrArr[i]);
        }
        return result;
    }


    // Convert string: "r g b a" to colour
    public Color StrToColour(string colourStr)
    {
        Color resultColour = new Color();
        string[] colourStrArr = colourStr.Split(' ');
        if (colourStrArr.Length != 4)
        {
            Debug.Log("StrToColour count mismatch. Expected 4 components but got " + colourStrArr.Length);
        }
        else
        {
            resultColour.r = float.Parse(colourStrArr[0]);
            resultColour.g = float.Parse(colourStrArr[1]);
            resultColour.b = float.Parse(colourStrArr[2]);
            resultColour.a = float.Parse(colourStrArr[3]);
        }
        return resultColour;
    }


    public void OnClickOpen()
    {
        string path = Application.persistentDataPath + "/model.dalab";
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(path))
        {
            if (model != null)
            {
                foreach (Transform child in model.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            model = StrToModel(File.ReadAllText(path), "mo|"); //Read Model ;
        }
    }


    // Convert model string to parent gameobject model containing children gameobjects meshe and material
    public GameObject StrToModel(string modelStr, string splitStr)
    {
        GameObject resultModel = new GameObject();
        string[] modelStrArr = modelStr.Split(splitStr);
        for (int i = 0; i < modelStrArr.Length; i++)
        {
            GameObject resultObj = StrToGameObjectMesh(modelStrArr[i], "m|");
            resultObj.transform.parent = resultModel.transform; //make the gameobj with mesh and material inside child of the model
        }
        resultModel.transform.localScale = new Vector3(-1, 1, 1); // set the position of parent model. Reverse X to show properly
        return resultModel;
    }


    // Convert string mesh: vertices(v3'v|'v3'v|'v3'v|'v3)'m|'normals(v3'n|'v3'n|'v3'n|'v3)'m|'triangles(int int int int)'m|'indicies(int int int int)'m|'meshTopology'm|'colour(r g b a) to GameObject containing a mesh
    public GameObject StrToGameObjectMesh(string meshStr, string splitStr)
    {
        GameObject meshObj = new GameObject();
        Mesh resultMesh = new Mesh();
        resultMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; //By default mesh support 16-bit index format which support buffer of 65535 so we need to increase is to 32
        Color resultColor = new Color();
        string[] meshStrArr = meshStr.Split(splitStr);
        if (meshStrArr.Length != 6)
        {
            Debug.Log("StrToGameObjectMesh Warning. Number of elements in string array is: " + meshStrArr.Length + ". No mesh found!");
        }
        else
        {
            Vector3[] resultVertices = StrToV3Arr(meshStrArr[0], "v|");
            Vector3[] resultNormals = StrToV3Arr(meshStrArr[1], "n|");
            int[] resultTriangles = StrToIntArr(meshStrArr[2]);
            int[] resultIndices = StrToIntArr(meshStrArr[3]);
            string meshTopoStr = meshStrArr[4];
            resultColor = StrToColour(meshStrArr[5]);
            resultMesh.vertices = resultVertices;
            resultMesh.normals = resultNormals;
            resultMesh.triangles = resultTriangles;

            if (meshTopoStr == "Lines") //You can add conditions for other mesh topologies
            {
                resultMesh.SetIndices(resultIndices, MeshTopology.Lines, 0);
            }
            else
            {
                resultMesh.SetIndices(resultIndices, MeshTopology.Triangles, 0);
            }
        }
        meshObj.AddComponent<MeshFilter>();
        meshObj.AddComponent<MeshRenderer>();
        meshObj.GetComponent<MeshFilter>().mesh = resultMesh;
        meshObj.GetComponent<MeshRenderer>().material.color = resultColor;
        return meshObj;
    }




}
