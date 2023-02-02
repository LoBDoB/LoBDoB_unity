using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Photon.Pun;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

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

    public float actTime = 1.5f;
    public float delayTime = 1.5f;

    string innerCircle;
    string outerCircle;

    Dictionary<string, Club> innerClubs = new Dictionary<string, Club>();
    Dictionary<string, Club> outerClubs = new Dictionary<string, Club>();

    GameObject player;
    Harry_CamController cc;
    // Start is called before the first frame update
    void Start()
    {
        // Dictionary 형태의 Json 파일을 읽어와 역직렬화
        innerCircle = File.ReadAllText(Application.dataPath + "/JsonFile/inner_circle.json");
        outerCircle = File.ReadAllText(Application.dataPath + "/JsonFile/outer_circle.json");
        innerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(innerCircle);
        outerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(outerCircle);

        // Dictionary의 정보를 기반으로 클럽 생성
        foreach (var item in innerClubs)
        {
            StartCoroutine(SetClubCo(item.Value, true));
        }
        foreach (var item in outerClubs)
        {
            StartCoroutine(SetClubCo(item.Value, false));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find(PhotonNetwork.NickName) != null && player == null)
        {
            player = GameObject.Find(PhotonNetwork.NickName);
            cc = Camera.main.transform.parent.GetComponent<Harry_CamController>();
            StartCoroutine("MoveCam");
        }
    }

    IEnumerator MoveCam()
    {
        Harry_GameManager.Instance.Player_CanMove = false;
        cc.StartInter(new Vector3(0, 90, -300), new Vector3(15, 0, 0));
        yield return new WaitForSeconds(actTime + delayTime + 0.5f);
        Harry_GameManager.Instance.Player_CanMove = true;
        cc.EndInter();
    }

    IEnumerator SetClubCo(Club clubInfo, bool isInner)
    {
        GameObject club = Instantiate(clubFac);

        club.transform.position = new Vector3(clubInfo.x * 5, clubInfo.y * 5, clubInfo.z * 5);
        club.GetComponent<Harry_ClubInfo>().clubInfo = clubInfo;

        while (player == null)
        {
            yield return null;
        }

        // inner 서클이면 크기가 다르게
        if (isInner)
        {
            Vector3 scale = 10 * (float)clubInfo.num_members / 30 * Vector3.one;
            club.transform.localScale = Vector3.zero;
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", actTime, "delay", delayTime, "easetype", iTween.EaseType.easeOutCirc));
            iTween.ScaleTo(club, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 1.5f, "delay", 1.5f, "easetype", iTween.EaseType.easeOutCirc));
        }
        // outer 서클이면 크기를 같게
        else
        {
            Vector3 scale = 10 * Vector3.one;
            club.transform.localScale = Vector3.zero;
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", actTime, "delay", delayTime, "easetype", iTween.EaseType.easeOutCirc));
            iTween.ScaleTo(club, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 1.5f, "delay", 1.5f, "easetype", iTween.EaseType.easeOutCirc));
        }
    }
}
