using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    bool jump = false;
    public bool Jump { get; private set; }

    bool fly = false;
    public bool Fly { get; private set; }

    bool attack = false;
    public bool Attack { get; private set; }

    bool lookAround = false;
    public bool LookAround { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(Instance);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        jump = Input.GetKeyDown(KeyCode.R);
        fly = Input.GetKey(KeyCode.Space);
        attack = Input.GetKeyDown(KeyCode.E);
        lookAround = Input.GetKeyDown(KeyCode.T);
    }
}
