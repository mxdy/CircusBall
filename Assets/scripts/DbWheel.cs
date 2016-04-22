using UnityEngine;
using System.Collections;

public class DbWheel : Wheel {
    public Rigidbody2D left_rb2d;        // 左轮子刚体
    public Rigidbody2D right_rb2d;        // 右轮子刚体

    void Start()
    {
        SetVelocity(velocity);

        validPosY += transform.position.y + heightOffset;

        wheelType = ConstantEnum.WheelType.db_wheel;
    }

    void SetVelocity(float v)
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
        base.ChangeDir();

        SetVelocity(-velocity);
    }

    public override Vector2 GetBirdPos(Transform tran = null)
    {
        Vector2 v2 = Vector2.zero;

        if (tran)
        {
            v2.x = tran.localPosition.x;
            v2.y = transform.localPosition.y + 1;
        }
        else
        {
            Debug.LogError("这个类型的轮子需要一个碰撞的物体!");
        }

        return v2;
    }
  }
