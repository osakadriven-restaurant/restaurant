// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Utilities;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Profiling;
using System.Collections;

public class TestView : MonoBehaviour
{
    public enum eSeq
    {
        First,
        Second,
        Theird,
        Transition,
    }

    [SerializeField]
    public Camera m_camera = null;

    [SerializeField]
    public GameObject m_modelPrefab = null;

    [SerializeField]
    public GameObject m_humanPrefab = null;

    private Transform m_pageRoot = null;
    private Transform[] m_pageList = new Transform[4];
    private UnityEngine.UI.Button[] m_buttonLeftList = null;
    private UnityEngine.UI.Button[] m_buttonRightList = null;
    private Dictionary<int, UnityEngine.UI.Button[]> m_dicButtonList = new Dictionary<int, UnityEngine.UI.Button[]>();
    private Dictionary<int, bool> m_buttonFlagDic = new Dictionary<int, bool>();
    private UnityEngine.UI.Button m_nextButton = null;
    private UnityEngine.UI.Button m_prevButton = null;
    private bool[] m_buttonLeftFlagList = new bool[4];
    private bool m_nextButtonFlag = false;
    private bool m_prevButtonFlag = false;
    private eSeq m_seq = eSeq.First;
    private eSeq m_transitonToSeq = eSeq.First;
    private float m_transitionTimer = 0.0f;
    private eSeq m_page = 0;
    private eSeq m_pagePrev = 0;
    private bool m_test = false;
    private GameObject m_modelObject = null;
    public GameObject m_humanObject = null;
    private bool m_initFlag = false;

    private void Start()
    {
        m_pageRoot = this.transform.Find("Root");
        m_pageList[0] = this.transform.Find("Root/Page01");
        m_pageList[1] = this.transform.Find("Root/Page02");
        m_pageList[2] = this.transform.Find("Root/Page03");
        for (int i = 0; i < m_pageList.Length; ++i)
        {
            if (m_pageList[i])
            {
                m_pageList[i].gameObject.AddComponent<UnityEngine.CanvasGroup>().alpha = 0.0f;
            }
        }
        m_pageList[0].gameObject.GetComponent<UnityEngine.CanvasGroup>().alpha = 1.0f;
        m_buttonLeftList = m_pageList[0].Find("ScrollViewLeft").GetComponentsInChildren<UnityEngine.UI.Button>();
        m_buttonRightList = m_pageList[0].Find("ScrollViewRight").GetComponentsInChildren<UnityEngine.UI.Button>();
        m_nextButton = this.transform.Find("NextButton").GetComponent<UnityEngine.UI.Button>();
        m_prevButton = this.transform.Find("PrevButton").GetComponent<UnityEngine.UI.Button>();
        m_buttonLeftList[0].onClick.AddListener(OnPage1LeftClick00);
        m_buttonLeftList[1].onClick.AddListener(OnPage1LeftClick01);
        m_buttonLeftList[2].onClick.AddListener(OnPage1LeftClick02);
        m_buttonLeftList[3].onClick.AddListener(OnPage1LeftClick03);
        m_nextButton.onClick.AddListener(OnNextClick);
        m_prevButton.onClick.AddListener(OnPrevClick);
        for (int i = 0; i < m_buttonLeftFlagList.Length; ++i)
        {
            m_buttonLeftFlagList[i] = false;
        }
    }

