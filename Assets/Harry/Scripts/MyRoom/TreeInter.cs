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

        // ������ �ʱ� ���� ����
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
            // F��ư�� Ȱ��ȭ �Ǿ����� �� F��ư�� ������ Ŀ���͸���¡ ����
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCustomize();
            }
        }
        else
        {
            pressF.SetActive(false);
        }

        // Ŀ���͸���¡ ���̸� Q, E ��ư���� ���� ȸ�� ����
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
            // W, S Ű�� ī�޶� �̵�
            dir = v * Vector3.up;
            transform.parent.position -= dir * 15f * Time.deltaTime;

            Vector3 treeDir = (treePos - treeFrontPos).normalized;
            camPos += treeDir * scrollWheel * 1000f * Time.deltaTime;
            // ��ũ�ѷ� Ȯ��
            cc.StartInter(camPos, treeFrontRot);
        }

        // FŰ ������ ������Ʈ ����
        if (Input.GetKeyDown(KeyCode.F))
        {        
            // ���콺 ��ġ�� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���� ���콺 ��ġ�� ���� �������� �־��ٸ� ������ ����
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("SlotItem")))
            {
                string objName = hit.collider.transform.gameObject.name;
                //Harry_GameManager.Instance.Deco_Use[objName.Substring(0, objName.Length - 7)] = false;
                photonView.RPC("RPCDeco_Use", RpcTarget.AllBuffered, objName.Substring(0, objName.Length - 7), false);

                //hit.collider.transform.parent.GetComponent<MeshRenderer>().enabled = true;
                photonView.RPC("RPCEnableSlot", RpcTarget.AllBuffered, hit.collider.transform.parent.gameObject.name, true);

                // ������ �ִϸ��̼�
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
    // ���� Ŀ���͸���¡ ȭ������ ��ȯ
    void StartCustomize()
    {
        // Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        // ī�޶� Tree, Slot, SlotItem�� ���̰� ��
        Camera.main.cullingMask = 0;
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Tree");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Slot");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("SlotItem");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Effect");
        custom_UI.SetActive(true);
        canInter = false;
        // ī�޶� ���� �������� �̵�
        cc.StartInter(treeFrontPos, treeFrontRot);
        // �÷��̾ �������̰�
        Harry_GameManager.Instance.Player_CanMove = false;
    }

    // ���� Ŀ���͸���¡ ����
    public void EndCustomize()
    {
        // Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
        // ī�޶� ���� ���̰� ��
        Camera.main.cullingMask = -1;
        custom_UI.SetActive(false);
        canInter = true;
        // ���� �ȿ� ���� ���� ī�޶� ��ġ, ������ �̵�
        cc.StartInter(treeInterPos, treeInterRot);
        //�÷��̾ ������ �� �ְ�
        Harry_GameManager.Instance.Player_CanMove = true;

        // Q, E ��ư���� ȸ���� ������ ������ ���󺹱���
        transform.parent.rotation = treeRot;
        // ������ ��ġ ���� ���󺹱�
        transform.parent.position = treePos;
        
        camPos = treeFrontPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �ȿ� ������ Press F ��ư Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc) && !custom_UI.activeSelf)
        {
            this.cc = cc;
            canInter= true;

            // ī�޶� �̵�
            cc.StartInter(treeInterPos, treeInterRot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������ ������ Press F ��ư ��Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc) && !custom_UI.activeSelf)
        {
            canInter = false;

            // ī�޶� ���󺹱�
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
