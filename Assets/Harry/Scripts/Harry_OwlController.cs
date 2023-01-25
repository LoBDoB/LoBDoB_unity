using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Harry_OwlController : MonoBehaviourPun, IPunObservable
{
    public GameObject avatarCam;

    private Animator _Animator;
    private CharacterController _Ctrl;
    private Vector3 _MoveDirection = Vector3.zero;
    private float _Speed;
    private GameObject _View_Camera;
    private const int _Status_ground = 0;
    private const int _Status_onTree = 1;
    private const int _Status_fly = 2;
    private const int _Pose_fly = 0;
    private const int _Pose_gliding = 1;

    private Dictionary<string, bool> _Status = new Dictionary<string, bool>
    {
        {"Jump", false },
        {"Damage", false },
        {"Stop", false },
        {"Attack", false },
        {"onTree", false },
        {"Fly", false },
        {"Sing", false },
        {"Look", false },
    };

    void Start()
    {
        _Animator = this.GetComponent<Animator>();
        _Ctrl = this.GetComponent<CharacterController>();
        _View_Camera = GameObject.Find("CamFollow");

        //if (!photonView.IsMine)
        //{
        //    avatarCam.SetActive(false);
        //}
    }

    //private void LateUpdate()
    //{
    //    CAMERA();
    //}

    void Update()
    {
        if (photonView.IsMine && Harry_GameManager.Instance.Player_CanMove)
        {
            GRAVITY();
            STATUS();

            if (!_Status.ContainsValue(true))
            {
                MOVE();
                KEY_UP();
                JUMP();
                DAMAGE();
                ATTACK();
                SING();
                LOOK_AROUND();
                STOP();
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
                else if (status_name == "Damage")
                {
                    DAMAGE();
                }
                else if (status_name == "Stop")
                {
                    STOP();
                }
                else if (status_name == "Attack")
                {
                    ATTACK();
                }
                else if (status_name == "Fly")
                {
                    MOVE();
                    KEY_UP();
                    DAMAGE();
                }
                else if (status_name == "Sing")
                {
                    SING();
                }
                else if (status_name == "Look")
                {
                    LOOK_AROUND();
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
    private void STATUS ()
    {
        if(_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            _Status["Jump"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            _Status["Jump"] = false;
        }

        if(_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            _Status["Damage"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            _Status["Damage"] = false;
        }

        if(_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
        {
            _Status["Stop"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
        {
            _Status["Stop"] = false;
        }

        if(_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _Status["Attack"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _Status["Attack"] = false;
        }

        if(_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Sing"))
        {
            _Status["Sing"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Sing"))
        {
            _Status["Sing"] = false;
        }

        if (_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Look"))
        {
            _Status["Look"] = true;
        }
        else if(!_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Look"))
        {
            _Status["Look"] = false;
        }

        RaycastHit hit;
        bool isOnWater = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hit, 0.3f, LayerMask.GetMask("Water"));
        //Debug.Log(isOnWater);
        if (isOnWater)
        {
            _Status["Fly"] = true;
        }
        else if (Input.GetKey(KeyCode.Space) && !_Ctrl.isGrounded)
		{
            _Status["Fly"] = true;
        }
        else if (!Input.GetKey(KeyCode.Space) && _Ctrl.isGrounded && !isOnWater)
		{
            _Status["Fly"] = false;
        }
    }
    //--------------------------------------------------------------------- CAMERA
    private void CAMERA ()
	{
         _View_Camera.transform.position = this.transform.position;
                //+ new Vector3(0, 0.5f, 3);
	}
    //--------------------------------------------------------------------- GRAVITY
	private void GRAVITY ()
	{
        // isGrounded
		if (_Ctrl.isGrounded)
		{
			if(_MoveDirection.y < -0.5f)
            {
                _MoveDirection.y = -0.5f;
            }
		}
		else if (!_Ctrl.isGrounded)
		{
            _MoveDirection.y -= 1.5f * Time.deltaTime;
		}
        // Status
        if (_Status["Fly"])
		{
            //_Animator.SetFloat("Status", Mathf.Lerp(_Animator.GetFloat("Status"), _Status_fly, Time.deltaTime * 15f));
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Status", Mathf.Lerp(_Animator.GetFloat("Status"), _Status_fly, Time.deltaTime * 15f));
        }
        else if(!_Status["Fly"])
		{
            //_Animator.SetFloat("Status", Mathf.Lerp(_Animator.GetFloat("Status"), _Status_ground, Time.deltaTime * 15f));
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Status", Mathf.Lerp(_Animator.GetFloat("Status"), _Status_ground, Time.deltaTime * 15f));
        }
        // W KeyDown & Up
        Coroutine fly_pose = null;
        if (Input.GetKeyDown(KeyCode.Space))
		{
            if (_Ctrl.isGrounded)
		    {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "take_off", 0.1f);
            }
            else if (!_Ctrl.isGrounded)
		    {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
            }
            fly_pose = null;
            fly_pose = StartCoroutine(FlyPose());
        }
        else if (Input.GetKeyUp(KeyCode.Space))
		{
            photonView.RPC("RPCCrossFade", RpcTarget.All, "landing", 0.1f);
            try{
                StopCoroutine(fly_pose);
            }
            catch (System.NullReferenceException)
            {
                // Nothing method
            }
        }
        // Cross Fade Animation
        if(_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && _Animator.GetCurrentAnimatorStateInfo(0).IsName("take_off")
            && !_Animator.IsInTransition(0))
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
            }
            else{
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.5f);
            }
        }
        else if(_Animator.GetCurrentAnimatorStateInfo(0).IsName("landing")
            && !_Animator.IsInTransition(0)
            && _Ctrl.isGrounded)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
            }
            else{
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
        // rise & descent position
        if (Input.GetKey(KeyCode.Space) && !_Status["Stop"])
		{
			if(this.transform.position.y < 22.5f)
			{
				_MoveDirection.y = 1.5f;
			}
			else{
				_MoveDirection.y = 0;
			}
		}
        else{
			_MoveDirection.y -= 1.0f * Time.deltaTime;
		}

		_Ctrl.Move(_MoveDirection * Time.deltaTime);
	}
    //--------------------------------------------------------------------- FlyPose
    private IEnumerator FlyPose ()
    {
        while(true) {
            yield return new WaitForSeconds(2.0f);
            //_Animator.SetFloat("FlyPose", _Pose_gliding);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "FlyPose", (float)_Pose_gliding);
            yield return new WaitForSeconds(2.0f);
            //_Animator.SetFloat("FlyPose", _Pose_fly);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "FlyPose", (float)_Pose_fly);
        }
    }
    //--------------------------------------------------------------------- MOVE
    private void MOVE()
    {
        float speed = 2;
        //------------------------------------------------------------ Speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 4;
            //_Animator.SetFloat("Speed", 1);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Speed", 1f);
        }
        else 
        {
            speed = 2;
            //_Animator.SetFloat("Speed", 0);
            photonView.RPC("RPCSetFloat", RpcTarget.All, "Speed", 0f);
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

        ////------------------------------------------------------------ Forward
        //if (Input.GetKey(KeyCode.W))
        //{
        //    // velocity
        //    if(_Animator.GetCurrentAnimatorStateInfo(0).IsName("move") || !_Ctrl.isGrounded)
        //    {
        //        Vector3 velocity = this.transform.rotation * new Vector3(0, 0, speed);
        //        MOVE_XZ(velocity);
        //        MOVE_RESET();
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            if(!_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
            }
        }
        
        ////------------------------------------------------------------ character rotation
        //if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        //{
        //    this.transform.Rotate(Vector3.up, 0.5f);
        //}
        //else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        //{
        //    this.transform.Rotate(Vector3.up, -0.5f);
        //}
        //if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        //{
        //    if(_Ctrl.isGrounded)
        //    {
        //        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A))
        //        {
        //    	    photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D))
        //        {
        //    	    photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
        //        }
        //    }
        //    // rotate stop
        //    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        //    {
        //        if(!_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        //        {
        //    	    photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
        //        }
        //    }
        //}
	}
    //--------------------------------------------------------------------- MOVE_SUB
	private void MOVE_XZ (Vector3 velocity)
	{
        _MoveDirection = new Vector3 (velocity.x, _MoveDirection.y, velocity.z);
        _Ctrl.Move(_MoveDirection * Time.deltaTime);
    }
    private void MOVE_RESET()
    {
        _MoveDirection.x = 0;
        _MoveDirection.z = 0;
    }
    //--------------------------------------------------------------------- KEY_UP
    private void KEY_UP ()
	{
	    if (Input.GetKeyUp(KeyCode.W))
        {
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
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
        //else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        //{
    	   // if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        //	{
        //        if(Input.GetKey(KeyCode.A))
        //        {
        //            photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
        //        }
        //        else if(Input.GetKey(KeyCode.D))
        //        {
        //            photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
        //        }
        //        else{
        //    	    photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
        //        }
        //    }
        //}
	}
    //--------------------------------------------------------------------- JUMP
	private void JUMP ()
	{
        if(_Ctrl.isGrounded)
        {
		    if(!_Animator.IsInTransition(0))
		    {
				if (Input.GetKeyDown(KeyCode.E))
				{
                    photonView.RPC("RPCCrossFade", RpcTarget.All, "jump", 0.1f);
					_MoveDirection.y = 2.5f;
                    StartCoroutine(JumpPose());
				}
                if(_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        photonView.RPC("RPCCrossFade", RpcTarget.All, "move", 0.1f);
                    }
                    else{
                        photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
                    }
                }
			}
		}
	}
    //--------------------------------------------------------------------- JumpPose
    private IEnumerator JumpPose ()
    {
        float t = 0;
        while(true)
        {
            if(t >= 1)
            {
                yield break;
            }
            t += 1 * Time.deltaTime;
            photonView.RPC("RPCSetFloat", RpcTarget.All, "JumpPose", t);
            yield return null;
        }
    }
    //--------------------------------------------------------------------- DAMAGE
	private void DAMAGE ()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			photonView.RPC("RPCCrossFade", RpcTarget.All, "damage", 0.1f);
		}
        if (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && _Animator.GetCurrentAnimatorStateInfo(0).IsTag("Damage")
            && !_Animator.IsInTransition(0))
        {
            photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
        }
	}
    //--------------------------------------------------------------------- ATTACK
    private void ATTACK ()
    {
        if (Input.GetKeyDown(KeyCode.R))
	    {
	    	photonView.RPC("RPCCrossFade", RpcTarget.All, "attack_charge", 0.1f);
		}
        if (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && _Animator.GetCurrentAnimatorStateInfo(0).IsName("attack_charge")
            && !_Animator.IsInTransition(0))
        {
            photonView.RPC("RPCCrossFade", RpcTarget.All, "attack", 0.6f);
        }
        if (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && _Animator.GetCurrentAnimatorStateInfo(0).IsName("attack")
            && !_Animator.IsInTransition(0))
        {
            photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
        }
    }
    //--------------------------------------------------------------------- SING
    private void SING ()
    {
        if(_Ctrl.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.X))
    	    {
	        	photonView.RPC("RPCCrossFade", RpcTarget.All, "sing", 0.1f);
		    }
            if (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
                && _Animator.GetCurrentAnimatorStateInfo(0).IsName("sing")
                && !_Animator.IsInTransition(0))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
    }
    //--------------------------------------------------------------------- LOOK_AROUND
    private void LOOK_AROUND()
    {
        if(_Ctrl.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.T))
    	    {
	        	photonView.RPC("RPCCrossFade", RpcTarget.All, "look_around", 0.1f);
		    }
            if (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
                && _Animator.GetCurrentAnimatorStateInfo(0).IsName("look_around")
                && !_Animator.IsInTransition(0))
            {
                photonView.RPC("RPCCrossFade", RpcTarget.All, "idle", 0.1f);
            }
        }
    }
    //--------------------------------------------------------------------- STOP
    private void STOP ()
    {
        if (Input.GetKeyDown(KeyCode.X)
            && !_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
	    {
            photonView.RPC("RPCCrossFade", RpcTarget.All, "down", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.X)
            && _Animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop")
            && !_Animator.IsInTransition(0))
	    {
            photonView.RPC("RPCCrossFade", RpcTarget.All, "jump", 0.1f);
            _MoveDirection.y = 3.0f;
            StartCoroutine(JumpPose());
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