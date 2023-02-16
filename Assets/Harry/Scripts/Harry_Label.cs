using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_Label : MonoBehaviour
{
    Harry_Callender cal;

    public int id;
    public int startIdx;
    public int endIdx;
    public string title;
    public string detail;
    public List<string> members = new List<string>();
    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        cal = GameObject.Find("Callender").GetComponent<Harry_Callender>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        if (!cal.isPop)
        {
            GameObject go = Instantiate(popup);
            go.transform.parent = GameObject.Find("Canvas").transform;
            go.transform.position = Input.mousePosition;
            cal.isPop = true;

            go.transform.Find("TitleInput").GetComponent<InputField>().text = title;
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
            go.transform.Find("DetailInput").GetComponent<InputField>().text = detail;
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
            cal.StartIdx = startIdx;
            cal.EndIdx_ForSmall = endIdx;

            go.transform.Find("DateBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject cal = go.transform.Find("SmallCallender").gameObject;
                cal.SetActive(!cal.gameObject.activeSelf);
            });
            go.transform.Find("MemberBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject mem = go.transform.Find("Member").gameObject;
                mem.GetComponent<Harry_Member>().members = this.members;
                mem.SetActive(!mem.gameObject.activeSelf);
            });
            go.transform.Find("EnterBtn").GetComponent<Button>().onClick.AddListener(() => cal.EnterEvent(title, detail, go));
            go.transform.Find("EnterBtn").GetComponent<Button>().onClick.AddListener(Delete_orig);
        }
    }

    void Delete_orig()
    {
        foreach (Harry_Label label in cal.labels)
        {
            if (label.id == this.id)
            {
                Destroy(label.gameObject);
            }
        }
    }
}
