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
using System.IO;

public class SaveMesh : MonoBehaviour
{
    public GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        //Code generated mesh: https://youtu.be/_-UPgQ-k4ds 
        Vector3[] vertices = new Vector3[] { new Vector3(-1f, 1f, -1f), new Vector3(1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, -1f), new Vector3(1f, -1f, 1f), new Vector3(1f, 1f, -1f), new Vector3(1f, 1f, 1f), new Vector3(1f, -1f, -1f), new Vector3(-1f, -1f, 1f), new Vector3(1f, -1f, 1f), new Vector3(-0.999999f, 1f, 1.000001f), new Vector3(1f, 1f, 1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, 1f), new Vector3(-1f, 1f, -1f), new Vector3(-0.999999f, 1f, 1.000001f), new Vector3(-1f, 1f, -1f), new Vector3(-0.999999f, 1f, 1.000001f), new Vector3(1f, 1f, -1f), new Vector3(1f, 1f, 1f), new Vector3(-1f, -1f, -1f), new Vector3(1f, -1f, 1f), new Vector3(-1f, -1f, 1f), new Vector3(1f, -1f, -1f) };
        Vector3[] normals = new Vector3[] { new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f) };
        int[] triangles = new int[] { 0, 1, 2, 0, 3, 1, 4, 5, 6, 4, 7, 5, 8, 9, 10, 10, 9, 11, 12, 13, 14, 14, 13, 15, 16, 17, 18, 18, 17, 19, 20, 21, 22, 20, 23, 21 };
        int[] indicies = new int[] { 0, 1, 2, 0, 3, 1, 4, 5, 6, 4, 7, 5, 8, 9, 10, 10, 9, 11, 12, 13, 14, 14, 13, 15, 16, 17, 18, 18, 17, 19, 20, 21, 22, 20, 23, 21 };
        Color color = new Color(0f, 0.5334923f, 1f, 1f);
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.SetIndices(indicies, MeshTopology.Triangles, 0);
        model = new GameObject();
        model.AddComponent<MeshFilter>();
        model.AddComponent<MeshRenderer>();
        model.GetComponent<MeshFilter>().mesh = mesh;
        model.GetComponent<MeshRenderer>().material.color = color;
    }

    // Update is called once per frame void Update(){ }

    // Convert array of Vector3 to string "Vx Vy Vz'v|'Vx Vy Vz'v|'Vx Vy Vz'v|'Vx Vy Vz"
    public string V3ArrToStr(Vector3[] v3Arr, string splitStr)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Vector3 v3 in v3Arr)
        {
            sb.Append(v3.x).Append(" ").Append(v3.y).Append(" ").Append(v3.z).Append(splitStr);
        }
        if (sb.Length > 0) 
        {
            sb.Remove(sb.Length - splitStr.Length, splitStr.Length); //remove last splitStr
        }
        return sb.ToString();
    }


    // Convert array of int to string "int int int int"
    public string intArrToStr(int[] intArr)
    {
        StringBuilder sb = new StringBuilder();
        foreach (int i in intArr)
        {
            sb.Append(i).Append(" ");
        }
        if (sb.Length > 0) 
        {
            sb.Remove(sb.Length - 1, 1); //remove the last " "
        }
        return sb.ToString();
    }


    // Convert RGBA colour to string: "r g b a"
    public string ColourToStr(Color colour)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(colour.r).Append(" ").Append(colour.g).Append(" ").Append(colour.b).Append(" ").Append(colour.a);
        return sb.ToString();
    }


    public void OnClickSave()
    {
        File.WriteAllText(Application.persistentDataPath + "/model.dalab", ModelToStr(ref model, "mo|"));
        Debug.Log(Application.persistentDataPath);
    }


    // Convery an GameObject model with meshes and materials inside to string 
    public string ModelToStr(ref GameObject model, string splitStr)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < model.GetComponentsInChildren<Renderer>().Length; i++)
        {
            Mesh mesh = model.GetComponentsInChildren<MeshFilter>()[i].mesh;
            Material material = model.GetComponentsInChildren<MeshRenderer>()[i].material;
            sb.Append(MeshToStr(mesh, material.color, "m|")).Append(splitStr);
        }
        if (sb.Length > 0) 
        {
            sb.Remove(sb.Length - splitStr.Length, splitStr.Length); // remove the last splitStr
        }
        return sb.ToString();
    }


    // Convert a mesh to string: vertices(v3'v|'v3'v|'v3'v|'v3)'m|'normals(v3'n|'v3'n|'v3'n|'v3)'m|'triangles(int int int int)'m|'indicies(int int int int)'m|'meshTopology'm|'colour(r g b a)
    public string MeshToStr(Mesh mesh, Color meshColour, string splitStr)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(V3ArrToStr(mesh.vertices, "v|")).Append(splitStr).Append(V3ArrToStr(mesh.normals, "n|")).Append(splitStr).Append(intArrToStr(mesh.triangles))
          .Append(splitStr).Append(intArrToStr(mesh.GetIndices(0))).Append(splitStr).Append(mesh.GetTopology(0)).Append(splitStr).Append(ColourToStr(meshColour));
        return sb.ToString();
    }


}
