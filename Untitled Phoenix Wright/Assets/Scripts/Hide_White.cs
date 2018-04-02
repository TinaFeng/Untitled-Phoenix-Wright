﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hide_White : MonoBehaviour {

    RawImage image;
	// Use this for initialization
	void Start () {

        image = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {

        if (image.texture==null)
        {
            image.texture = (Texture)Resources.Load("null");
        }
	}
}
