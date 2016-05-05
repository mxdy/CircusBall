using UnityEngine;
using System.Collections;

public class OneWheel : Wheel {
    protected CircleCollider2D coll;              // 
    Transform wheelSprite;


    void Start()
    {
        coll = gameObject.GetComponent<CircleCollider2D>();
        wheelSprite = transform.Find("wheel_sprite");

        validPosY += transform.position.y + heightOffset;
        wheelType = ConstantEnum.WheelType.one_wheel;
    }

    void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));
        wheelSprite.Rotate(0, 0, angleA * velocity * Time.deltaTime, Space.Self);
    }

    public override void SetVelocity(float v)
    {
        velocity = v;
    }

    public override void ChangeDir()
    {
        SetVelocity(-velocity);
    }

    public override void DestoryMyself()
    {
        base.DestoryMyself();

        Destroy(gameObject);
    }

    public override void OnTriggerWheel(GameObject other)
    {
        base.OnTriggerWheel(other);

        if (other.gameObject.tag != "wheelOfMine" 
            && other.gameObject.tag != "Scenery"
            && other.gameObject.tag != "Player"
            && other.gameObject.tag != "Untagged"
            && gameObject.tag != "Untagged")
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
                    if (Mathf.Abs(other_wheel.velocity) <= Mathf.Abs(velocity))
                    {
                        ChangeDir();
                    }
                    else
                    {
                        other_wheel.ChangeDir();
                    }
                }
            }
        }
    }

    public override Vector2 GetTargetPos(Transform tran = null)
    {
        Vector2 v2;

        v2.x = transform.position.x;
        v2.y = transform.position.y + coll.radius * transform.localScale.y + tran.GetComponent<CircleCollider2D>().radius * tran.localScale.y;

        return v2;
    }
}
