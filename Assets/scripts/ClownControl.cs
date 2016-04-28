using UnityEngine;
using System.Collections;
using Spine.Unity;

public class ClownControl : MonoBehaviour {

    public float velocity = 3;  // 前进速度
    public float upForce = 20;  // 起跳力度
    private Rigidbody2D rb2d;   //  主角的刚体
    private bool isJp = false;  // 是否起跳
    private bool isAir = true; // 是否还在空中
    private Wheel targetWheel;  // 当前掉落的轮子
    private SkeletonAnimation skelAni;  // spine动画数据

    Vector3 endPos;     // 中间矫正经过的点临时存储
    Vector3 centerPos;  // 球的中心点
    float angleSpeed = 1f;  // 旋转速度（角度）
    float radius;   // 围绕半径
    bool isLeft = true; // 是否掉落在左边
    CircleCollider2D plyerCollider; // 角色的碰撞体

    bool isOnPlan = true;

    const float CHANGEOFFSET = 15;

    ConstantEnum.WheelType wheelType = ConstantEnum.WheelType.one_wheel;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        plyerCollider = gameObject.GetComponent<CircleCollider2D>();

        skelAni = gameObject.GetComponent<SkeletonAnimation>();

        GameControlScript.current.clown = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameControlScript.current.isGameOver && !isAir)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.touchCount > 0)
            {
                isJp = true;

                if (targetWheel)
                {
                    targetWheel.SetVelocity(-8);
                }

                if (isOnPlan) isOnPlan = false;
            }
        }
    }

    void FixedUpdate()
    {
        // jump
        if (isJp)
        {
            isJp = false;
            isAir = true;

            rb2d.isKinematic = false;
            rb2d.velocity = new Vector2(velocity, 0);
            rb2d.AddForce(new Vector2(0, upForce));

            skelAni.state.SetAnimation(0, "up", false);

            rb2d.gravityScale = 1;
        }

        // after jump
        if (!isAir)
        {
            // 矫正角色位置前需要关掉重力
            rb2d.gravityScale = 0;

            switch (wheelType)
            {
                case ConstantEnum.WheelType.one_wheel:
                    // 不管左边还是右边都向中间靠拢
                    if (isLeft)
                    {
                        angleSpeed += CHANGEOFFSET * Time.deltaTime * 4;
                        if (angleSpeed >= 90f)
                        {
                            angleSpeed = 90f;
                        }
                    }
                    else
                    {
                        angleSpeed -= CHANGEOFFSET * Time.deltaTime * 4;
                        if (angleSpeed <= 90f)
                        {
                            angleSpeed = 90f;
                        }
                    }

                    float angle = angleSpeed * Mathf.Deg2Rad;


                    endPos.x = targetWheel.transform.position.x - Mathf.Cos(angle) * radius;
                    endPos.y = centerPos.y + Mathf.Sin(angle) * radius;
                    break;
                case ConstantEnum.WheelType.db_wheel:
                    endPos = targetWheel.GetTargetPos(transform);
                    endPos.y -= plyerCollider.radius * transform.localScale.y;
                    break;
                default:
                    break;
            }

            transform.position = endPos;
        }
    }

    // 小鸟的碰撞
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!GameControlScript.current.isGameOver)
        {
            if (other.gameObject.tag == "Scenery")
            {
                // 掉到地上
                birdDie();
            }
            else if (other.gameObject.tag == "oneWheel")
            {
                wheelType = ConstantEnum.WheelType.one_wheel;

                CorrectBird(other.gameObject);
            }
            else if (other.gameObject.tag == "dbWheel")
            {
                wheelType = ConstantEnum.WheelType.db_wheel;

                // 掉落到单轮子上
                if (other.gameObject.GetComponent<DbWheel>())
                {
                    other.gameObject.GetComponent<DbWheel>().SetTargetPosXDis(transform);
                }

                // 掉落到两个轮子上
                ChangeTarget(other.gameObject);
            }
        }
    }

    // 小鸟的矫正 小球，中球，大球跳上去都需要矫正
    void CorrectBird(GameObject other)
    {
        // 掉落高度不够的话也判定死亡
        float valid_y = other.gameObject.GetComponent<Wheel>().validPosY;

        Debug.Log(valid_y);
        Debug.Log(transform.position.y);

        if (valid_y < transform.position.y)
        {
            // 改变目标状态
            ChangeTarget(other.gameObject);

            // 获得目标轮子的中心坐标
            centerPos = other.transform.position;

            CircleCollider2D circle = other.gameObject.GetComponent<CircleCollider2D>();

            // 赋值半径
            radius = circle.radius * other.transform.localScale.y + (plyerCollider.radius - plyerCollider.offset.y) * transform.localScale.y;

            // 判断跳落的点是左边还是右边
            isLeft = (transform.position.x < centerPos.x) ? true : false;

            // 这个值应该是个小于90度的数
            angleSpeed = Mathf.Asin((transform.position.y - centerPos.y) / radius) * Mathf.Rad2Deg;

            // 左右跳落角度需要处理
            if (!isLeft) angleSpeed = 180 - angleSpeed;
        }
        //else
        //{
        //    birdDie();
        //}
    }

    void ChangeTarget(GameObject other)
    {
        // 新的目标轮子
        targetWheel = other.gameObject.GetComponent<Wheel>();

        // 轮子改变方向
        targetWheel.GetComponent<Wheel>().ChangeDir();

        // 分数加一
        GameControlScript.current.BirdScored();

        // 设置成自己脚下的轮子
        other.gameObject.tag = "wheelOfMine";

        isAir = false;

        // 改成走的动画
        Spine.TrackEntry cur_ani = skelAni.state.SetAnimation(0, "donw", false);
        cur_ani.Complete += delegate {
            skelAni.state.SetAnimation(0, "go", true);
        };

    }

    void birdDie()
    {
        // 死亡
        GameControlScript.current.BirdDied();

        GameControlScript.current.isGameOver = true;

        skelAni.state.SetAnimation(0, "donwdie", false).time = 9f/30f;

        rb2d.isKinematic = true;
    }

    }
