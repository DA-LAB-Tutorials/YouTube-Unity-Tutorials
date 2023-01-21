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
using UnityEngine.Networking;
using System;

public class Mapbox : MonoBehaviour
{
    public string accessToken;
    public float centerLatitude = -33.8873f;
    public float centerLongitude = 151.2189f;
    public float zoom = 12.0f;
    public int bearing = 0;
    public int pitch = 0;
    public enum style {Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets};
    public style mapStyle = style.Streets;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    private int mapWidth = 800;
    private int mapHeight = 600;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11" };
    private string url = "";
    private bool mapIsLoading = false; 
    private Rect rect;
    private bool updateMap = true;

    private string accessTokenLast;
    private float centerLatitudeLast = -33.8873f;
    private float centerLongitudeLast = 151.2189f;
    private float zoomLast = 12.0f;
    private int bearingLast = 0;
    private int pitchLast = 0;
    private style mapStyleLast = style.Streets;
    private resolution mapResolutionLast = resolution.low;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMapbox());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast, centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom || bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }
    }


    IEnumerator GetMapbox()
    {
        url = "https://api.mapbox.com/styles/v1/mapbox/" + styleStr[(int)mapStyle] + "/static/" + centerLongitude + "," + centerLatitude + "," + zoom + "," + bearing + "," + pitch + "/" + mapWidth + "x" + mapHeight + "?" + "access_token=" + accessToken;
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            centerLatitudeLast = centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
        }
    }
}



