using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Harry_SquareManager : MonoBehaviour
{
    #region ��������Ʈ ��ɵ�
    void MyRoom()
    {
        StartCoroutine(ChangeToOwl("Harry_MainLobbyDesign"));
    }
    void Club()
    {
        StartCoroutine(ChangeToOwl("Harry_ClubWorld"));
    }
    #endregion
    // ����Ʈ����Ʈ�� �˻��� ����� �̸��� �Լ��� ��� ��ųʸ�
    private Dictionary<string, UnityAction> functions = new Dictionary<string, UnityAction>();
    public GameObject funcFac;

    public static Harry_SquareManager Instance;

    public InputField chatInput;
    InputField spotLight;
    Button teleport;
    Button emotion;
    Button schedule;
    Transform content;

    GameObject Callender;

    public GameObject player;
    public GameObject effectFac;
    public GameObject owlFac;

    bool canMove = true;
    public bool CanMove
    {
        get { return canMove; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }

        spotLight = GameObject.Find("SpotLight").GetComponent<InputField>();
        spotLight.onValueChanged.AddListener(SpotLight);
        spotLight.gameObject.SetActive(false);
        content = spotLight.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content");

        teleport = GameObject.Find("Teleport").GetComponent<Button>();
        teleport.onClick.AddListener(OnClickTeleport); 
        foreach (Transform tr in teleport.transform) { tr.gameObject.SetActive(false); }
        teleport.transform.Find("Club").GetComponent<Button>().onClick.AddListener(Club);
        teleport.transform.Find("MyRoom").GetComponent<Button>().onClick.AddListener(MyRoom);

        emotion = GameObject.Find("Emotion").GetComponent<Button>();
        emotion.transform.Find("Motions").gameObject.SetActive(false);
        emotion.onClick.AddListener(OnClickEmotion);

        schedule = GameObject.Find("Schedule").GetComponent<Button>(); 
        schedule.onClick.AddListener(OnClickSchedule);  

        // ��ųʸ��� �Լ� �̸��� �Լ� ����� �߰�
        functions.Add("���̷� �̵��ϱ�", MyRoom);
        functions.Add("Ŭ�� �̵��ϱ�", Club);

        Callender = GameObject.Find("CalCanvas");
        Callender.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Return))
        {
            spotLight.gameObject.SetActive(!spotLight.gameObject.activeSelf);
        }
    }

    void SpotLight(string s)
    {
        // ���� ����� ��ư�� ������ �߰��Ǵ°��� ���� ���� �Ź����� ��ư �� ����
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
        foreach (var func in functions)
        {
            // ���� �˻��� Ű���带 �����ϴ� �Լ� �̸��� �ִٸ�
            if (func.Key.Contains(s))
            {
                // �Լ��� ��ɰ� �̸��� ��Ƽ� ��ư���� ����
                GameObject go = Instantiate(funcFac, content);
                go.GetComponent<Button>().onClick.AddListener(func.Value);
                go.transform.Find("Text").GetComponent<Text>().text = func.Key.ToString();
            }
        }
    }

    void OnClickTeleport()
    {
        foreach (Transform tr in teleport.transform)
        {
            tr.gameObject.SetActive(!tr.gameObject.activeSelf);
        }
    }

    void OnClickEmotion()
    {
        emotion.transform.Find("Motions").gameObject.SetActive(!emotion.transform.Find("Motions").gameObject.activeSelf);
    }

    public void OnClickMotions(string s)
    {
        if (player)
        {
            player.GetComponent<Harry_AvatarController>().EMOTE(s);
        }
    }

    void OnClickSchedule()
    {
        Tablet();
        Callender.SetActive(!Callender.activeSelf);
    }

    public bool isTablet = false;
    void Tablet()
    {
        if (isTablet)
        {
            isTablet = false;
            canMove = true;
        }
        else
        {
            isTablet = true;
            canMove = false;
        }
    }

    IEnumerator ChangeToOwl(string sceneName)
    {
        foreach (Transform tr in player.transform)
        {
            tr.gameObject.SetActive(false);
        }
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Harry_AvatarController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;
        GameObject effect = Instantiate(effectFac);
        effect.transform.position = player.transform.position;

        GameObject owl = Instantiate(owlFac, player.transform);
        owl.GetComponent<Animator>().SetFloat("Status", 2);

        CharacterController cc = owl.GetComponent<CharacterController>();
        float currentTime = 0;
        while (currentTime < 3f)
        {
            cc.Move(Vector3.up * Time.deltaTime * 0.5f);
            yield return null;
            currentTime += Time.deltaTime;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }
}
