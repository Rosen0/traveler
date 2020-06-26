using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_MovingObj : MonoBehaviour
{
    public float Move_Speed;
    [HideInInspector]
    public Vector3 pos;
    public Vector3 Target_pos;
    private Vector3 Vel;
    private Vector3 Origin_pos;
    private Vector3 Com;
    // Start is called before the first frame update
    void Start()
    {
        Origin_pos = transform.position;
        pos = transform.position;
        //target = new Vector3(1, 1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vel = Target_pos - Origin_pos;
        Vel = Vel.normalized;
        Vel = Vel * Move_Speed * Time.deltaTime;
        pos = pos + Vel;
        transform.position = pos;
        Com = pos - Target_pos;
        loop();
    }
    private void loop() 
    {
        if (Com.magnitude < 0.1)
        {
            pos = Target_pos;
            Target_pos = Origin_pos;
            Origin_pos = pos;
        }

    }
}
