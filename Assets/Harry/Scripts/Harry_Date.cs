using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Harry_Date : MonoBehaviour, IPointerDownHandler
{
    Harry_Callender callender;
    public int labelCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        callender = transform.parent.GetComponent<Harry_Callender>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < callender.dates.Count; i++)
        {
            if (callender.dates[i] == gameObject)
            {
                callender.StartIdx = i;
            }
        }
    }
}
