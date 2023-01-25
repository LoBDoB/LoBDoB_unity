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
    // 한 페이지에서 볼 수 있는 클럽의 최대 수
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

    // 검색하여 나온 결과들을 저장하는 리스트
    [SerializeField]
    List<GameObject> searchList = new List<GameObject>();

    // 페이지의 인덱스
    int idx = 0;
    int Idx
    {
        get { return idx; }
        set
        {
            // 페이지의 최대 수 지정
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

            // 모든 클럽 비활성화
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(false);
            }
            // 페이지의 인덱스에 맞춰 클럽을 보여줌
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

        // Dictionary 형태의 Json 파일을 읽어와 역직렬화
        circle = File.ReadAllText(Application.dataPath + "/Harry/magic_ball.json");
        clubs = JsonConvert.DeserializeObject<Dictionary<string, Club>>(circle);

        // Dictionary의 정보를 기반으로 클럽 생성
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
            // F버튼이 활성화 되어있을 때 F버튼을 누르면 보기 시작
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
            // 상호작용 중이라면 Q, E 버튼을 통해 구슬 회전
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, -60f * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, 60f * Time.deltaTime, 0);
            }
            // ESC 버튼을 통해 상호작용 종료
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
        // 초기에 모든 클럽을 검색 리스트에 추가함
        searchList.Add(club);
    }

    void StartCustomize()
    {
        // 페이지의 인덱스를 0으로 초기화
        Idx = 0;
        // 상호작용 시작
        isInter = true;
        // 커서 활성화
        Cursor.visible = true;
        canInter = false;
        // 카메라를 구슬 정면으로 이동
        cc.StartInter(new Vector3(0, 0.93f, 1.531f), new Vector3(0, 0, 0));
        // 플레이어가 못움직이게
        Harry_GameManager.Instance.Player_CanMove = false;

        ballUI.SetActive(true);
    }

    // 구슬 보기 종료
    public void EndCustomize()
    {
        // 상호작용 종료
        isInter = false;
        // 커서 비활성화
        Cursor.visible = false;
        canInter = true;
        // 구역 안에 들어올 때의 카메라 위치, 각도로 이동
        cc.EndInter();
        //플레이어가 움직일 수 있게
        Harry_GameManager.Instance.Player_CanMove = true;

        // Q, E 버튼으로 회전한 구슬의 각도를 원상복구함
        transform.rotation = ballRot;
        // 구슬의 위치 또한 원상복구
        transform.position = ballPos;

        ballUI.SetActive(false);

        // 비활성화 되어있던 클럽들을 모두 활성화
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(true);
        }
    }

    void SearchClub(string s)
    {
        // 기존의 서치 리스트를 초기화
        searchList.Clear();

        foreach (Transform item in transform)
        {
            // 모든 클럽을 순회하여 태그로 검색
            if (item.GetComponent<Harry_MinClubInfo>().clubInfo.tag.Contains(s))
            {
                // 검색 키워드를 클럽의 태그가 포함하고 있다면 서치 리스트에 클럽 추가
                searchList.Add(item.gameObject);
            }
        }

        // 만약 서치 리스트의 클럽 수가 한 페이지 최대 클럽 수보다 적다면 페이지 넘기는 버튼 비활성화
        if (searchList.Count <= maxCount) 
        {
            leftBtn.interactable = false; rightBtn.interactable = false;
        }
        else
        {
            leftBtn.interactable = true; rightBtn.interactable = true;
        }

        // 페이지의 인덱스를 0으로 초기화
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
        // 구역 안에 들어오면 Press F 버튼 활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            this.cc = cc;
            canInter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 구역을 나가면 Press F 버튼 비활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            canInter = false;
        }
    }
}