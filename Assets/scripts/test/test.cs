using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    Rigidbody2D rb2d;

    bool running = false;

    Vector3 endPos;     // 中间矫正经过的点临时存储
    Vector3 centerPos;  // 球的中心点
    float angleSpeed = 1f;  // 旋转速度（角度）
    float radius;   // 围绕半径
    bool isLeft = true; // 是否掉落在左边
    Transform target;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (running)
        {
            // 不管左边还是右边都向中间靠拢
            if (isLeft)
            {
                angleSpeed += 20 * Time.deltaTime * 4;
                if (angleSpeed >= 90f)
                {
                    angleSpeed = 90f;
                }
            }
            else
            {
                angleSpeed -= 20 * Time.deltaTime * 4;
                if (angleSpeed <= 90f)
                {
                    angleSpeed = 90f;
                }
            }

            float angle = angleSpeed * Mathf.Deg2Rad;

            endPos.x = target.position.x - Mathf.Cos(angle) * radius;

            endPos.y = centerPos.y + Mathf.Sin(angle) * radius;

            transform.position = endPos;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        rb2d.isKinematic = true;
        running = true;

        target = other.transform;

        centerPos = target.position;
        CircleCollider2D wheel = other.gameObject.GetComponent<CircleCollider2D>();
        CircleCollider2D player = gameObject.GetComponent<CircleCollider2D>();
        radius = wheel.radius * target.localScale.y + (player.radius - player.offset.y) * transform.localScale.y;

        other.transform.GetComponent<Rigidbody2D>().velocity = Vector2.right * 4;

        // 左边
        isLeft = (transform.position.x < centerPos.x) ? true : false;

        // 这个值应该是个小于90度的数
        angleSpeed = Mathf.Asin((transform.position.y - centerPos.y) / radius) * Mathf.Rad2Deg;

        if (!isLeft) angleSpeed = 180 - angleSpeed;
    }
}
