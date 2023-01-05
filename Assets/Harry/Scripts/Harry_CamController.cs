using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_CamController : MonoBehaviourPun
{
    [SerializeField]
    float rotSpeed = 300f;

    GameObject player;

    float mx;
    float my;

    Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(PhotonNetwork.NickName);
        camPos = transform.GetChild(0).localPosition;

        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == true)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }

        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");

        if (Cursor.visible == false)
        {
            mx += mh * rotSpeed * Time.deltaTime;
            my += mv * rotSpeed * Time.deltaTime;
            my = Mathf.Clamp(my, -30, 30);
        }

        transform.eulerAngles = new Vector3(-my, mx, 0);

        //transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, camPos, Time.deltaTime * 5);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
