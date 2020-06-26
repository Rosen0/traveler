using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCamera : MonoBehaviour
{

    public Transform player;   //玩家
    public bool cameraMoveX;

    public Vector3 startPoint;
    public Vector3 endPoint;
    void Start()
    {
        cameraMoveX = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (cameraMoveX)
        {
            FollowPlayerX();
        }

        else
        {
            Check();
        }
    }

    public void FollowPlayerX()
    {
        if (transform.position.x < startPoint.x)
        {
            cameraMoveX = false;
        }
        else if (transform.position.x > endPoint.x)
        {
            cameraMoveX = false;
        }

        Vector3 targetPos = new Vector3(player.position.x,transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.08f);

    }


    public void Check()
    {
        if (player.position.x >= startPoint.x && player.position.x <= endPoint.x)
        {
            cameraMoveX = true;
        }
        else
        {
            cameraMoveX = false;
        }




    }

}
