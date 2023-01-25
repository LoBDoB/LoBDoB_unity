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
        // Dictionary ������ Json ������ �о�� ������ȭ
        innerCircle = File.ReadAllText(Application.dataPath + "/Harry/inner_circle.json");
        outerCircle = File.ReadAllText(Application.dataPath + "/Harry/outer_circle.json");
        innerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(innerCircle);
        outerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(outerCircle);

        // Dictionary�� ������ ������� Ŭ�� ����
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

        // inner ��Ŭ�̸� ũ�Ⱑ �ٸ���
        if (isInner)
        {
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", 1.5f, "delay", 1f, "easetype", iTween.EaseType.easeOutCirc));
            club.transform.localScale = 10 * (float)clubInfo.num_members / 30 * Vector3.one;
        }
        // outer ��Ŭ�̸� ũ�⸦ ����
        else
        {
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", 1.5f, "delay", 1f, "easetype", iTween.EaseType.easeOutCirc));
            club.transform.localScale = 10 * Vector3.one;
        }
    }
}
