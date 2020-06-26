using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//攻击模式
public class Attack : FSMState
{
    private CtrlState ctrlState;
    //士兵
    private GameObject tmonster;
    //主角 
    private GameObject tPlayer;
    //获取动画
    private Animator ani;

    private int monsterSpeed = 0;

    public Attack(CtrlState _ctrlstate, GameObject tmonster)
    {
        ctrlState = _ctrlstate;
        this.tmonster = tmonster;
    }

     public override void Start()
     {
         //获取主角对象
         tPlayer = GameObject.Find("Player");
         ani = tmonster.GetComponent<Animator>();
     }
    public override void Update()
    {
        if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) <= 2.0f)
        {
            ani.SetTrigger("Attack");
        }
        if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) <= 5.0f)
        {
            ChangeState();
        }
    }
    public void ChangeState()
    {
        ctrlState.SetEnemyState(new Patrol(ctrlState,tmonster));
    }
    public override void End()
    {

    }

}
