﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    double BaseLatitude = 2;
    double BaseLongitude = 1;

    double DestinationLatitude = 20.77f;
    double DestinationLongitude = 11.26f;

    public GameObject hololensCamera;

    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetDestination(double Latitude, double Longitude)
    {
        DestinationLatitude = Latitude;
        DestinationLongitude = Longitude;
    }

    public void SetHumanPosition(double Latitude, double Longitude)
    {
        BaseLatitude = Latitude;
        BaseLongitude = Longitude;
    }

    private void Update()
    {
        SetHumanPosition(hololensCamera.transform.position.z, hololensCamera.transform.position.x);
        var direc = new Vector2((float)(DestinationLongitude - BaseLongitude), (float)(DestinationLatitude - BaseLatitude));
        float theta = Mathf.Acos(direc.x / direc.magnitude);
        rect.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * theta);
        if(direc.y < 0)
        {
            rect.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * theta);
        }
    }
}
