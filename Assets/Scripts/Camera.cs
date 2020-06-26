using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Transform Player;   //玩家
    PlayCamera playerCamera;        //主相机
    Vector2 boxSize;            //视野范围
    public bool cameraMove;
    void Start() {
        cameraMove = false;
    }

    // Update is called once per frame
    void Update() {

        if (cameraMove)
        {
            FollowPlayer();
        }
        else
        {
            CheckBoundary();
        }

    }

    public void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(Player.position.x, Player.position.y + 2.0f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.08f);
        float distance = Vector3.Distance(targetPos,transform.position);
        if (distance<0.5f)
        {
            cameraMove = false;
        }
    }

    public void CheckBoundary()
    {
        float leftDistance = 0; //左右距离
        if (Player.position.x < transform.position.x) //在左边
        {
            leftDistance = transform.position.x - Player.position.x;

        }
        else
        {
            leftDistance = Player.position.x - transform.position.x;
        }
        if (leftDistance > boxSize.x * 2.0f)
        {
            Debug.Log("超出视野范围X轴");
            cameraMove = true;
        }

        float uDDistance = 0;   //上下距离

        if (Player.position.y < transform.position.y)
        {
            uDDistance = transform.position.y - Player.position.y;
        }
        else
        {
            uDDistance = Player.position.y - transform.position.y;
        }

        if (uDDistance > boxSize.y * 2.0f)
        {
            Debug.Log("超出视野范围Y轴");
            cameraMove = true;
        }
    }
}
