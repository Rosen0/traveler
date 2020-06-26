using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class monster : MonoBehaviour
{
    private CtrlState ctrlstate;
    
    //public void OnTriggerEnter2D(Collider2D col)
    //{
    //    Debug.Log("123");
    //    if (col.tag == "Player")
    //    {
    //        Debug.Log("怪物碰到了主角");
    //    }
    //    else
    //    {
    //        Debug.Log("NULL");
    //    }
    //}

 

    void Start()
    {
        ctrlstate = new CtrlState();
        Patrol Monster = new Patrol(ctrlstate,this.gameObject);
        ctrlstate.SetEnemyState(Monster);
    }

    void Update()
    {
        
        ctrlstate.UpdateEnemyState();
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("attack"))
        {
            Destroy(gameObject);
        }
    }
}
