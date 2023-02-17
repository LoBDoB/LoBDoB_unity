using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_PutObject : MonoBehaviour
{
    Transform room;

    public GameObject objFactory;
    GameObject obj;
    bool canPut = true;
    public Material can;
    public Material cant;
    [SerializeField]
    List<Material> origMats = new List<Material>();
    Harry_ObjectCol objCol;

    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.Find("Map").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ThirdPut();
    }

    public void delObj()
    {
        if (obj)
        {
            Destroy(obj);
            obj = null;
        }
    }
    
    void ThirdPut()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Ground")))
            {
                obj = Instantiate(objFactory);
                obj.transform.parent = transform;
                objCol = obj.transform.GetChild(0).gameObject.AddComponent<Harry_ObjectCol>();
                AddOrigMats();
            }
        }
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Ground")))
            {
                obj.transform.position = hit.point;
                Vector3 angle = obj.transform.eulerAngles;
                angle.z = 0;
                angle.x = hit.normal.x;
                obj.transform.eulerAngles = angle;
            }
            else
            {
                canPut = false;
            }

            ChangeMat(canPut);

            if (Input.GetKey(KeyCode.Q))
                obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.E))
                obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
        }
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            ChangeToOrigMat();
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            obj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            obj.transform.parent = room;
            obj = null;
        }
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f))
            {
                if (hit.transform.parent.CompareTag("Obj"))
                {
                    Destroy(hit.transform.parent.gameObject);
                }
            }
        }
    }

    void ChangeMat(bool value)
    {
        Transform go = obj.transform.GetChild(0);
        if (value)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                Material[] mats = new Material[go.GetChild(i).GetComponent<Renderer>().materials.Length];

                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j] = can;
                }

                go.GetChild(i).GetComponent<Renderer>().materials = mats;
            }
        }
        else
        {
            for (int i = 0; i < go.childCount; i++)
            {
                Material[] mats = new Material[go.GetChild(i).GetComponent<Renderer>().materials.Length];

                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j] = cant;
                }

                go.GetChild(i).GetComponent<Renderer>().materials = mats;
            }
        }
    }

    void AddOrigMats()
    {
        origMats.Clear();
        Transform go = obj.transform.GetChild(0);
        for (int i = 0; i < go.childCount; i++)
        {
            foreach(Material mt in go.GetChild(i).GetComponent<Renderer>().materials)
            {
                origMats.Add(mt);
            }
        }
    }

    void ChangeToOrigMat()
    {
        Transform go = obj.transform.GetChild(0);
        int idx = 0;
        for (int i = 0; i < go.childCount; i++)
        {
            Material[] mats = new Material[go.GetChild(i).GetComponent<Renderer>().materials.Length];

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = origMats[idx++]; ;
            }

            go.GetChild(i).GetComponent<Renderer>().materials = mats;
        }
    }
}