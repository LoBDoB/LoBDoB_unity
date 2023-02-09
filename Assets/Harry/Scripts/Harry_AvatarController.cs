using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Harry_AvatarController : MonoBehaviourPun, IPunObservable
{
    private Animator _Animator;
    private CharacterController _Ctrl;
    private Vector3 _MoveDirection = Vector3.zero;
    [SerializeField]
    private float _Speed;

    private Dictionary<string, bool> _Status = new Dictionary<string, bool>
    {
        {"Jump", false },
    };

    void Start()
    {
        _Animator = this.GetComponent<Animator>();
        _Ctrl = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            GRAVITY();
            STATUS();

            if (!_Status.ContainsValue(true))
            {
                MOVE();
                KEY_UP();
                JUMP();
            }
            else if (_Status.ContainsValue(true))
            {
                string status_name = "";
                foreach (var i in _Status)
                {
                    if (i.Value == true)
                    {
                        status_name = i.Key;
                        break;
                    }
                }
                if (status_name == "Jump")
                {
                    MOVE();
                    JUMP();
                }
            }
        }
        else if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 15 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 15 * Time.deltaTime);
        }
    }
    //--------------------------------------------------------------------- STATUS
    private void STATUS()
    {
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            _Status["Jump"] = true;
        }
        else if (!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            _Status["Jump"] = false;
        }
    }
    //--------------------------------------------------------------------- GRAVITY
    private void GRAVITY()
    {
        // isGrounded
        if (_Ctrl.isGrounded)
        {
            if (_MoveDirection.y < -0.5f)
            {
                _MoveDirection.y = -0.5f;
            }
        }
        else if (!_Ctrl.isGrounded)
        {
            _MoveDirection.y -= 4.5f * Time.deltaTime;
        }

        _Ctrl.Move(_MoveDirection * Time.deltaTime);
    }
    //--------------------------------------------------------------------- MOVE
    float speed = 0;
    float animSpeed = 0;
    private void MOVE()
    {
        //------------------------------------------------------------ Speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, _Speed * 2, Time.deltaTime * 5f);
            animSpeed = Mathf.Lerp(animSpeed, 1, Time.deltaTime * 5f);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Speed", animSpeed);
        }
        else
        {
            speed = Mathf.Lerp(speed, _Speed, Time.deltaTime * 5f);
            animSpeed = Mathf.Lerp(animSpeed, 0, Time.deltaTime * 5f);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Speed", animSpeed);
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = h * Camera.main.transform.right + v * Camera.main.transform.forward;
        dir.y = 0;
        dir.Normalize();

        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

            Vector3 velocity = this.transform.rotation * new Vector3(0, 0, speed);
            MOVE_XZ(velocity);
            MOVE_RESET();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
            }
        }

        //KEY_UP();
    }
    //--------------------------------------------------------------------- MOVE_SUB
    private void MOVE_XZ(Vector3 velocity)
    {
        _MoveDirection = new Vector3(velocity.x, _MoveDirection.y, velocity.z);
        _Ctrl.Move(_MoveDirection * Time.deltaTime);
    }
    private void MOVE_RESET()
    {
        _MoveDirection.x = 0;
        _MoveDirection.z = 0;
    }
    //--------------------------------------------------------------------- KEY_UP
    private void KEY_UP()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
    }
    //--------------------------------------------------------------------- JUMP
    private void JUMP()
    {
        if (_Ctrl.isGrounded)
        {
            if (!_Animator.IsInTransition(0))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC("RPCCrossFade", RpcTarget.All, "jump", 0.1f);
                    _MoveDirection.y = 2.5f;
                }
                if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
                    }
                    else
                    {
                        photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
                    }
                }
            }
            KEY_UP();
        }
    }

    //도착 위치
    Vector3 receivePos;
    //회전되야 하는 값
    Quaternion receiveRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //데이터 보내기
        if (stream.IsWriting) // isMine == true
        {
            //position, rotation
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        //데이터 받기
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void RPCSetFloat(string key, float value)
    {
        if (_Animator != null)
            _Animator.SetFloat(key, value);
    }

    [PunRPC]
    void RPCCrossFade(string stateName, float normalizedTransitionDuration)
    {
        if (_Animator != null)
            _Animator.CrossFade(stateName, normalizedTransitionDuration);
    }
}