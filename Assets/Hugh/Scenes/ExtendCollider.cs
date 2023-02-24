using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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


    public Sprite[] reductionExtendImage;


    public GameObject[] hideButton;


    public bool extend = false;

    public bool onClick = false;

    private void Start()
    {
        overlay.renderQueue = 3000;
        screenMaterial.renderQueue = 3000;
    }

    public void Click()
    {
        onClick = true;
        for (int i = 0; i < hideButton.Length; i++)
        {
            hideButton[i].SetActive(true);
        }
        if (onClick == true && extend == true)
        {
            Image btnImage = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
            btnImage.sprite = reductionExtendImage[0];
            screenMaterial.renderQueue = 3000;
            RectTransform scale = extendObject.GetComponent<RectTransform>();
            extendObject.transform.SetParent(beginningParent);
            scale.sizeDelta = beginningSize;
            scale.localPosition = beginningPos;
            extend = false;
            onClick = false;

            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.LogError("31313");
        if (other.gameObject.name != "MainCameraView")
        {
            return;

        }


        //Debug.LogError(other.gameObject.name);
        if (onClick == true && extend == false)
        {
            RawImage forExtendScreen = other.gameObject.GetComponent<RawImage>();
            RectTransform scale = other.gameObject.GetComponent<RectTransform>();
            extendObject = other.gameObject;
            extend = true;
            onClick = false;
            // 초기값
            beginningPos = scale.transform.localPosition;
            beginningSize = scale.sizeDelta;
            //beginningParent = scale.GetComponentInParent<Transform>();

            other.transform.SetParent(extendParent);
            forExtendScreen.material.renderQueue = 3001;
            scale.sizeDelta = extendSize.sizeDelta;
            scale.transform.localPosition = extendSize.localPosition;
            Image btnImage = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
            btnImage.sprite = reductionExtendImage[1];
            for (int i = 0; i < hideButton.Length; i++)
            {
                hideButton[i].SetActive(false);
            }

        }
    }
}
