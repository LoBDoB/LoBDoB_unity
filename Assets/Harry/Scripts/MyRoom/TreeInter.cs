using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TreeInter : MonoBehaviourPun
{
    public GameObject pressF;
    public GameObject custom_UI;

    bool canInter;

    Quaternion treeRot;
    Vector3 treePos;
    Vector3 camPos;

    [SerializeField]
    Vector3 treeInterPos;
    [SerializeField]
    Vector3 treeInterRot;
    [SerializeField]
    Vector3 treeFrontPos;
    [SerializeField]
    Vector3 treeFrontRot;

    // Start is called before the first frame update
    void Start()
    {
        pressF.SetActive(false);
        custom_UI.SetActive(false);

        // 나무의 초기 각도 저장
        treeRot = transform.parent.rotation;
        treePos = transform.parent.position;

        camPos = treeFrontPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (canInter)
        {
            pressF.SetActive(true);
            // F버튼이 활성화 되어있을 때 F버튼을 누르면 커스터마이징 시작
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCustomize();
            }
        }
        else
        {
            pressF.SetActive(false);
        }

        // 커스터마이징 중이면 Q, E 버튼으로 나무 회전 가능
        if (custom_UI.activeSelf)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.parent.Rotate(0, -60f * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.E)) 
            {
                transform.parent.Rotate(0, 60f * Time.deltaTime, 0);
            }

            Vector3 dir = Vector3.zero; 
            float v = Input.GetAxisRaw("Vertical");
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            // W, S 키로 카메라 이동
            dir = v * Vector3.up;
            transform.parent.position -= dir * 15f * Time.deltaTime;

            Vector3 treeDir = (treePos - treeFrontPos).normalized;
            camPos += treeDir * scrollWheel * 1000f * Time.deltaTime;
            // 스크롤로 확대
            cc.StartInter(camPos, treeFrontRot);
        }

        // F키 눌러서 오브젝트 삭제
        if (Input.GetKeyDown(KeyCode.F))
        {        
            // 마우스 위치에 레이
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 만약 마우스 위치에 슬롯 아이템이 있었다면 아이템 삭제
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("SlotItem")))
            {
                string objName = hit.collider.transform.gameObject.name;
                //Harry_GameManager.Instance.Deco_Use[objName.Substring(0, objName.Length - 7)] = false;
                photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, objName.Substring(0, objName.Length - 7), false);

                //hit.collider.transform.parent.GetComponent<MeshRenderer>().enabled = true;
                photonView.RPC("RPCEnableSlot", RpcTarget.AllBuffered, hit.collider.transform.parent.gameObject.name, true);

                // 스케일 애니메이션
                iTween.ScaleTo(hit.collider.transform.gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
                StartCoroutine(DestroyObj(hit.collider.transform.gameObject));
            }
        }
    }

    IEnumerator DestroyObj(GameObject hit)
    {
        yield return new WaitForSeconds(0.53f);
        PhotonNetwork.Destroy(hit);
    }

    Harry_CamController cc;
    // 나무 커스터마이징 화면으로 전환
    void StartCustomize()
    {
        // 커서 활성화
        Cursor.visible = true;
        // 카메라에 Tree, Slot, SlotItem만 보이게 함
        Camera.main.cullingMask = 0;
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Tree");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Slot");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("SlotItem");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Effect");
        custom_UI.SetActive(true);
        canInter = false;
        // 카메라를 나무 정면으로 이동
        cc.StartInter(treeFrontPos, treeFrontRot);
        // 플레이어가 못움직이게
        Harry_GameManager.Instance.Player_CanMove = false;
    }

    // 나무 커스터마이징 종료
    public void EndCustomize()
    {
        // 커서 비활성화
        Cursor.visible = false;
        // 카메라에 모든걸 보이게 함
        Camera.main.cullingMask = -1;
        custom_UI.SetActive(false);
        canInter = true;
        // 구역 안에 들어올 때의 카메라 위치, 각도로 이동
        cc.StartInter(treeInterPos, treeInterRot);
        //플레이어가 움직일 수 있게
        Harry_GameManager.Instance.Player_CanMove = true;

        // Q, E 버튼으로 회전한 나무의 각도를 원상복구함
        transform.parent.rotation = treeRot;
        // 나무의 위치 또한 원상복구
        transform.parent.position = treePos;
        
        camPos = treeFrontPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 구역 안에 들어오면 Press F 버튼 활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc) && !custom_UI.activeSelf)
        {
            this.cc = cc;
            canInter= true;

            // 카메라 이동
            cc.StartInter(treeInterPos, treeInterRot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 구역을 나가면 Press F 버튼 비활성화
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc) && !custom_UI.activeSelf)
        {
            canInter = false;

            // 카메라 원상복구
            cc.EndInter();
        }
    }

    [PunRPC]
    void RPCDeco_Use(string key, bool value)
    {
        Harry_GameManager.Instance.Deco_Use[key] = value;
    }

    [PunRPC]
    void RPCEnableSlot(string name, bool value)
    {
        GameObject.Find(name).GetComponent<MeshRenderer>().enabled = value;
    }
}
