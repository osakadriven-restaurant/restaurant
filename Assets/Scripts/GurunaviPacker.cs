using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ぐるなびAPIから取得したデータを保存しておくクラス
/// 使い方は
/// </summary>
public class GurunaviPacker
{

    /// <summary>
    /// 店のぐるなび内でのID
    /// </summary>
    public string ID { get; private set; }

    /// <summary>
    /// お店の名前
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 店の緯度
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// 店の経度
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// そのお店のぐるなびでのホームページ
    /// </summary>
    public string URL { get; private set; }

    public GurunaviPacker(string doc)
    {
        foreach (var line in doc.Split('\n'))
        {
            if (line.Contains("id"))
            {
                string a1 = line.Replace(",", "");
                string a2 = a1.Replace("\"id\": ", "");
                string a3 = a2.Replace("\"", "");
                ID = a3;
            }
            if (line.Contains("name") && !line.Contains("kana"))
            {

                string a1 = line.Replace(",", "");
                string a2 = a1.Replace("\"name\": ", "");
                string a3 = a2.Replace("\"", "");

                Name = a3;
            }
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
            if (line.Contains("url"))
            {

                string a1 = line.Replace(",", "");
                string a2 = a1.Replace("\"url\": ", "");
                string a3 = a2.Replace("\"", "");

                URL = a3;
            }
        }
    }
}
