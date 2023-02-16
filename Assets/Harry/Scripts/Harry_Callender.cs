using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Harry_Callender : MonoBehaviour
{
    public List<GameObject> dates = new List<GameObject>();
    public List<Harry_Label> labels = new List<Harry_Label>();
    public GameObject label;
    public Dropdown pallete;
    public Toggle college;
    public Toggle club;
    public Toggle personal;
    public GameObject popup;

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
    public bool isPop = false;
    int endIdx = -1;
    public int EndIdx_ForSmall
    {
        get { return endIdx; }
        set
        {
            endIdx = value;
        }
    }
    public int EndIdx
    {
        get { return endIdx; }
        set
        {
            if (endIdx != value && startIdx != -1)
            {
                endIdx = value;

                GameObject go = Instantiate(popup);
                go.transform.parent = GameObject.Find("Canvas").transform;
                go.transform.position = Input.mousePosition;
                isPop = true;

                string title = "";
                go.transform.Find("TitleInput").GetComponent<InputField>().onEndEdit.AddListener((s) =>
                {
                    try
                    {
                        title = s;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Debug.Log(e.ToString());
                    }
                });
                string detail = "";
                go.transform.Find("DetailInput").GetComponent<InputField>().onEndEdit.AddListener((s) =>
                {
                    try
                    {
                        detail = s;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Debug.Log(e.ToString());
                    }
                });

                go.transform.Find("DateBtn").GetComponentInChildren<Text>().text = "22.02." + (startIdx + 1).ToString() + " ~ " + "22.02." + (endIdx + 1).ToString();
                go.transform.Find("DateBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameObject cal = go.transform.Find("SmallCallender").gameObject;
                    cal.SetActive(!cal.gameObject.activeSelf);
                });
                go.transform.Find("MemberBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameObject mem = go.transform.Find("Member").gameObject;
                    mem.SetActive(!mem.gameObject.activeSelf);
                });
                go.transform.Find("EnterBtn").GetComponent<Button>().onClick.AddListener(() => EnterEvent(title, detail, go));
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
        if (Input.GetMouseButtonUp(0) && !isPop)
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
            try
            {
                go.SetActive(value);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    void OnClickClub(bool value)
    {
        foreach (GameObject go in clubs)
        {
            try
            {
                go.SetActive(value);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    void OnClickPersonal(bool value)
    {
        foreach (GameObject go in personals)
        {
            try
            {
                go.SetActive(value);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    void SetWeek()
    {
        week = Mathf.Clamp(week, 0, 3);

        foreach (Transform tr in transform)
        {
            tr.gameObject.SetActive(false);
        }

        for (int i = week * 7; i < week * 7 + 7; i++)
        {
            dates[i].SetActive(true);
            Vector3 pos = dates[i].transform.localPosition;
            pos.y = -216;
            dates[i].transform.localPosition = pos;
            Color col = dates[i].gameObject.GetComponent<Image>().color;
            col.a = 1;
            dates[i].gameObject.GetComponent<Image>().color = col;
        }
        if (week != 3)
        {
            bool isLabel = false;
            for (int i = week * 7 + 7; i < week * 7 + 14; i++)
            {
                if (dates[i].GetComponent<Harry_Date>().labelCnt > 0)
                {
                    isLabel = true;
                    break;
                }
            }
            for (int i = week * 7 + 7; i < week * 7 + 14; i++)
            {
                dates[i].SetActive(true);
                Vector3 pos = dates[i].transform.localPosition;
                pos.y = -388;
                dates[i].transform.localPosition = pos;
                if (isLabel)
                {
                    Color col = dates[i].gameObject.GetComponent<Image>().color;
                    col.a = 1;
                    dates[i].gameObject.GetComponent<Image>().color = col;
                }
                else
                {
                    Color col = dates[i].gameObject.GetComponent<Image>().color;
                    col.a = 0.2f;
                    dates[i].gameObject.GetComponent<Image>().color = col;
                }
            }
        }

        switch (week)
        {
            case 0:
                weekIdx.text = "2023년 2월 첫 번째";
                break;
            case 1:
                weekIdx.text = "2023년 2월 두 번째";
                break;
            case 2:
                weekIdx.text = "2023년 2월 세 번째";
                break;
            case 3:
                weekIdx.text = "2023년 2월 네 번째";
                break;
        }
    }

    int cnt = 0;
    public void EnterEvent(string title, string detail, GameObject pop)
    {
        week = startIdx / 7;
        SetWeek();

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
            else
                go.transform.GetChild(0).GetComponent<Text>().text = title;

            dates[i].GetComponent<Harry_Date>().labelCnt = (maxCnt + 1);
            go.transform.localPosition = new Vector3(0, 30 - maxCnt * 25);

            go.GetComponent<Harry_Label>().id = cnt;
            go.GetComponent<Harry_Label>().title = title;
            go.GetComponent<Harry_Label>().detail = detail;
            go.GetComponent<Harry_Label>().startIdx = startIdx;
            go.GetComponent<Harry_Label>().endIdx = endIdx;

            List<string> list = pop.transform.Find("Member").GetComponent<Harry_Member>().members;
            go.GetComponent<Harry_Label>().members = list;

            labels.Add(go.GetComponent<Harry_Label>());
        }
        cnt++;

        if (endIdx >= week * 7 + 7)
        {
            for (int i = week * 7 + 7; i < week * 7 + 14; i++)
            {
                Color col = dates[i].gameObject.GetComponent<Image>().color;
                col.a = 1;
                dates[i].gameObject.GetComponent<Image>().color = col;
            }
        }

        Destroy(pop);
        isPop = false;
        endIdx = -1;
        startIdx = -1;
    }
}
