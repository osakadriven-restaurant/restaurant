using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// 使い方
/// 適当なオブジェクトにアタッチする
/// 適当なタイミングでGetDestinationByIdを呼ぶ
/// 適当な関数をOnDestinationGotに登録しておくと、座標取得時点で呼ばれる
/// </summary>
public class DestinationSearcher : MonoBehaviour
{

    public UnityEvent OnDestinationGot;
    public static Vector3 Destination;
    public void GetDestinationById(string id)
    {
        StartCoroutine(GETRequest($"https://api.gnavi.co.jp/RestSearchAPI/v3/?keyid=8446b8f3a55150243fe036fa0fa7b8d3&id={id}"));
    }


    IEnumerator GETRequest(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        string response = request.downloadHandler.text;
        Destination = GetPosition(response);
        OnDestinationGot.Invoke();
    }

    static Vector3 GetPosition(string response)
    {
        (double Latitude, double Longitude) = (0,0);
        foreach (var line in response.Split('\n'))
        {
            if (line.Contains("latitude"))
            {

                string a1 = line.Replace(",", "");
                string a2 = a1.Replace("\"latitude\": ", "");
                string a3 = a2.Replace("\"", "");

                Latitude = double.Parse(a3);
            }
            if (line.Contains("longitude"))
            {

                string a1 = line.Replace(",", "");
                string a2 = a1.Replace("\"longitude\": ", "");
                string a3 = a2.Replace("\"", "");

                Longitude = double.Parse(a3);
            }
        }
        // 緯度経度からメートルに変換。1度=110942.97m
        return new Vector3((float)Longitude, 0, (float)Latitude) * 110942.97f;
    }
}