    private void Update()
    {
        /*
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(m_camera.transform.forward, new Vector3(1, 1, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward + m_camera.transform.right * 0.0f;
        moveForward.y *= 1.0f;
        this.transform.position = moveForward;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
        */
        if (m_seq == eSeq.First)
        {
            if (m_test)
            {
                m_humanObject = GameObject.Instantiate(m_humanPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(Vector3.zero), this.transform.parent);
                m_humanObject.transform.localPosition = Vector3.zero;
                StartCoroutine(coAnimation(m_humanObject.GetComponent<Animator>(), new Vector3(1.0f, 0.0f, 1.0f).normalized, new Vector3(4.0f, 0.0f, 4.0f)));
            }
            if (m_nextButtonFlag)
            {
                NextPageProc();
            }
        }
        else if (m_seq == eSeq.Second)
        {
            if (m_nextButtonFlag)
            {
                NextPageProc();
            }
            if (m_prevButtonFlag)
            {
                PrevPageProc();
            }
        }
        else if (m_seq == eSeq.Theird)
        {
            if (m_initFlag)
            {
            }
            if (m_prevButtonFlag)
            {
                PrevPageProc();
            }
            m_initFlag = false;
        }
        else if (m_seq == eSeq.Transition)
        {
            float time = 0.25f;
            m_transitionTimer += Time.deltaTime;
            float rate = m_transitionTimer / time;
            if (m_transitionTimer > time)
            {
                rate = 1.0f;
                m_seq = m_transitonToSeq;
                m_transitionTimer = 0.0f;
                m_initFlag = true;
            }
            if (m_page > m_pagePrev)
            {
                m_pageList[(int)m_page].GetComponent<UnityEngine.CanvasGroup>().alpha = rate;
                m_pageList[(int)m_pagePrev].GetComponent<UnityEngine.CanvasGroup>().alpha = (1.0f - rate);
                m_pageRoot.localPosition = new Vector3(rate * -600.0f + (int)m_pagePrev * -600.0f, 0.0f, 0.0f);
            }
            else if (m_page < m_pagePrev)
            {
                m_pageList[(int)m_page].GetComponent<UnityEngine.CanvasGroup>().alpha = rate;
                m_pageList[(int)m_pagePrev].GetComponent<UnityEngine.CanvasGroup>().alpha = (1.0f - rate);
                m_pageRoot.localPosition = new Vector3((1.0f - rate) * -600.0f + (int)m_page * -600.0f, 0.0f, 0.0f);
            }
        }
        for (int i = 0; i < m_buttonLeftFlagList.Length; ++i)
        {
            m_buttonLeftFlagList[i] = false;
        }
        m_nextButtonFlag = false;
        m_prevButtonFlag = false;
        m_test = false;
    }

    IEnumerator coAnimation(Animator anim, Vector3 dir, Vector3 target)
    {
        yield return null;
        anim.SetBool("Next", true);
        yield return null;
        anim.SetBool("Next", false);
        while(true)
        {
            anim.transform.localPosition += dir * 0.001f;
            anim.transform.localRotation = Quaternion.LookRotation(dir);
            if (Vector3.Distance(anim.transform.localPosition, target) < 1.0f)
            {
                break;
            }
            yield return null;
        }
        anim.SetBool("Back", true);
        if (!m_modelObject)
        {
            m_modelObject = GameObject.Instantiate(m_modelPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(Vector3.zero), this.transform.parent);
            m_modelObject.transform.localPosition = anim.transform.localPosition + new Vector3(0.5f, 0.0f, 0.0f);
        }
    }

    private void NextPageProc()
    {
        m_transitonToSeq = m_seq + 1;
        m_seq = eSeq.Transition;
        m_pagePrev = m_transitonToSeq - 1;
        m_page = m_transitonToSeq;
    }

    private void PrevPageProc()
    {
        m_transitonToSeq = m_seq - 1;
        m_seq = eSeq.Transition;
        m_pagePrev = m_transitonToSeq + 1;
        m_page = m_transitonToSeq;
    }

    public void OnPage1LeftClick00()
    {
        m_test = true;
    }

    public void OnPage1LeftClick01()
    {

    }

    public void OnPage1LeftClick02()
    {

    }

    public void OnPage1LeftClick03()
    {

    }

    public void OnNextClick()
    {
        m_nextButtonFlag = true;
    }

    public void OnPrevClick()
    {
        m_prevButtonFlag = true;
    }
}
