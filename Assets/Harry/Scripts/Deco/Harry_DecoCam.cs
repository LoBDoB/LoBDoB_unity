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
        // WASD 키로 캠 위치 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        dir = h * transform.right + v * transform.forward;
        // 마우스 좌클릭으로 캠 위치 이동
        if (Input.GetMouseButton(0))
            dir = -mh * transform.right + -mv * transform.up;
        transform.position += dir * speed * 5 * Time.deltaTime;
        // 마우스 우클릭으로 캠 각도 이동
        Vector3 rotDir = new Vector3(-mv, mh, 0);
        if (Input.GetMouseButton(1))
            transform.eulerAngles += rotDir * rotSpeed * 5 * Time.deltaTime;
        // 스크롤로 확대
        transform.position += scrollWheel * transform.forward * speed * 50 * Time.deltaTime;
    }
}