using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Harry_Callender : MonoBehaviour
{
    public List<GameObject> dates = new List<GameObject>();
    public GameObject label;
    public Dropdown pallete;
    public Toggle college;
    public Toggle club;
    public Toggle personal;

    List<GameObject> colleges = new List<GameObject>();
    List<GameObject> clubs = new List<GameObject>();
    List<GameObject> personals = new List<GameObject>();

    int week = 0;
    public Text weekIdx;
    public Button prevWeek;
    public Button nextWeek;

    int startIdx = -1;
    public int StartIdx
    {
        get { return startIdx; }
        set
        {
            if (startIdx != value)
            {
                startIdx = value;
            }
        }
    }

    int endIdx = -1;
    public int EndIdx
    {
        get { return endIdx; }
        set
        {
            if (endIdx != value && startIdx != -1)
            {
                endIdx = value;

                int maxCnt = 0;
                for (int i = startIdx; i <= endIdx; i++)
                {
                    if (dates[i].GetComponent<Harry_Date>().labelCnt >= maxCnt)
                        maxCnt = dates[i].GetComponent<Harry_Date>().labelCnt;
                }

                for (int i = startIdx; i <= endIdx; i++)
                {
                    GameObject go = Instantiate(label, dates[i].transform);

                    switch (pallete.value)
                    {
                        case 0:
                            go.GetComponent<Image>().color = Color.red;
                            colleges.Add(go);
                            break;
                        case 1:
                            go.GetComponent<Image>().color = Color.blue;
                            clubs.Add(go);
                            break;
                        case 2:
                            go.GetComponent<Image>().color = Color.green;
                            personals.Add(go);
                            break;
                    }

                    if (i != startIdx)
                        go.transform.GetChild(0).gameObject.SetActive(false);

                    dates[i].GetComponent<Harry_Date>().labelCnt = (maxCnt + 1);
                    go.transform.localPosition = new Vector3(0, 30 - maxCnt * 25);
                }

                if (endIdx >= 7)
                {
                    for (int i = 7; i < dates.Count; i++)
                    {
                        Color col = dates[i].gameObject.GetComponent<Image>().color;
                        col.a = 1;
                        dates[i].gameObject.GetComponent<Image>().color = col;
                    }
                }

                endIdx = -1;
                startIdx = -1;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform tr in transform)
        {
            dates.Add(tr.gameObject);
        }
        for (int i = 0; i < dates.Count; i++)
        {
            dates[i].transform.Find("Text").GetComponent<Text>().text = (i + 1).ToString();
        }

        prevWeek.onClick.AddListener(OnClickPrev);
        nextWeek.onClick.AddListener(OnClickNext);

        college.onValueChanged.AddListener(OnClickCollege);
        club.onValueChanged.AddListener(OnClickClub);
        personal.onValueChanged.AddListener(OnClickPersonal);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GraphicRaycaster ray = GameObject.Find("All_Canvas").GetComponent<GraphicRaycaster>();
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            ray.Raycast(ped, results);

            // 이벤트 처리부분
            for (int i = 0; i < dates.Count; i++)
            {
                try
                {
                    if (dates[i] == results[0].gameObject)
                    {
                        EndIdx = i;
                    }
                }
                catch(ArgumentException ex)
                {
                    startIdx = -1;
                }
            }
        }
    }

    void OnClickPrev()
    {
        week--;
        SetWeek();
    }

    void OnClickNext()
    {
        week++;
        SetWeek();
    }

    void OnClickCollege(bool value)
    {
        foreach (GameObject go in colleges)
        {
            go.SetActive(value);
        }
    }

    void OnClickClub(bool value)
    {
        foreach (GameObject go in clubs)
        {
            go.SetActive(value);
        }
    }

    void OnClickPersonal(bool value)
    {
        foreach (GameObject go in personals)
        {
            go.SetActive(value);
        }
    }

    void SetWeek()
    {
        week = Mathf.Clamp(week, 0, 3);
        for (int i = 0; i < dates.Count; i++)
        {
            dates[i].transform.Find("Text").GetComponent<Text>().text = (i + 1 + week * 7).ToString();
        }

        switch (week)
        {
            case 0:
                weekIdx.text = "2023년 2월 첫번째";
                break;
            case 1:
                weekIdx.text = "2023년 2월 두번째";
                break;
            case 2:
                weekIdx.text = "2023년 2월 세번째";
                break;
            case 3:
                weekIdx.text = "2023년 2월 네번째";
                break;
        }
    }
}
