using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

[Serializable]
public class Club
{
    public string name;
    public string desc_short;
    public string desc_long;
    public string tag;
    public int num_members;
    public float x;
    public float y;
    public float z;
    public string img_url;
}

public class Harry_ClubWorldManager : MonoBehaviour
{
    public GameObject clubFac;

    string innerCircle;
    string outerCircle;

    Dictionary<string, Club> innerClubs = new Dictionary<string, Club>();
    Dictionary<string, Club> outerClubs = new Dictionary<string, Club>();
    // Start is called before the first frame update
    void Start()
    {
        // Dictionary 형태의 Json 파일을 읽어와 역직렬화
        innerCircle = File.ReadAllText(Application.dataPath + "/Harry/inner_circle.json");
        outerCircle = File.ReadAllText(Application.dataPath + "/Harry/outer_circle.json");
        innerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(innerCircle);
        outerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(outerCircle);

        // Dictionary의 정보를 기반으로 클럽 생성
        foreach (var item in innerClubs)
        {
            SetClubs(item.Value, true);
        }
        foreach (var item in outerClubs)
        {
            SetClubs(item.Value, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetClubs(Club clubInfo, bool isInner)
    {
        GameObject club = Instantiate(clubFac);
        club.transform.position = Vector3.zero;
        club.GetComponent<Harry_ClubInfo>().clubInfo = clubInfo;

        // inner 서클이면 크기가 다르게
        if (isInner)
        {
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", 1.5f, "delay", 1f, "easetype", iTween.EaseType.easeOutCirc));
            club.transform.localScale = 10 * (float)clubInfo.num_members / 30 * Vector3.one;
        }
        // outer 서클이면 크기를 같게
        else
        {
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", 1.5f, "delay", 1f, "easetype", iTween.EaseType.easeOutCirc));
            club.transform.localScale = 10 * Vector3.one;
        }
    }
}
