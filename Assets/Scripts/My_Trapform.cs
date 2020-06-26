using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_Trapform : MonoBehaviour
{
    private int flag;
    private int t;
    public int Time;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform.GetComponent<Renderer>().enabled = true;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        flag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == 1) 
        {
            t += 1;
            if (t == Time - 20) 
            {
                rigid.constraints = RigidbodyConstraints2D.None;
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (t == Time)
            {
                transform.GetComponent<Renderer>().enabled = false;
                transform.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (t >= Time + 1000)
            {
                transform.GetComponent<Renderer>().enabled = true;
                transform.GetComponent<BoxCollider2D>().enabled = true;
                flag = 0;
                t = 0;
            }
            Debug.Log(t);
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(1);
        if (flag == 0) 
        {
            flag = 1;
        }
        
    }
}
