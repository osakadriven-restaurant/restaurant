using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    double BaseLatitude = 2;
    double BaseLongitude = 1;

    double DestinationLatitude = -30;
    double DestinationLongitude = 10;

    public GameObject hololensCamera;

    public TestView clsTestView;

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

        if (clsTestView.m_humanObject != null)
        {
            SetDestination(clsTestView.m_humanObject.transform.position.z, clsTestView.m_humanObject.transform.position.x);
        }

        var direc = new Vector2((float)(DestinationLongitude - BaseLongitude), (float)(DestinationLatitude - BaseLatitude));
        float theta = Mathf.Acos(direc.x / direc.magnitude);
        rect.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * theta);
        if(direc.y < 0)
        {
            rect.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * theta);
        }
    }
}
