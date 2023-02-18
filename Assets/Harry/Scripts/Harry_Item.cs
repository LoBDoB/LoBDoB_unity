using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harry_Item : MonoBehaviour
{
    public Harry_PutObject po;
    public int idx;
    public int category;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClickItem(idx, category));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickItem(int idx, int category)
    {
        switch (category)
        {
            case 0:
                po.objFactory = po.buildings[idx];
                break;
            case 1:
                po.objFactory = po.plants[idx];
                break;
            case 2:
                po.objFactory = po.furnitures[idx];
                break;
            case 3:
                po.objFactory = po.etc[idx];
                break;
        }
    }
}
