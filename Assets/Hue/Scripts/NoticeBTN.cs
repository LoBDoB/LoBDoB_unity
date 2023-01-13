using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBTN : MonoBehaviour
{
    public void SurfingBtn()
    {
        Application.OpenURL(gameObject.name);
    }
}
