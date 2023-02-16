using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_Member : MonoBehaviour
{
    public List<string> members = new List<string>();

    InputField search;
    Transform mem_content;
    Transform sch_content;
    Button invite;
    // Start is called before the first frame update
    void Awake()
    {
        search = transform.Find("Search").GetComponent<InputField>();
        invite = transform.Find("InviteBtn").GetComponent<Button>();
        mem_content = transform.Find("Friends").Find("Viewport").Find("Content");
        sch_content = transform.Find("SearchResult").Find("Viewport").Find("Content");
        transform.Find("SearchResult").gameObject.SetActive(false);

        search.onSubmit.AddListener((s) =>
        {
            try
            {
                transform.Find("SearchResult").gameObject.SetActive(true);
                foreach (Transform tr in sch_content)
                {
                    if (tr.GetComponent<Harry_Friend>().Name.Contains(s))
                    {
                        tr.gameObject.SetActive(true);
                    }
                    else
                        tr.gameObject.SetActive(false);
                }
                search.text = "";
            }
            catch { }
        });
        invite.onClick.AddListener(() =>
        {
            members.Clear();
            foreach (Transform tr in mem_content)
            {
                if (tr.gameObject.activeSelf)
                    members.Add(tr.GetComponent<Harry_Friend>().Name);
            }
            gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        foreach (Transform tr in mem_content)
        {
            if (members.Contains(tr.GetComponent<Harry_Friend>().Name))
                tr.gameObject.SetActive(true);
            else
                tr.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
