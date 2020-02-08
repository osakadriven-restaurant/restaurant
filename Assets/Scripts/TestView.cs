// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Utilities;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

public class TestView : MonoBehaviour
{
    [SerializeField]
    public Camera m_camera = null;

    private void Update()
    {
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
    }
}
