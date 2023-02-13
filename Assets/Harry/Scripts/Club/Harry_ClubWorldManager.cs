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
    [SerializeField]
    GameObject club1Fac;
    [SerializeField]
    GameObject club2Fac;
    [SerializeField]
    GameObject club3Fac;
    [SerializeField]
    GameObject club4Fac;
    [SerializeField]
    GameObject club5Fac;

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
        // Dictionary ������ Json ������ �о�� ������ȭ
        innerCircle = File.ReadAllText(Application.dataPath + "/JsonFile/inner_circle.json");
        outerCircle = File.ReadAllText(Application.dataPath + "/JsonFile/outer_circle.json");
        innerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(innerCircle);
        outerClubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(outerCircle);

        // Dictionary�� ������ ������� Ŭ�� ����
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
        GameObject club = null;
        int ran = UnityEngine.Random.Range(0, 5);
        switch(ran)
        {
            case 0:
                club = Instantiate(club1Fac);
                break;
            case 1:
                club = Instantiate(club2Fac);
                break;
            case 2:
                club = Instantiate(club3Fac);
                break;
            case 3:
                club = Instantiate(club4Fac);
                break;
            case 4:
                club = Instantiate(club5Fac);
                break;
        }

        club.transform.position = new Vector3(clubInfo.x * 5, clubInfo.y * 5, clubInfo.z * 5);
        club.GetComponent<Harry_ClubInfo>().clubInfo = clubInfo;

        while (player == null)
        {
            yield return null;
        }

        // inner ��Ŭ�̸� ũ�Ⱑ �ٸ���
        if (isInner)
        {
            Vector3 scale = 10 * (float)clubInfo.num_members / 30 * Vector3.one;
            club.transform.localScale = Vector3.zero;
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", actTime, "delay", delayTime, "easetype", iTween.EaseType.easeOutCirc));
            iTween.ScaleTo(club, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 1.5f, "delay", 1.5f, "easetype", iTween.EaseType.easeOutCirc));
        }
        // outer ��Ŭ�̸� ũ�⸦ ����
        else
        {
            Vector3 scale = 10 * Vector3.one;
            club.transform.localScale = Vector3.zero;
            iTween.MoveTo(club, iTween.Hash("x", clubInfo.x * 50, "y", clubInfo.y * 50, "z", clubInfo.z * 50, "time", actTime, "delay", delayTime, "easetype", iTween.EaseType.easeOutCirc));
            iTween.ScaleTo(club, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 1.5f, "delay", 1.5f, "easetype", iTween.EaseType.easeOutCirc));
        }
    }
}
