using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_Diorama : MonoBehaviour
{
    public GameObject pressF;

    bool canInter;
    bool isInter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canInter)
        {
            pressF.SetActive(true);
            // F��ư�� Ȱ��ȭ �Ǿ����� �� F��ư�� ������ ���� ����
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
            // ESC ��ư�� ���� ��ȣ�ۿ� ����
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCustomize();
            }
        }
    }

    void StartCustomize()
    {
        // ��ȣ�ۿ� ����
        isInter = true;
        // Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        canInter = false;
        // �÷��̾ �������̰�
        Harry_GameManager.Instance.Player_CanMove = false;
    }

    // ���� ���� ����
    public void EndCustomize()
    {
        // ��ȣ�ۿ� ����
        isInter = false;
        // Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
        canInter = true;
        //�÷��̾ ������ �� �ְ�
        Harry_GameManager.Instance.Player_CanMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �ȿ� ������ Press F ��ư Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            canInter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������ ������ Press F ��ư ��Ȱ��ȭ
        Harry_CamController cc;
        if (other.gameObject.TryGetComponent<Harry_CamController>(out cc))
        {
            canInter = false;
        }
    }
}
