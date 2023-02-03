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
                    Debug.Log(i + ": " + dates[i].GetComponent<Harry_Date>().labelCnt);
                    if (dates[i].GetComponent<Harry_Date>().labelCnt >= maxCnt)
                        maxCnt = dates[i].GetComponent<Harry_Date>().labelCnt;
                }

                for (int i = startIdx; i <= endIdx; i++)
                {
                    //Instantiate(label, dates[i].transform.Find("Viewport").transform.Find("Content"));
                    GameObject go = Instantiate(label, dates[i].transform);
                    dates[i].GetComponent<Harry_Date>().labelCnt = (maxCnt + 1);
                    //go.GetComponent<RectTransform>().position = new Vector3(0, 30 - maxCnt * 25);
                    go.transform.localPosition = new Vector3(0, 30 - maxCnt * 25);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GraphicRaycaster ray = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
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
}
