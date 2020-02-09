using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// ぐるなびAPIを呼び出すためのクラス
/// 簡単な使い方
/// 適当なオブジェクトにアタッチする
/// 適当なタイミングでQueryを呼ぶ
/// 適当な関数をOnShopGotに登録しておくと、座標取得時点で呼ばれる
/// </summary>
public class ShopSearch : MonoBehaviour
{
    /// <summary>
    /// 取得してきた各種データを保存しておく。
    /// Requestを呼ぶ前にアクセスしてはいけない
    /// </summary>
    public static List<GurunaviPacker> Gurunavis { get; set; }
    public UnityEvent OnShopGot;

    public void Query(double latitude, double longitude)
    {
        StartCoroutine(GETRequest($"https://api.gnavi.co.jp/RestSearchAPI/v3/?keyid=8446b8f3a55150243fe036fa0fa7b8d3&latitude={latitude}&longitude={longitude}"));
    }
    public void Query(double latitude, double longitude, string keyword, bool far)
    {
        int range = 2;
        int offset = 1;
        if (far)
        {
            range = 3;
            offset = 3;
        }
        string free = "";
        if (keyword == "和食")
        {
            free = "%E5%92%8C%E9%A3%9F";
        }
        if (keyword == "フレンチ")
        {
            free = "%E3%83%95%E3%83%AC%E3%83%B3%E3%83%81";
        }
        if (keyword == "居酒屋")
        {
            free = "%E5%B1%85%E9%85%92%E5%B1%8B";
        }
        StartCoroutine(GETRequest($"https://api.gnavi.co.jp/RestSearchAPI/v3/?keyid=8446b8f3a55150243fe036fa0fa7b8d3&latitude={latitude}&longitude={longitude}&freeword={free}&range={range}&offset_page={offset}"));
    }


    public Vector2 GetStorePosition(string id)
    {

        foreach (var x in Gurunavis)
        {
            if (x.ID == id)
            {
                return new Vector2((float)x.Longitude, (float)x.Latitude);
            }
        }
        return Vector2.zero;
    }

    IEnumerator GETRequest(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        string response = request.downloadHandler.text;
        Gurunavis = PurseResponse(response);

        OnShopGot.Invoke();
    }

    static List<GurunaviPacker> PurseResponse(string response)
    {
        int get_count = 0;
        int index = -1;
        List<string> stores = new List<string>();
        foreach (var line in response.Split('\n'))
        {
            if (line.Contains("\"id\""))
            {
                get_count = 9;
                index++;
                stores.Add("");
            }
            if (get_count > 0)
            {
                get_count--;
                stores[index] = stores[index] + line.Trim() + "\n";
            }
        }
        List<GurunaviPacker> gurus = new List<GurunaviPacker>();
        foreach (var x in stores)
        {
            gurus.Add(new GurunaviPacker(x));
        }

        return gurus;
    }
}
