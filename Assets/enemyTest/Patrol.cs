using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//巡逻模式

public class Patrol : FSMState
{
    private CtrlState ctrlState;
    //路径点
    private List<Transform> mStargetPointTransform = new List<Transform>();
    //路径点索引
    private int mPointIndex = 0;
    //士兵
    private GameObject tmonster;
    //主角 
    private GameObject tPlayer;
    //巡逻路径点
    private GameObject tPoints;
    //士兵移动速度
    public float monsterSpeed = 1.0f;
    //获取动画
    private Animator ani;

   public  Patrol(CtrlState _ctrlstate, GameObject tmonster)
    {
        ctrlState = _ctrlstate;
        this.tmonster = tmonster;
    }
 
    public override void Start()
    {
        //获取主角对象
        tPlayer = GameObject.Find("Player");
        tPoints = tmonster.transform.parent.Find("Points").gameObject;
        ani = tmonster.GetComponent<Animator>();
        //获取路径点
        Transform[] transforms = tPoints.GetComponentsInChildren<Transform>();

        foreach (var m_transform in transforms)
        {

            if (m_transform != tPoints.transform)
            {
                mStargetPointTransform.Add(m_transform);
                Debug.Log("点的位置为" + m_transform.position);
            }
        }

        
        
    }

    public override void Update()
    {
        float step = monsterSpeed * Time.deltaTime;

        tmonster.transform.position = Vector3.MoveTowards(tmonster.transform.position,
            this.mStargetPointTransform[this.mPointIndex].position, step);
       
        if (Vector3.Distance(tmonster.transform.position, this.mStargetPointTransform[this.mPointIndex].position) < 0.5f)
        {
            ChangeFace();
            //切换目标点
            this.mPointIndex++;
            if (this.mPointIndex >= this.mStargetPointTransform.Count)
            {
                this.mPointIndex = 0;
                Debug.Log("目标点" + mPointIndex);
            }
        }

        if (Vector3.Distance(tPlayer.transform.position, tmonster.transform.position) <= 5.0f)
        {
            ani.SetTrigger("SeePlayer");
            
            ChangeMove();
        }

    }
    public void ChangeFace()
    {
        //bug1（修复）
        //当玩家跑到怪物右边，怪物调头触发目标点再次调头
        if (tmonster.transform.position.x - tPlayer.transform.position.x < 0)
        {
            if (tmonster.transform.position.x - this.mStargetPointTransform[0].position.x > 0)
            {
                tmonster.transform.localScale = new Vector3(-3, 3, 1);
            }
        }
        //碰到目标点调头
        if (tmonster.transform.localScale.x == -3)
        {
            tmonster.transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            tmonster.transform.localScale = new Vector3(-3, 3, 1);
        }
    }
    public void ChangeMove()
    {
        ctrlState.SetEnemyState(new Chase(ctrlState,tmonster));
    }

    public override void End()
    {

    }


}


