using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 玩家现在的方向
/// </summary>
public enum PlayDir
{
    Left,
    Right = 1 ,
}

public class MoveController : MonoBehaviour {

    public float speed;         //速度
    [Header("距离地面的最小高度")]
    public float distance;      //距离地面的高度
    public Vector3 moveSpeed;   //每一帧的移动速度
    public PlayDir nowDir;      //现在的玩家的方向
    public Animator playAnimator;

    public float gravity;       //受到的重力
    public bool gravityEnable;  //重力开关
    public bool inputEnable;    //接受输入开关  true 游戏接受按键输入  false不接受按键输入
    public float jumpPower;     //向上跳跃的力
    public bool isGround;       //是否在地面  true在地面 false不在地面
    public float jumpTime;      //跳跃的蓄力时间
    public float recoilForce;   //攻击后坐力
    public int jumpCount;       //跳跃次数
    public bool isAlive;        //是否生存
    public bool isClimb;        //爬墙状态
    public bool jumpState;      // 跳跃状态
    public float sprintTime;    //冲刺次数
    public string sceneName;
    public bool isCanSprint;
    //float timeJump;             //跳跃当前的蓄力时间

    public Vector2 boxSize;
    int playerLayerMask;



    GameObject knifeEffectOne;  
    GameObject knifeEffectTwo;  
    GameObject knifeEffectUp;  
    GameObject knifeEffectDown;  //刀光特效物体
    public Image dieMaskImage; 
    public Vector3 startPoint;
    void Start()
    {
        isAlive = true;
        nowDir = PlayDir.Left;
        //boxSize = new Vector2(0.4f, 1.0f);    //设置盒子射线的大小
        startPoint = transform.position;


        playAnimator = GetComponent<Animator>();
        knifeEffectOne = transform.Find("LRAttackImage").gameObject;
        knifeEffectTwo = transform.Find("LRAttackImage2").gameObject;
        knifeEffectUp = transform.Find("UDAttackImage (1)").gameObject;
        knifeEffectDown = transform.Find("UDAttackImage").gameObject;
        knifeEffectOne.SetActive(false);
        knifeEffectTwo.SetActive(false);
        knifeEffectUp.SetActive(false);
        knifeEffectDown.SetActive(false);   //初始化刀光物体，并关闭

        gravityEnable = true;
        inputEnable = true;
        jumpState = false;

        isCanSprint = true;   //状态初始化

        playerLayerMask = LayerMask.GetMask("Player");
        playerLayerMask = ~playerLayerMask;             //获得当前玩家层级的mask值，并使用~运算，让射线忽略玩家层检测
    }

    void Update() {
        if (!isAlive)
        {
            return; //  死亡不进行任何操作
        }
        LRMove();
        UpdateAnimtorState();
        UDMpve();
        Jump();
        AttackFunc();
        SprintFunc();


        playAnimator.SetBool("IsGround",isGround);
        CheckNextMove();
    }

    ///根据落地状态更新动画以及玩家的状态信息
    public void UpdateAnimtorState()
    {
        if (isGround)
        {
            playAnimator.SetBool("IsJump", false);
            playAnimator.ResetTrigger("IsJumpTwo");
            jumpCount = 0;
            playAnimator.SetBool("IsDown", false);
            jumpState = false;
            if (isClimb)
            {
                isClimb = false;
            }
            isCanSprint = true;
        }
        else
        {
            if (!jumpState)
            {
                playAnimator.SetBool("IsDown", true);
            }
            else
            {
                playAnimator.SetBool("IsDown", false);
            }
            if (isClimb)
            {
                jumpCount = 0;  //跳跃次数重置
            }
        }
    }

    /// 左右移动
    public void LRMove()
    {
        if (!inputEnable)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");
        moveSpeed.x = h * speed;

        if (!isClimb)
        {
            DirToRotate();
        }

        if (h == 0)//停止按键输入
        {
            playAnimator.SetTrigger("stopTrigger");
            playAnimator.ResetTrigger("IsRotate");
            playAnimator.SetBool("IsRun", false);
        }
        else
        {
            playAnimator.ResetTrigger("stopTrigger");
        }
    }

