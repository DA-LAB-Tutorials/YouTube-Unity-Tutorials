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

public class DetectTransformChange : MonoBehaviour
{
    public GameObject trackedObject;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = trackedObject.transform.position;
        lastRotation = trackedObject.transform.rotation;
        lastScale = trackedObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        OnTransformChanged();
    }

    void OnTransformChanged()
    {
        if (trackedObject.transform.position != lastPosition || trackedObject.transform.rotation != lastRotation || trackedObject.transform.localScale != lastScale)
        {
            Debug.Log("Transform has changed!");
            // Perform some action in response to the change
        }

        lastPosition = trackedObject.transform.position;
        lastRotation = trackedObject.transform.rotation;
        lastScale = trackedObject.transform.localScale;
    }
}
