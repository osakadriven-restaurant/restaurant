using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    LineRenderer clsLineRenderer;

    public GameObject hololensCamera;
    public TestView clsTestView;
    void Start()
    {
        this.clsLineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clsTestView.m_humanObject != null)
        {

            this.clsLineRenderer.enabled = true;

            this.clsLineRenderer.startWidth = 0.1f;
            this.clsLineRenderer.endWidth = 0.1f;

            this.clsLineRenderer.positionCount = 2;
            // 開始
            clsLineRenderer.SetPosition(0, hololensCamera.transform.position);
            // 終了（Unityちゃん）
            clsLineRenderer.SetPosition(1, clsTestView.m_humanObject.transform.position + new Vector3(0.0f, 0.2f, -0.6f));

        }
    }
}