    /// 根据方向进行旋转
    public void DirToRotate()
    {
        if (nowDir == PlayDir.Left && moveSpeed.x > 0)
        {
            transform.Rotate(0, 180, 0);
            nowDir = PlayDir.Right;
            if (isGround)
            {
                playAnimator.SetTrigger("IsRotate");
            }

        }
        else if (nowDir == PlayDir.Right && moveSpeed.x < 0)
        {
            transform.Rotate(0, -180, 0);
            nowDir = PlayDir.Left;
            if (isGround)//在地面才播放转向动画
            {
                playAnimator.SetTrigger("IsRotate");
            }
        }
        else if (nowDir == PlayDir.Right && moveSpeed.x > 0)
        {
            playAnimator.SetBool("IsRun", true);
        }
        else if (nowDir == PlayDir.Left && moveSpeed.x < 0)
        {
            playAnimator.SetBool("IsRun", true);
        }

    }

    /// 重力更新
    public void UDMpve()
    {
        if (!gravityEnable)
        {
           // moveSpeed.y = 0;
            return;
        }

        if (isGround)   //在地面
        {
            moveSpeed.y = 0;
        }
        else
        {
            if (isClimb)
            {
                moveSpeed.y = 0;
            }
            else
            {
                moveSpeed.y += -1 * gravity * Time.deltaTime;
            }

        }
    }


    /// 跳跃
    public void Jump()
    {
        if (!inputEnable)
        {
            return;
        }
        if (isClimb && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ClimpJumpMove());
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpState = true;
            if (!isGround && jumpCount < 2)
            {
                jumpCount = 2;
            }
            else
            {
                jumpCount++;
            }

            if (jumpCount == 1)
            {
                moveSpeed.y += jumpPower;
                playAnimator.SetBool("IsJump", true);   //播放一段跳动画
            }
            else if (jumpCount == 2)
            {
                playAnimator.SetTrigger("IsJumpTwo");    //播放二段跳动画
                moveSpeed.y = jumpPower;
            }
            //timeJump = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpState = false;
            //timeJump = 0;
        }

        //进入上跳减速状态，但还在上升
        if (moveSpeed.y > 0 && moveSpeed.y < 1.5f)
        {
            playAnimator.SetBool("IsSlowUp", true);
        }
        else
        {
            playAnimator.SetBool("IsSlowUp", false);
        }

