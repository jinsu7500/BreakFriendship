using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;

    bool isGround;
    Vector3 curPos;
    int jumpCount = 0;

    void Awake()
    {
        // 닉네임 표시
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }

    void Update()
    {
        
        if (PV.IsMine)
        {
            // <-(-1 반환), ->(1 반환) 이동, 안누르면 0 반환
            float axis = Input.GetAxisRaw("Horizontal");
            // transform으로 하게되면 벽에 부딪칠 경우 벽을 뚫고 갈려고 함
            RB.velocity = new Vector2(4 * axis, RB.velocity.y);

            if (axis != 0)
            {
                AN.SetBool("isRun", true);
                PV.RPC("FilpXRPC", RpcTarget.AllBuffered, axis);// 재접속시 FilpX를 동기화해주기 위해서 AllBuffered
            }
            else
            {
                AN.SetBool("isRun", false);
            }

            // 점프, 바닥체크
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            
          
            AN.SetBool("isJump", !isGround);
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
            {
                Debug.Log(jumpCount);
                PV.RPC("JumpRPC", RpcTarget.All);
                jumpCount++;
                AN.SetBool("isDoubleJump", !isGround);
                Debug.Log(jumpCount);
            }
            if (isGround)
            {
                jumpCount = 0;
                AN.SetBool("isDoubleJump", false);
            }
            
            
           
        }

        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    [PunRPC]
    void FilpXRPC(float axis) => SR.flipX = axis == -1;// 왼쪽 키를 누를 경우 True 반환 오른쪽 키를 누르는 경우 False 반환

    [PunRPC]
    void JumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 500);
    }

    [PunRPC]
    void DoubleJumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 500);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
