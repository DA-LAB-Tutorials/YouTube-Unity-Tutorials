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
using UnityEngine.Networking;
using System;
using TMPro;

public class MapControl : MonoBehaviour
{
    //Public variables for Mapbox request
    public string accessToken;
    public double centerLongitude = 151.20601823658595; //Change to the longitude of the centre of the area you need
    public double centerLatitude = -33.87305769270712; //Change to the latitude of the centre of the area you need
    public enum style { Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets };
    public style mapStyle = style.Streets;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    public TextMeshProUGUI loadingText; //Text to show while map is loading

    //Private variables update map setting and update
    private double[] boundingBox;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11" };
    private string url = "";
    private bool updateMap = true;
    private bool ResetMap = false;
    private Material mapMaterial;
    private Vector2 screenResolution;
    private double mapWidthMeter;
    private double mapHeightMeter;
    private int mapWidthPx = 1280;
    private int mapHeightPx = 1280;
    private double planeToCameraDistance;
    private bool mapIsLoading = false;
    private int framesSinceMapLoaded = 0;
    private int minFramesSinceMapLoaded = 60;

    //Variables to keep track of change in public variables and trigger update when user change them
    private string accessTokenLast;
    private double centerLongitudeLast = 151.20601823658595;
    private double centerLatitudeLast = -33.87305769270712;
    private style mapStyleLast = style.Streets;
    private resolution mapResolutionLast = resolution.low;


    // Start is called before the first frame update
    void Start()
    {
        planeToCameraDistance = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
        screenResolution = new Vector2(Screen.width, Screen.height);
        MatchPlaneToScreenSize();
        if (gameObject.GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
        }
        mapMaterial = new Material(Shader.Find("Unlit/Texture"));
        gameObject.GetComponent<MeshRenderer>().material = mapMaterial;
        StartCoroutine(GetMapbox());
    }

