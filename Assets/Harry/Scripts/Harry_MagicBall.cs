using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEditor.Search;

public class Harry_MagicBall : MonoBehaviour
{
    // �� ���������� �� �� �ִ� Ŭ���� �ִ� ��
    const int maxCount = 10;
    int maxIdx;

    public GameObject clubFac;
    public GameObject pressF;
    public GameObject ballUI;

    Button leftBtn;
    Button rightBtn;

    Harry_CamController cc;
    bool canInter;
    bool isInter;
    string circle;

    Quaternion ballRot;
    Vector3 ballPos;

    // �˻��Ͽ� ���� ������� �����ϴ� ����Ʈ
    [SerializeField]
    List<GameObject> searchList = new List<GameObject>();

    // �������� �ε���
    int idx = 0;
    int Idx
    {
        get { return idx; }
        set
        {
            // �������� �ִ� �� ����
            maxIdx = Mathf.CeilToInt((float)searchList.Count / maxCount);

            if (value < 0)
            {
                idx = value + maxIdx;
            }
            else if (value > maxIdx - 1)
            {
                idx = value - maxIdx;
            }
            else
            {
                idx = value;
            }

            // ��� Ŭ�� ��Ȱ��ȭ
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(false);
            }
            // �������� �ε����� ���� Ŭ���� ������
            for (int i = idx * maxCount; i <= (idx + 1) * maxCount - 1; i++)
            {
                if (i < searchList.Count)
                {
                    searchList[i].SetActive(true);
                }
            }

            ballUI.transform.Find("Index").GetComponent<Text>().text = idx.ToString();
        }
    }

    Dictionary<string, Club> clubs = new Dictionary<string, Club>();
    // Start is called before the first frame update
    void Start()
    {
        ballRot = transform.rotation;
        ballPos = transform.position;

        ballUI.transform.Find("Search").GetComponent<InputField>().onSubmit.AddListener(SearchClub);
        leftBtn = ballUI.transform.Find("LeftArrow").GetComponent<Button>();
        rightBtn = ballUI.transform.Find("RightArrow").GetComponent<Button>();

        pressF.SetActive(false);
        ballUI.SetActive(false);

        // Dictionary ������ Json ������ �о�� ������ȭ
        circle = File.ReadAllText(Application.dataPath + "/Harry/magic_ball.json");
        clubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(circle);

        // Dictionary�� ������ ������� Ŭ�� ����
        foreach (var item in clubs)
        {
            SetClubs(item.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInter)
        {
            pressF.SetActive(true);
            // F��ư�� Ȱ��ȭ �Ǿ����� �� F��ư�� ������ ���� ����
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCustomize();
            }
        }
        else
        {
            pressF.SetActive(false);
        }

        if (isInter)
        {
            // ��ȣ�ۿ� ���̶�� Q, E ��ư�� ���� ���� ȸ��
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, -60f * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, 60f * Time.deltaTime, 0);
            }
            // ESC ��ư�� ���� ��ȣ�ۿ� ����
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCustomize();
            }
        }
    }

    void SetClubs(Club clubInfo)
    {
        GameObject club = Instantiate(clubFac, transform);
        club.transform.localPosition = new Vector3(clubInfo.x, clubInfo.y, clubInfo.z) / 2.5f;
        club.GetComponent<Harry_MinClubInfo>().clubInfo = clubInfo;
        // �ʱ⿡ ��� Ŭ���� �˻� ����Ʈ�� �߰���
        searchList.Add(club);
    }

    void StartCustomize()
    {
        // �������� �ε����� 0���� �ʱ�ȭ
        Idx = 0;
        // ��ȣ�ۿ� ����
        isInter = true;
        // Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        canInter = false;
        // ī�޶� ���� �������� �̵�
        cc.StartInter(new Vector3(0, 0.93f, 1.531f), new Vector3(0, 0, 0));
        // �÷��̾ �������̰�
        Harry_GameManager.Instance.Player_CanMove = false;

        ballUI.SetActive(true);
    }

    // ���� ���� ����
    public void EndCustomize()
    {
        // ��ȣ�ۿ� ����
        isInter = false;
        // Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
        canInter = true;
        // ���� �ȿ� ���� ���� ī�޶� ��ġ, ������ �̵�
        cc.EndInter();
        //�÷��̾ ������ �� �ְ�
        Harry_GameManager.Instance.Player_CanMove = true;

        // Q, E ��ư���� ȸ���� ������ ������ ���󺹱���
        transform.rotation = ballRot;
        // ������ ��ġ ���� ���󺹱�
        transform.position = ballPos;

        ballUI.SetActive(false);

        // ��Ȱ��ȭ �Ǿ��ִ� Ŭ������ ��� Ȱ��ȭ
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(true);
        }
    }

    void SearchClub(string s)
    {
        // ������ ��ġ ����Ʈ�� �ʱ�ȭ
        searchList.Clear();

        foreach (Transform item in transform)
        {
            // ��� Ŭ���� ��ȸ�Ͽ� �±׷� �˻�
            if (item.GetComponent<Harry_MinClubInfo>().clubInfo.tag.Contains(s))
            {
                // �˻� Ű���带 Ŭ���� �±װ� �����ϰ� �ִٸ� ��ġ ����Ʈ�� Ŭ�� �߰�
                searchList.Add(item.gameObject);
            }
        }

        // ���� ��ġ ����Ʈ�� Ŭ�� ���� �� ������ �ִ� Ŭ�� ������ ���ٸ� ������ �ѱ�� ��ư ��Ȱ��ȭ
        if (searchList.Count <= maxCount) 
        {
            leftBtn.interactable = false; rightBtn.interactable = false;
        }
        else
        {
            leftBtn.interactable = true; rightBtn.interactable = true;
        }

        // �������� �ε����� 0���� �ʱ�ȭ
        Idx = 0;
        ballUI.transform.Find("Search").GetComponent<InputField>().text = "";
    }

    public void LeftClick()
    {
        Idx--;
    }

    public void RightClick()
    {
        Idx++;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �ȿ� ������ Press F ��ư Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            this.cc = cc;
            canInter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������ ������ Press F ��ư ��Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            canInter = false;
        }
    }
}