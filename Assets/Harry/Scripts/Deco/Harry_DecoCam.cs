using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_DecoCam : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float rotSpeed;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Third_Demen_Cam();
    }

    void Third_Demen_Cam()
    {
        Vector3 dir;
        // WASD Ű�� ķ ��ġ �̵�
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        dir = h * transform.right + v * transform.forward;
        // ���콺 ��Ŭ������ ķ ��ġ �̵�
        if (Input.GetMouseButton(0))
            dir = -mh * transform.right + -mv * transform.up;
        transform.position += dir * speed * 5 * Time.deltaTime;
        // ���콺 ��Ŭ������ ķ ���� �̵�
        Vector3 rotDir = new Vector3(-mv, mh, 0);
        if (Input.GetMouseButton(1))
            transform.eulerAngles += rotDir * rotSpeed * 5 * Time.deltaTime;
        // ��ũ�ѷ� Ȯ��
        transform.position += scrollWheel * transform.forward * speed * 50 * Time.deltaTime;
    }
}