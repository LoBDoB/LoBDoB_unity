using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_PostBox : MonoBehaviour
{
    public GameObject notice;
    public GameObject pressF;

    Harry_CamController cc;
    bool canInter;
    bool isInter;

    // Start is called before the first frame update
    void Start()
    {
        notice.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canInter)
        {
            pressF.SetActive(true);
            // F버튼이 활성화 되어있을 때 F버튼을 누르면 보기 시작
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCustomize();
            }
        }
        else
        {
            pressF.SetActive(false);
        }

        if (isInter)
        {
            // ESC 버튼을 통해 상호작용 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCustomize();
            }
        }
    }

    void StartCustomize()
    {
        // 상호작용 시작
        isInter = true;
        // 커서 활성화
        Cursor.visible = true;
        canInter = false;
        // 플레이어가 못움직이게
        Harry_GameManager.Instance.Player_CanMove = false;

        notice.SetActive(true);
    }

    // 구슬 보기 종료
    public void EndCustomize()
    {
        // 상호작용 종료
        isInter = false;
        // 커서 비활성화
        Cursor.visible = false;
        canInter = true;
        //플레이어가 움직일 수 있게
        Harry_GameManager.Instance.Player_CanMove = true;

        notice.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 구역 안에 들어오면 Press F 버튼 활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            this.cc = cc;
            canInter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 구역을 나가면 Press F 버튼 비활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            canInter = false;
        }
    }
}
