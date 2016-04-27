using UnityEngine;
using System.Collections;

public class OneWheel : Wheel {
    protected Rigidbody2D rb2d;                   // 轮子的刚体
    protected CircleCollider2D coll;              // 轮子的碰撞体

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<CircleCollider2D>();

        validPosY += transform.position.y + heightOffset;

        rb2d.velocity = new Vector2(velocity, 0);

        wheelType = ConstantEnum.WheelType.one_wheel;
    }

    public override void ChangeDir()
    {
        velocity *= -1;

        rb2d.velocity = new Vector2(velocity, 0);
    }

    public override void DestoryMyself()
    {
        base.DestoryMyself();

        Destroy(gameObject);
    }

    public override Vector2 GetTargetPos(Transform tran = null)
    {
        Vector2 v2;

        v2.x = transform.position.x;
        v2.y = transform.position.y + coll.radius * transform.localScale.y + tran.GetComponent<CircleCollider2D>().radius * tran.localScale.y;

        return v2;
    }
}
