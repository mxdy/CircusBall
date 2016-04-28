using UnityEngine;
using System.Collections;

public class FirstWheel : OneWheel {

	// Use this for initialization
 	void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<CircleCollider2D>();

        validPosY += transform.position.y + heightOffset;

        wheelType = ConstantEnum.WheelType.one_wheel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void ChangeDir()
    {
        SetVelocity(-velocity);
    }

    public override void SetVelocity(float v)
    {
        rb2d.velocity = new Vector2(v, 0);
    }
}
