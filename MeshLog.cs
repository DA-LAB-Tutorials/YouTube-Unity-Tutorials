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
using System.Text;
using System;

public class MeshLog : MonoBehaviour
{
    public GameObject model;
    private Color modelColor;
    // Start is called before the first frame update void Start(){}
    // Update is called once per frame void Update(){}

    private string V3ArrayToStr(string varStr, Vector3[] v3Arr)
    {
        string str = "Vector3[] " + varStr + " = new Vector3[] {";
        foreach (Vector3 vertex in v3Arr)
        {
            str = str + "new Vector3(" + vertex.x + "f, " + vertex.y + "f, " + vertex.z + "f), ";
        }
        if (str.Length > 1)
        {
            str = str.Remove(str.Length - 2); //Remove comma and the space at the end
        }
        str += "};";
        return str;
    }


    private string IntArrayToStr(string varStr, int[] intArr)
    {
        string str = "int[] " + varStr + " = new int[] {";
        foreach (int index in intArr)
        {
            str = str + index + ", ";
        }
        if (str.Length > 1)
        {
            str = str.Remove(str.Length - 2); //Remove comma and the space at the end
        }
        str += "};";
        return str;
    }

    public void PrintModel()
    {
        for (int i = 0; i < model.GetComponentsInChildren<MeshFilter>().Length; i++)
        {
            Mesh mesh = model.GetComponentsInChildren<MeshFilter>()[i].mesh;
            modelColor =  model.GetComponentsInChildren<MeshRenderer>()[i].material.color;
            PrintMesh(mesh);
        }
    }


    private void PrintMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;
        int[] indicies = mesh.GetIndices(0);
        if (model.GetComponent<MeshRenderer>() == null)
        {
            model.AddComponent<MeshRenderer>();
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(V3ArrayToStr("vertices", vertices)); //vertices
        sb.Append(Environment.NewLine);
        sb.Append(V3ArrayToStr("normals", normals)); //normals
        sb.Append(Environment.NewLine);
        sb.Append(IntArrayToStr("triangles", triangles)); //triangles
        sb.Append(Environment.NewLine);
        sb.Append(IntArrayToStr("indicies", indicies)); //indicies
        sb.Append(Environment.NewLine);
        sb.Append("Color color = new Color(" + modelColor.r + "f, " + modelColor.g + "f, " + modelColor.b + "f, " + modelColor.a + "f);");
        sb.Append(Environment.NewLine);
        sb.Append("Mesh mesh = new Mesh();");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.vertices = vertices;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.normals = normals;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.triangles = triangles;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.SetIndices(indicies, MeshTopology." + mesh.GetTopology(0) + ", 0);");
        sb.Append(Environment.NewLine);
        sb.Append("GameObject meshGameObj = new GameObject();");
        sb.Append(Environment.NewLine);
        sb.Append("meshGameObj.AddComponent<MeshFilter>();");
        sb.Append(Environment.NewLine);
        sb.Append("meshGameObj.AddComponent<MeshRenderer>();");
        sb.Append(Environment.NewLine);
        sb.Append("meshGameObj.GetComponent<MeshFilter>().mesh = mesh;");
        sb.Append(Environment.NewLine);
        sb.Append("meshGameObj.GetComponent<MeshRenderer>().material.color = color;");

        Debug.Log(sb);
    }

}
