using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OutMeetingRoom : MonoBehaviour
{

    //private void OnEnable()
    //{
    //    gameObject.GetComponent<Button>().onClick.AddListener(()=>OutButton());

    //}


    public void OutButton()
    {
        SceneManager.LoadScene(0);


    }

}
