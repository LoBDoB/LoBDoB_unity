using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_SmallCallender : MonoBehaviour
{
    public bool isStart = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            transform.GetChild(i).GetComponent<Harry_SmallDate>().idx = i;
            transform.GetChild(i).GetComponent<Harry_SmallDate>().sc = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