    // Update is called once per frame
    void Update()
    {
        if (mapIsLoading)
        {
            loadingText.gameObject.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            framesSinceMapLoaded = 0;
        }
        else
        {
            framesSinceMapLoaded++;
            if (framesSinceMapLoaded > minFramesSinceMapLoaded) //Show map after defined time since last map load to avoid map flickering
            {
                loadingText.gameObject.SetActive(false);
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                framesSinceMapLoaded = 0;
            }
            if (ResetMap || screenResolution.x != Screen.width || screenResolution.y != Screen.height || !Mathf.Approximately((float)planeToCameraDistance, Vector3.Distance(gameObject.transform.position, Camera.main.transform.position))) //Check change to screen size or camera position
            {
                planeToCameraDistance = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
                screenResolution.x = Screen.width;
                screenResolution.y = Screen.height;
                MatchPlaneToScreenSize();
                StartCoroutine(GetMapbox());
                updateMap = false;
            }

            else if (ResetMap || (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately((float)centerLongitudeLast, (float)centerLongitude) || !Mathf.Approximately((float)centerLatitudeLast, (float)centerLatitude) || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))) //Check if user change any public variable
            {
                StartCoroutine(GetMapbox());
                updateMap = false;
            }
        }
    }


    IEnumerator GetMapbox()
    {
        mapIsLoading = true;
        boundingBox = GetRecMinMaxLonLat(centerLongitude, centerLatitude, mapWidthMeter, mapHeightMeter);
        url = "https://api.mapbox.com/styles/v1/mapbox/" + styleStr[(int)mapStyle] + "/static/[" + boundingBox[0] + "," + boundingBox[1] + "," + boundingBox[2] + "," + boundingBox[3] + "]/" + mapWidthPx + "x" + mapHeightPx + "?" + "access_token=" + accessToken;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
            //Reset to previously working status
            accessToken = accessTokenLast;
            centerLongitude = centerLongitudeLast;
            centerLatitude = centerLatitudeLast;
            mapStyle = mapStyleLast;
            mapResolution = mapResolutionLast;
            updateMap = true;
            ResetMap = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", ((DownloadHandlerTexture)www.downloadHandler).texture);
            //Update variables to keep track of changes in public variables by the user
            accessTokenLast = accessToken;
            centerLongitudeLast = centerLongitude;
            centerLatitudeLast = centerLatitude;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
            ResetMap = false;
        }
        mapIsLoading = false;
    }


    //Set the scale of plane to match the screen size
    private void MatchPlaneToScreenSize()
    {
        double planeHeightScale = 2.0 * Camera.main.orthographicSize / 10.0; //Orthographic Camera. Radians = (Math.PI / 180) * degrees. Default plane is 10 units in x and z
        double planeWidthScale = planeHeightScale * Camera.main.aspect;
        gameObject.transform.localScale = new Vector3((float)planeWidthScale, 1, (float)planeHeightScale);
        mapWidthMeter = planeWidthScale * 10.0; //Assuming each Unity unit is 1 m in real world. Default plane is 10 units in x and z
        mapHeightMeter = planeHeightScale * 10.0; //Assuming each Unity unit is 1 m in real world. Default plane is 10 units in x and z
        //Set map width and height in pixel based on view aspec ratio
        if (Camera.main.aspect > 1) //Width is bigger than height
        {
            mapWidthPx = 1280; //Mapbox width should be a number between 1 and 1280 pixels.
            mapHeightPx = (int)Math.Round(1280 / Camera.main.aspect); //Height is proportional to to view aspect ratio
        }
        else //Height is bigger than width
        {
            mapHeightPx = 1280; //Mapbox height should be a number between 1 and 1280 pixels.
            mapWidthPx = (int)Math.Round(1280 / Camera.main.aspect); //Width is proportional to to view aspect ratio
        }
    }


    //Return map bounding box [minLon, minLat, maxLon, maxLat] using center(lon, lat) in decimal degree, and map width and height 
    private double[] GetRecMinMaxLonLat(double centerLon, double centerLat, double width, double height)
    {
        double distance = Math.Sqrt(Math.Pow(height / 2.0, 2) + Math.Pow(width / 2.0, 2)); //Hypotenuse from two sides 
        double topRightBearing = Math.Atan((width / 2.0) / (height / 2.0)); //Bearing in radian
        double bottomLeftBearing = 3.14159f + topRightBearing; //bottomLeftBearing = 180 + topRightBearing. 180 degree = 3.14159 radian.
        double[] bottomLeft = GetPointLonLat(centerLon, centerLat, distance, bottomLeftBearing);
        double[] topRight = GetPointLonLat(centerLon, centerLat, distance, topRightBearing);
        return new double[] { bottomLeft[0], bottomLeft[1], topRight[0], topRight[1] };
    }


    //Based on on a Stackoverflow answer by David M https://stackoverflow.com/questions/7222382/get-lat-long-given-current-point-distance-and-bearing
    //Return a point(lon, lat) in decimal degree using start point (lon, lat) in decimal degree and distance from it in meter and bearing (Agnle measured clockwise from north) in radian
    private double[] GetPointLonLat(double startLonDegree, double startLatDegree, double distance, double bearingRadian)
    {
        double earthRadius = 6378100.000; //In meters
        double startLatRadians = startLatDegree * (Math.PI / 180); //Radians = Degree * (PI / 180)
        double startLonRadians = startLonDegree * (Math.PI / 180); //Radians = Degree * (PI / 180)
        double targetLatRadians = Math.Asin(Math.Sin(startLatRadians) * Math.Cos(distance / earthRadius) + Math.Cos(startLatRadians) * Math.Sin(distance / earthRadius) * Math.Cos(bearingRadian));
        double targetLonRadians = startLonRadians + Math.Atan2(Math.Sin(bearingRadian) * Math.Sin(distance / earthRadius) * Math.Cos(startLatRadians), Math.Cos(distance / earthRadius) - Math.Sin(startLatRadians) * Math.Sin(targetLatRadians));
        return new double[] { targetLonRadians * (180.0 / Math.PI), targetLatRadians * (180.0 / Math.PI) }; //Degree = (180 / Math.PI) * Radian
    }

}