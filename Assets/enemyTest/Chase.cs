using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追逐模式
public class Chase : FSMState
{
    public float monsterSpeed = 4.0f;
    private CtrlState ctrlState;
    //士兵
    private GameObject tmonster;
    //主角 
    private GameObject tPlayer;
    //获取动画
    private Animator ani;

    public Chase(CtrlState _ctrlstate, GameObject tmonster)
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
        if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) <= 5.0f)
        {
            //转向
            if (tmonster.transform.position.x - tPlayer.transform.position.x >= 0.0f)
            {
                tmonster.transform.localScale = new Vector3(3, 3, 1);
            }
            else
            {
                tmonster.transform.localScale = new Vector3(-3, 3, 1);
            }

            var step = monsterSpeed * Time.deltaTime;
            
            tmonster.transform.position = Vector3.MoveTowards(tmonster.transform.position, tPlayer.transform.position, step);

            if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) <= 1.0f)
            {
                //攻击模式
                ChangeAttack();
            }
        }
        if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) >= 5.0f)
        {
            //转向
            if (tmonster.transform.localScale.x == 3)
            {
                tmonster.transform.localScale = new Vector3(-3, 3, 1);
            }
            else
            {
                tmonster.transform.localScale = new Vector3(3, 3, 1);
            }
            //巡逻模式
            ChangeDistance();
        }
        
    }

    public void ChangeAttack()
    {
        ctrlState.SetEnemyState(new Attack(ctrlState, tmonster));
    }
    public void ChangeDistance()
    {
        ctrlState.SetEnemyState(new Patrol(ctrlState, tmonster));
    }
    public override void End()
    {

    }
}
