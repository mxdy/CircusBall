using UnityEngine;
using System.Collections;

public class DbWheel : Wheel {
    private Transform left_wheel;        // 左轮子
    private Transform right_wheel;       // 右轮子
    private Transform body_wheel;        // 身体
    private BoxCollider2D body_box;      // 身体boxcollider

    public float targetLocalDistance;    // 目标到轮子中心的水平距离

    void Start()
    {
        right_wheel = transform.Find("right_wheel");
        left_wheel = transform.Find("left_wheel");
        body_wheel = transform.Find("board");

        body_box = transform.GetComponent<BoxCollider2D>();

        validPosY += transform.position.y + heightOffset;
        wheelType = ConstantEnum.WheelType.db_wheel;
    }

    void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();

        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));

        right_wheel.Rotate(0, 0,  angleA * velocity * Time.deltaTime, Space.Self);
        left_wheel.Rotate(0, 0, angleA * velocity * Time.deltaTime, Space.Self);
    }

    public override void SetVelocity(float v)
    {
        velocity = v;
    }

    public override void DestoryMyself()
    {
        base.DestoryMyself();

        Destroy(transform.parent.gameObject);
    }

    public override void ChangeDir()
    {
        SetVelocity(-velocity);
    }

    public override void OnTriggerWheel(GameObject other)
    {
        base.OnTriggerWheel(other);

        if (other.gameObject.tag != "wheelOfMine"
            && other.gameObject.tag != "Scenery"
            && other.gameObject.tag != "Player")
        {
            Wheel other_wheel = other.gameObject.GetComponent<Wheel>();

            // 还没有检测
            if (!isChecked)
            {
                isChecked = true;
                other_wheel.isChecked = true;

                // 反向
                if (other_wheel.velocity * velocity <= 0)
                {
                    ChangeDir();
                    other_wheel.ChangeDir();
                }
                else // 同向
                {
                    if (Mathf.Abs(other_wheel.velocity) >= Mathf.Abs(velocity))
                    {
                        other_wheel.ChangeDir();
                    }
                    else
                    {
                        ChangeDir();
                    }
                }
            }
            else
            {
                ChangeDir();
            }
        }
    }

    public override Vector2 GetTargetPos(Transform tran = null)
    {
        Vector2 v2 = Vector2.zero;

        if (tran)
        {
            v2.x = body_wheel.position.x - targetLocalDistance;
            v2.y = body_wheel.position.y + body_box.size.y * body_wheel.localScale.y + tran.GetComponent<CircleCollider2D>().radius;
        }
        else
        {
            Debug.LogError("这个类型的轮子需要一个碰撞的物体!");
        }

        return v2;
    }

    public void SetTargetPosXDis(Transform tran)
    {
        targetLocalDistance = body_wheel.position.x - tran.position.x;
    }
  }
