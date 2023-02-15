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
            // ���콺 ��ư�� ������ �巡�� ����
            grapped = true;

            // ������� �����ϰ� ��ġ�� ���콺 ��ġ��
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
        // ���ǰ� �ִ� �������̸� ��ư ��Ȱ��ȭ
        if (Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name])
        {
            GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        // ���콺�� �� ��
        if (Input.GetMouseButtonUp(0))
        {
            // ������� �����ϰ� ��ũ�Ѻ��� ��ũ���� �����ϰ� ��ȯ
            Destroy(thumb);
            thumb = null;
            sr.vertical = true;

            // ���콺�� �� �� ���� �巡�� ���̾��ٸ�
            if (grapped)
            {
                // ���콺 ��ġ�� ����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // ���� ���콺 ��ġ�� ���� �������� �־��ٸ� ������ ����
                if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("SlotItem")))
                {
                    string objName = hit.collider.transform.gameObject.name;
                    if (objName.Length - 7 >= 0 && Harry_GameManager.Instance.Deco_Use.ContainsKey(objName.Substring(0, objName.Length - 7)))
                        //Harry_GameManager.Instance.Deco_Use[objName.Substring(0, objName.Length - 7)] = false;
                        photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, objName.Substring(0, objName.Length - 7), false);

                    // ���� �ִ� �������� �����ϰ� �� ��ġ�� ������ ������ �ٽ� ����
                    GameObject decoItem = PhotonNetwork.Instantiate(decoItemFac.name, hit.transform.parent.position, decoItemFac.transform.rotation);
                    //Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name] = true;
                    photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, decoItemFac.gameObject.name, true);

                    // ����Ʈ ����
                    GameObject effect = Instantiate(effectFac);
                    effect.transform.parent = hit.transform.parent;
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localScale = Vector3.one * 2.5f;

                    // ������ �������� �θ� ����
                    //decoItem.transform.parent = hit.transform.parent;
                    //decoItem.transform.localPosition = Vector3.zero;
                    photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, decoItem.name, hit.transform.parent.name);
                    decoItem.transform.localRotation = decoItemFac.transform.rotation;

                    // ������ �ִϸ��̼�
                    Vector3 scale = decoItem.transform.localScale;
                    decoItem.transform.localScale = scale * 0.5f;
                    iTween.ScaleTo(decoItem, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));

                    PhotonNetwork.Destroy(hit.collider.transform.gameObject);
                }
                // ���� ���콺 ��ġ�� ������ �ִٸ� �������� ��ġ
                else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Slot")))
                {
                    GameObject decoItem = PhotonNetwork.Instantiate(decoItemFac.name, hit.transform.parent.position, decoItemFac.transform.rotation);
                    //Harry_GameManager.Instance.Deco_Use[decoItemFac.gameObject.name] = true;
                    photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, decoItemFac.gameObject.name, true);

                    //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    photonView.RPC("RPCEnableSlot", RpcTarget.AllBuffered, hit.collider.gameObject.name, false);

                    // ����Ʈ ����
                    GameObject effect = Instantiate(effectFac);
                    effect.transform.parent = hit.transform;
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localScale = Vector3.one * 2.5f;

                    // ������ �������� �θ� ����
                    //decoItem.transform.parent = hit.transform;
                    //decoItem.transform.localPosition = Vector3.zero;
                    photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, decoItem.name, hit.transform.name);
                    decoItem.transform.localRotation = decoItemFac.transform.rotation;

                    // ������ �ִϸ��̼�
                    Vector3 scale = decoItem.transform.localScale;
                    decoItem.transform.localScale = scale * 0.5f;
                    iTween.ScaleTo(decoItem, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
                }
            }

            // �巡�� ����
            grapped = false;
        }
        
        if (grapped)
        {
            // ���� �巡�� ���̶�� ��ũ�� ���� ��ũ�� ����
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
