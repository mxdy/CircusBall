using UnityEngine;
using System.Collections;

public class DbWheel : Wheel {
    public Rigidbody2D left_rb2d;        // 左轮子刚体
    public Rigidbody2D right_rb2d;       // 右轮子刚体
    public Rigidbody2D body_rb2d;        // 身体刚体
    private BoxCollider2D body_box;      // 身体boxcollider

    public float targetLocalDistance;    // 目标到轮子中心的水平距离

    void Start()
    {
        SetVelocity(velocity);

        validPosY += transform.position.y + heightOffset;

        wheelType = ConstantEnum.WheelType.db_wheel;

        body_rb2d = gameObject.GetComponent<Rigidbody2D>();
        body_box = gameObject.GetComponent<BoxCollider2D>();
    }

    public override void SetVelocity(float v)
    {
        Vector2 vel = new Vector2(v, 0);
        left_rb2d.velocity = vel;
        right_rb2d.velocity = vel;
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

    public override Vector2 GetTargetPos(Transform tran = null)
    {
        Vector2 v2 = Vector2.zero;

        if (tran)
        {

            v2.x = transform.position.x - targetLocalDistance;
            v2.y = transform.position.y + (body_box.size.y- body_box.offset.y) * transform.localScale.y;
        }
        else
        {
            Debug.LogError("这个类型的轮子需要一个碰撞的物体!");
        }

        return v2;
    }

    public void SetTargetPosXDis(Transform tran)
    {
        targetLocalDistance = transform.position.x - tran.position.x;
    }
  }
