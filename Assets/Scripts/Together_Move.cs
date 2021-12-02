using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Together_Move : MonoBehaviour
{
    public GameObject apple;
    public GameObject position;
    public PhotonView PV;

    [SerializeField]
    private float moveSpeed = 0.3f;
    [SerializeField]
    private float jumpSpeed = 0.3f;


    public void Move_to_Right()
    {
        float x = apple.transform.position.x + 0.01f;
        float y = apple.transform.position.y;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, moveSpeed);
        PV.RPC("Right_Update", RpcTarget.Others);

    }
    public void Move_to_Left()
    {
        float x = apple.transform.position.x - 0.01f;
        float y = apple.transform.position.y;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, moveSpeed);
        PV.RPC("Left_Update", RpcTarget.Others);
    }
    public void Move_to_Up()
    {
        float x = apple.transform.position.x;
        float y = apple.transform.position.y + 0.005f;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, jumpSpeed);
        PV.RPC("UP_Update", RpcTarget.Others);
    }

    [PunRPC]
    void Right_Update()
    {
        float x = apple.transform.position.x + 0.01f;
        float y = apple.transform.position.y;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, moveSpeed);
    }

    [PunRPC]
    void Left_Update()
    {
        float x = apple.transform.position.x - 0.01f;
        float y = apple.transform.position.y;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, moveSpeed);
    }

    [PunRPC]
    void UP_Update()
    {
        float x = apple.transform.position.x;
        float y = apple.transform.position.y + 0.005f;
        Vector2 vec = new Vector2(x, y);
        transform.position = Vector2.MoveTowards(transform.position, vec, jumpSpeed);
    }
    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.name == "Tilemap")
        {
            apple.transform.position = new Vector3(position.transform.position.x, position.transform.position.y, position.transform.position.z);
        }
    }
}