        //进入下落状态
        if (moveSpeed.y < 0)
        {

            playAnimator.SetBool("IsStopUp", true);
        }
        else
        {
            playAnimator.SetBool("IsStopUp", false);

        }
    }


    //爬墙转向
    public void ClimpRotate()
    {
        if (nowDir == PlayDir.Left)
        {
            nowDir = PlayDir.Right;
            transform.Rotate(0, 180, 0);
        }
        else
        {
            nowDir = PlayDir.Left;
            transform.Rotate(0, -180, 0);
        }
    }


    /// 攻击函数
    public void AttackFunc()
    {
        if (!inputEnable || isClimb)    //爬墙状态与无法按键获取时  无法攻击
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Input.GetKey(KeyCode.W))    //  向上攻击
            {
                playAnimator.SetTrigger("IsAttackUp");
                StartCoroutine(LookKnifeObj(knifeEffectUp,3));
                CheckAckInteractive(3);
            }
            else if (Input.GetKey(KeyCode.S) && !isGround) //  向上攻击且不在地面
            {
                playAnimator.SetTrigger("IsAttackDown");
                StartCoroutine(LookKnifeObj(knifeEffectDown, 3));
                CheckAckInteractive(4);
            }
            else    //左右攻击
            {
                playAnimator.SetTrigger("IsAttackLR1");
                StartCoroutine(LookKnifeObj(knifeEffectOne, 4));
                CheckAckInteractive((int)nowDir);
            }
        }
    }

    public void CheckAckInteractive(int dir)
    {
        float distance = 1.8f;          //射线的检测长度
        RaycastHit2D hit2D = new RaycastHit2D();
        Vector2 raySize = new Vector2(boxSize.x + 0.5f, boxSize.y);         //扩大检测X轴范围
        switch (dir)
        {
            case 1:
                hit2D = Physics2D.BoxCast(transform.position, raySize, 0, Vector2.left, distance, playerLayerMask);
                break;
            case 2:
                hit2D = Physics2D.BoxCast(transform.position, raySize, 0, Vector2.right, distance, playerLayerMask);
                break;
            case 3:
                hit2D = Physics2D.BoxCast(transform.position, raySize, 0, Vector2.up, distance, playerLayerMask);
                break;
            case 4:
                hit2D = Physics2D.BoxCast(transform.position, raySize, 0, Vector2.down, distance, playerLayerMask);
                break;
        }

        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.CompareTag("Trap"))   //如果是陷阱就有后坐力
            {
                AttackRestState();
                StartCoroutine(InteractiveMove(dir, 10));
            }
        }
    }


    public void Die()
    {
        //碰到陷阱就死亡  就是少血
        isAlive = false;
        Relife();

       
    }

    public void Relife()
    {
        SceneManager.LoadScene(sceneName);
        transform.position = startPoint;
        isAlive = true;
    }



    /// 攻击重置动作的相关状态
    public void AttackRestState()
    {
        jumpCount = 0;
        isCanSprint = true; //重置重置状态
    }





    /// 冲刺函数
    public void SprintFunc()
    {
        if (!inputEnable)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.J) && isCanSprint)
        {
            if (isClimb)
            {
                ClimpRotate();  //如果是爬墙状态冲刺，先转向在进行冲刺
            }
            StartCoroutine(SprintMove(sprintTime));
            playAnimator.SetTrigger("IsSprint");//播放冲刺动画
            isCanSprint = false;
        }
    }


    IEnumerator SprintMove(float time)
    {
        inputEnable = false;
        gravityEnable = false;
        moveSpeed.y = 0;
        if (nowDir == PlayDir.Left)
        {
            moveSpeed.x = 15*-1;
        }
        else
        {
            moveSpeed.x = 15;
        }

        yield return new WaitForSeconds(time);
        inputEnable = true;
        gravityEnable = true;

    }


    /// 检测是否在地面
    public bool CheckIsGround()
    {
        //float aryDistance = boxSize.y * 0.5f + 0.1f;
        RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, 5f, playerLayerMask);
        if (hit2D.collider != null)
        {


            float tempDistance = Vector3.Distance(transform.position, hit2D.point);
            if (tempDistance > (boxSize.y * 0.5f + distance))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    /// 检测下一帧的位置是否能够移动，并进行修正
    public void CheckNextMove()
    {
        Vector3 moveDistance = moveSpeed * Time.deltaTime;
        int dir = 0;//确定下一帧移动的左右方向
        if (moveSpeed.x > 0)
        {
            dir = 1;
        }
        else if (moveSpeed.x < 0)
        {
            dir = -1;
        }
        else
        {
            dir = 0;
        }
        if (dir != 0)//当左右速度有值时
        {
            RaycastHit2D lRHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.right * dir, 5.0f, playerLayerMask);
            if (lRHit2D.collider != null)//如果当前方向上有碰撞体
            {
                float tempXVaule = (float)Math.Round(lRHit2D.point.x, 1);                   //取X轴方向的数值，并保留1位小数精度。防止由于精度产生鬼畜行为
                Vector3 colliderPoint = new Vector3(tempXVaule, transform.position.y);      //重新构建射线的碰撞点
                float tempDistance = Vector3.Distance(colliderPoint, transform.position);   //计算玩家与碰撞点的位置
                if (tempDistance > (boxSize.x * 0.5f + distance))   //如果距离大于 碰撞盒子的高度的一半+最小地面距离
                {
                    transform.position += new Vector3(moveDistance.x, 0, 0);
                    if (isClimb)        //如果左右方向没有碰撞体了，退出爬墙状态
                    {
                        isClimb = false;
                        playAnimator.ResetTrigger("IsClimb"); //重置触发器  退出
                        playAnimator.SetTrigger("exitClimp");
                    }
  
                }
                else//如果距离小于  根据方向进行位移修正
                {

                    float tempX = 0;//新的X轴的位置
                    if (dir > 0)
                    {
                        tempX = tempXVaule - boxSize.x * 0.5f - distance + 0.01f;
                    }
                    else
                    {
                        tempX = tempXVaule + boxSize.x * 0.5f + distance - 0.01f;
                    }
                    if (lRHit2D.collider.CompareTag("Monster"))
                    {
                        Die();
                    }
                    transform.position = new Vector3(tempX, transform.position.y, 0);//修改玩家的位置
                    if (!lRHit2D.collider.CompareTag("Trap"))    //如果左右不是陷阱
                    {
                        EnterClimpFunc(transform.position); //检测当前是否能够进入爬墙状态
                        playAnimator.ResetTrigger("exitClimp");
                    }
                    else
                    {
                        Die();
                    }

                }

            }
            else
            {
                transform.position += new Vector3(moveDistance.x, 0, 0);
                if (isClimb)
                {
                    isClimb = false;
                    playAnimator.SetTrigger("exitClimp");
                    playAnimator.ResetTrigger("IsClimb"); //重置触发器  退出
                }


            }
        }
        //更新方向信息，上下轴
        if (moveSpeed.y > 0)
        {
            dir = 1;
        }
        else if (moveSpeed.y < 0)
        {
            dir = -1;
        }
        else
        {
            dir = 0;
        }
        //上下方向进行判断
        if (dir != 0)
        {
            RaycastHit2D uDHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector3.up * dir, 5.0f, playerLayerMask);
            if (uDHit2D.collider != null)
            {
                float tempYVaule = (float)Math.Round(uDHit2D.point.y, 1);
                Vector3 colliderPoint = new Vector3(transform.position.x, tempYVaule);
                float tempDistance = Vector3.Distance(transform.position, colliderPoint);

                if (tempDistance > (boxSize.y * 0.5f + distance))
                {

                    float tempY = 0;
                    float nextY = transform.position.y + moveDistance.y;
                    if (dir > 0)
                    {
                        tempY = tempYVaule - boxSize.y * 0.5f - distance;
                        if (nextY > tempY)
                        {
                            transform.position = new Vector3(transform.position.x, tempY+0.1f, 0);
                        }
                        else
                        {
                            transform.position += new Vector3(0, moveDistance.y, 0);
                        }
                    }
                    else
                    {
                        tempY = tempYVaule + boxSize.y * 0.5f + distance;
                        if (nextY < tempY)
                        {
                            transform.position = new Vector3(transform.position.x, tempY-0.1f, 0); 
                        }
                        else
                        {
                            transform.position += new Vector3(0, moveDistance.y, 0);
                        }
                    }
                    isGround = false;   //更新在地面的bool值
                }
                else
                {
                    float tempY = 0;
                    if (dir > 0)//如果是朝上方向移动，且距离小于规定距离，就说明玩家头上碰到了物体，反之同理。
                    {
                        tempY = uDHit2D.point.y - boxSize.y * 0.5f - distance + 0.05f;
                        isGround = false;
                    }
                    else
                    {
                        tempY = uDHit2D.point.y + boxSize.y * 0.5f + distance - 0.05f;
                        isGround = true;
                    }
                    moveSpeed.y = 0;
                    transform.position = new Vector3(transform.position.x, tempY, 0);
                    if (uDHit2D.collider.CompareTag("Monster"))
                    {
                        Die();
                    }
                    if (uDHit2D.collider.CompareTag("Trap"))    
                    {
                        Die();

                    }

                }

            }
            else
            {
                isGround = false;
                transform.position += new Vector3(0, moveDistance.y, 0);


            }

        }
        else
        {
            isGround = CheckIsGround();//更新在地面的bool值

        }
    }




    //爬墙
    public void EnterClimpFunc(Vector3 rayPoint)
    {
        //设定碰到墙 且  从碰撞点往下 玩家碰撞盒子高度内  没有碰撞体  就可进入碰撞状态。
        //RaycastHit2D hit2D = Physics2D.BoxCast(rayPoint, boxSize, 0, Vector2.down, boxSize.y, playerLayerMask);
        if (Input.GetKeyDown(KeyCode.L))
        {
             playAnimator.SetTrigger("IsClimb");//动画切换
             isClimb = true;
             isCanSprint = true; //爬墙状态，冲刺重置
        }

        
    }

    // 爬墙
    IEnumerator ClimpJumpMove()
    {
        inputEnable = false;    //此时不接受其余输入
        gravityEnable = false;
        isClimb = false;
        playAnimator.SetTrigger("IsStopClimpJump");

        playAnimator.ResetTrigger("IsClimb");
        if (nowDir == PlayDir.Left)
        {
            moveSpeed.x = 10;
        }
        else
        {
            moveSpeed.x = -10;
        }

        moveSpeed.y = 6;
        yield return new WaitForSeconds(0.15f);
        inputEnable = true;
        gravityEnable = true;

    }

    /// 显示隐藏的刀光特效物体
    IEnumerator LookKnifeObj(GameObject knifeObj, int frameCount)
    {
        knifeObj.SetActive(true);
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }
        knifeObj.SetActive(false);
    }


    //攻击后退效果
    IEnumerator InteractiveMove(int dir, int frameCount)
    {
        inputEnable = false;
        for (int i = 0; i < frameCount; i++)
        {
            switch (dir)
            {
                case 1:
                    moveSpeed.x = recoilForce;
                    break;
                case 2:
                    moveSpeed.x = -recoilForce;
                    break;
                case 3:
                    moveSpeed.y = -recoilForce;
                    break;
                case 4:
                    moveSpeed.y = recoilForce;
                    break;
            }

            yield return null;
        }
        inputEnable = true;
        yield return null;
    }



 

}
