using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendCollider : MonoBehaviour
{
    public GameObject canvas;
    public Material overlay;
    public Material screenMaterial;

    public GameObject extendObject;


    public Transform extendParent;
    public RectTransform extendSize;

    public Transform beginningParent;
    public Vector2 beginningPos;
    public Vector3 beginningSize;



    public bool extend = false;

    private void Start()
    {
        overlay.renderQueue = 3000;
        screenMaterial.renderQueue = 3000;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.LogError(other.gameObject);
        if (Input.GetKeyDown(KeyCode.F)&& extend == false)
        {
            RawImage forExtendScreen = other.gameObject.GetComponent<RawImage>();
            RectTransform scale = other.gameObject.GetComponent<RectTransform>();

            if (forExtendScreen.material.renderQueue == 3000)
            {
                extendObject = other.gameObject;
                extend = true;
                // 초기값
                beginningPos = scale.transform.localPosition;
                beginningSize = scale.sizeDelta;
                //beginningParent = scale.GetComponentInParent<Transform>();
                


                other.transform.SetParent(extendParent);
                forExtendScreen.material.renderQueue = 3001;
                scale.sizeDelta = extendSize.sizeDelta;
                scale.transform.localPosition = extendSize.localPosition;


            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (extend == true)
            {
                screenMaterial.renderQueue = 3000;
                RectTransform scale = extendObject.GetComponent<RectTransform>();
                extendObject.transform.SetParent(beginningParent);
                scale.sizeDelta = beginningSize;
                scale.localPosition = beginningPos;
                extend = false;
            }
        }
    }
}
