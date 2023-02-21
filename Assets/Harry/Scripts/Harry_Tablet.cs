using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_Tablet : MonoBehaviour
{
    public Harry_CamController cam;
    [SerializeField]
    GameObject cal;
    [SerializeField]
    GameObject my;
    [SerializeField]
    GameObject lec;

    bool isOn = false;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value && cam != null)
            {
                isOn = value;

                if (isOn)
                {
                    iTween.MoveTo(transform.GetChild(0).gameObject, iTween.Hash("x", -0.187f, "y", -0.033f, "z", 0.322f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));

                    cam.StartInter(Harry_AllUIManager.Instance.player.transform.position + new Vector3(0, 1.6f, 0.15f),
                        Harry_AllUIManager.Instance.player.transform.eulerAngles + new Vector3(21f, 0, 0));
                    Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Player"));
                    Cursor.visible = true;
                }
                else
                {
                    iTween.MoveTo(transform.GetChild(0).gameObject, iTween.Hash("x", -0.349000007f, "y", -0.158999994f, "z", 0.244000003f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                    cam.EndInter();
                    Camera.main.cullingMask = -1;
                }
            }
            switch (Harry_AllUIManager.Instance.tabletUI)
            {
                case 0:
                    cal.SetActive(true);
                    my.SetActive(false);
                    lec.SetActive(false);
                    break;
                case 1:
                    cal.SetActive(false);
                    my.SetActive(true);
                    lec.SetActive(false);
                    break;
                case 2:
                    cal.SetActive(false);
                    my.SetActive(false);
                    lec.SetActive(true);
                    break;
            }
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        IsOn = Harry_AllUIManager.Instance.isTablet;
    }
}
