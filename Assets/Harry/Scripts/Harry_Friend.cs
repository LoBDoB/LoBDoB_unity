using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_Friend : MonoBehaviour
{
    Transform mem_content;
    public string Name;
    // Start is called before the first frame update
    void Start()
    {
        mem_content = GameObject.Find("Friends").transform.Find("Viewport").Find("Content");
        GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (Transform tr in mem_content)
            {
                if (tr.GetComponent<Harry_Friend>().Name == Name) 
                    tr.gameObject.SetActive(!tr.gameObject.activeSelf);
            }
            GameObject.Find("SearchResult").gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
