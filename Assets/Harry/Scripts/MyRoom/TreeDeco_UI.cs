using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;

public class TreeDeco_UI : MonoBehaviourPun, IPointerDownHandler
{
    public GameObject decoItemFac;
    public GameObject thumbFac;
    public ScrollRect sr;

    public GameObject effectFac;

    GameObject canvas;
    GameObject thumb;

    bool grapped;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<UnityEngine.UI.Button>().interactable)
        {
            // 마우스 버튼을 누르면 드래그 시작
            grapped = true;

            // 썸네일을 생성하고 위치를 마우스 위치로
            thumb = Instantiate(thumbFac);
            thumb.transform.parent = canvas.transform;
            thumb.transform.position = Input.mousePosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        thumbFac = transform.GetChild(0).gameObject;

        Harry_GameManager.Instance.Deco_Use.Add(decoItemFac.gameObject.name, false);
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        // 사용되고 있는 아이템이면 버튼 비활성화
        if (Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name])
        {
            GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        // 마우스를 뗄 때
        if (Input.GetMouseButtonUp(0))
        {
            // 썸네일을 제거하고 스크롤뷰의 스크롤을 가능하게 전환
            Destroy(thumb);
            thumb = null;
            sr.vertical = true;

            // 마우스를 뗄 때 만약 드래그 중이었다면
            if (grapped)
            {
                // 마우스 위치에 레이
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 만약 마우스 위치에 슬롯 아이템이 있었다면 아이템 삭제
                if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("SlotItem")))
                {
                    string objName = hit.collider.transform.gameObject.name;
                    if (objName.Length - 7 >= 0 && Harry_GameManager.Instance.Deco_Use.ContainsKey(objName.Substring(0, objName.Length - 7)))
                        //Harry_GameManager.Instance.Deco_Use[objName.Substring(0, objName.Length - 7)] = false;
                        photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, objName.Substring(0, objName.Length - 7), false);

                    // 원래 있던 아이템을 삭제하고 그 위치에 선택한 아이템 다시 생성
                    GameObject decoItem = PhotonNetwork.Instantiate(decoItemFac.name, hit.transform.parent.position, decoItemFac.transform.rotation);
                    //Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name] = true;
                    photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, decoItemFac.gameObject.name, true);

                    // 이펙트 생성
                    GameObject effect = Instantiate(effectFac);
                    effect.transform.parent = hit.transform.parent;
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localScale = Vector3.one * 2.5f;

                    // 생성한 아이템의 부모 설정
                    //decoItem.transform.parent = hit.transform.parent;
                    //decoItem.transform.localPosition = Vector3.zero;
                    photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, decoItem.name, hit.transform.parent.name);
                    decoItem.transform.localRotation = decoItemFac.transform.rotation;

                    // 스케일 애니메이션
                    Vector3 scale = decoItem.transform.localScale;
                    decoItem.transform.localScale = scale * 0.5f;
                    iTween.ScaleTo(decoItem, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));

                    PhotonNetwork.Destroy(hit.collider.transform.gameObject);
                }
                // 만약 마우스 위치에 슬롯이 있다면 아이템을 배치
                else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Slot")))
                {
                    GameObject decoItem = PhotonNetwork.Instantiate(decoItemFac.name, hit.transform.parent.position, decoItemFac.transform.rotation);
                    //Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name] = true;
                    photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, decoItemFac.gameObject.name, true);

                    //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    photonView.RPC("RPCEnableSlot", RpcTarget.AllBuffered, hit.collider.gameObject.name, false);

                    // 이펙트 생성
                    GameObject effect = Instantiate(effectFac);
                    effect.transform.parent = hit.transform;
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localScale = Vector3.one * 2.5f;

                    // 생성한 아이템의 부모 설정
                    //decoItem.transform.parent = hit.transform;
                    //decoItem.transform.localPosition = Vector3.zero;
                    photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, decoItem.name, hit.transform.name);
                    decoItem.transform.localRotation = decoItemFac.transform.rotation;

                    // 스케일 애니메이션
                    Vector3 scale = decoItem.transform.localScale;
                    decoItem.transform.localScale = scale * 0.5f;
                    iTween.ScaleTo(decoItem, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
                }
            }

            // 드래그 종료
            grapped = false;
        }
        
        if (grapped)
        {
            // 만약 드래그 중이라면 스크롤 뷰의 스크롤 중지
            sr.vertical = false;
            if (thumb != null)
                thumb.transform.position = Input.mousePosition;
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

    [PunRPC]
    void RPCSetParent(string itemName, string parentName)
    {
        GameObject.Find(itemName).transform.parent = GameObject.Find(parentName).transform;
        GameObject.Find(itemName).transform.localPosition = Vector3.zero;
    }
}
