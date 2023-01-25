using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_CamController : MonoBehaviourPun
{
    [SerializeField]
    float rotSpeed = 600f;

    GameObject player;

    float mx;
    float my;

    Vector3 camPos;
    Vector3 uiCamPos;
    Vector3 uiCamRot;

    bool canRotate = false;
    bool isInter = false;
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
        // ESC 버튼으로 커서 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == true)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }

        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");

        // 커서 비활성화 시에만 시점 전환
        if (Cursor.visible == false && !isInter)
        {
            mx += mh * rotSpeed * Time.deltaTime;
            my += mv * rotSpeed * Time.deltaTime;
            my = Mathf.Clamp(my, -30, 30);

            transform.eulerAngles = new Vector3(-my, mx, 0);
        }

        // 상호작용 중이지 않으면 마우스 이동에 따라 회전
        if (!isInter)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, camPos, Time.deltaTime * 5f);
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
        // 상호작용 중이면 지정한 위치, 각도로 카메라 고정
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, uiCamPos, Time.deltaTime * 5f);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.Euler(uiCamRot), Time.deltaTime * 5f);
        }
    }

    // 상호작용 시작 시 호출하는 함수
    // pos에 위치, angle에 각도를 매개변수로
    public void StartInter(Vector3 pos, Vector3 angle)
    {
        isInter = true;
        uiCamPos = pos;
        uiCamRot = angle;
    }

    // 상호작용 종료 시 호출하는 함수
    public void EndInter()
    {
        isInter = false;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
