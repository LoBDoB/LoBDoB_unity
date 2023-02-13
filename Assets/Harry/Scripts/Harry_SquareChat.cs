using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_SquareChat : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }
    Vector3 dir;
    // Update is called once per frame
    void Update()
    {
        dir = transform.position + Vector3.up * 1.5f - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position = player.transform.position;
        foreach (Transform tr in transform)
        {
            tr.localPosition = new Vector3(1f, 2f, 0);
        }
    }
}
