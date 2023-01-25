using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_Test : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            cc.StartInter(new Vector3(0.73f, 1f, 0.77f), new Vector3(12.13f, -30.1f, 0));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            cc.EndInter();
        }
    }
}
