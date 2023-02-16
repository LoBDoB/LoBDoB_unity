using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_SmallDate : MonoBehaviour
{
    Harry_Callender cal;
    public Harry_SmallCallender sc;
    public int idx;
    // Start is called before the first frame update
    void Start()
    {
        cal = GameObject.Find("Callender").GetComponent<Harry_Callender>(); 

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (sc.isStart)
            {
                sc.isStart = false;
                cal.StartIdx = idx;
                GetComponent<Image>().color = Color.blue;
            }
            else
            {
                sc.isStart = true;
                cal.EndIdx_ForSmall = idx;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (cal != null && idx >= cal.StartIdx && idx <= cal.EndIdx)
        {
            GetComponent<Image>().color = Color.blue;
        }
        else
            GetComponent<Image>().color = Color.white;
    }
}
