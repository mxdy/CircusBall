using UnityEngine;
using System.Collections;

public class ClownControl : MonoBehaviour {

    public float velocity = 3;
    public float upForce = 20;

    private Rigidbody2D rb2d;

    private bool isJp = false;
    private bool isAir = false;

    private Vector2 standPoint;

    private Transform targetWheel;
    private Vector3 targetPos;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        targetWheel = GameObject.Find("wheel_first").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameControlScript.current.isGameOver && !isAir)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isJp = true;

                targetWheel.GetComponent<Wheel>().DestoryMyself();
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
        }

        // after jump
        if (!isAir)
        {
            rb2d.isKinematic = true;
            targetPos = targetWheel.transform.position;
            targetPos.y += 1.6f;
            transform.position = targetPos;
        }
    }

    // 小鸟的碰撞
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!GameControlScript.current.isGameOver)
        {
            if (other.gameObject.tag == "Scenery")
            {
                birdDie();
            }
            else if(other.gameObject.tag == "wheel")
            {
                CorrectBird(other.gameObject);
            }
        }
    }

    // 小鸟的矫正
    void CorrectBird(GameObject other)
    {
        // 掉落高度不够的话也判定死亡
        float valid_y = other.gameObject.GetComponent<Wheel>().validPosY;

        Debug.Log("wheel valid y pos:" + valid_y);
        Debug.Log("bird y pos:" + transform.position.y);
        Debug.Log("sprite height:" + gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

        if (valid_y < transform.position.y)
        {
            isAir = false;

            // 新的目标轮子
            targetWheel = other.transform;

            // 轮子改变方向
            targetWheel.GetComponent<Wheel>().ChangeDir();

            // 分数加一
            GameControlScript.current.BirdScored();

            // 设置成自己脚下的轮子
            other.gameObject.tag = "wheelOfMine";
        }
        else
        {
            birdDie();
        }
    }

    void birdDie()
    {
        // 死亡
        GameControlScript.current.BirdDied();

        GameControlScript.current.isGameOver = true;
    }

    }
