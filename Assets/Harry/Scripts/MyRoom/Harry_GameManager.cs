using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Harry_GameManager : MonoBehaviour
{
    public static Harry_GameManager Instance;

    public Dictionary<string, bool> Deco_Use = new Dictionary<string, bool>();

    bool player_CanMove = true;
    public bool Player_CanMove { get { return player_CanMove; } set { player_CanMove = value; } }

    GameObject decoCam;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        decoCam = GameObject.Find("Deco Camera");
        if (decoCam)
            decoCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickDeco()
    {
        if (Camera.main != null)
        {
            if (decoCam.activeSelf)
            {
                decoCam.GetComponent<Harry_DecoCam>().cc.EndInter();
                player_CanMove = true;
                decoCam.SetActive(false);
            }
            else
            {
                decoCam.SetActive(true);
                player_CanMove = false;
                decoCam.GetComponent<Harry_DecoCam>().cc = Camera.main.transform.parent.GetComponent<Harry_CamController>();
            }
        }
    }
}
