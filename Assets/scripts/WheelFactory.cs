using UnityEngine;
using System.Collections;

public class WheelFactory : MonoBehaviour {

    private Transform wheelBig;         // 大轮子
    private Transform wheelNormal;      // 中轮子
    private Transform wheelSmall;       // 小轮子
    private Transform wheelDb;          // 双轮子

    private Transform pool;             // 轮子缓存池

    private float atime = 1f;           // 计时器

    public float minCreatTime = 4.5f;   // 创建最小时间差 
    public float maxCreatTime = 6.1f;   // 创建最大时间差 

    // 轮子类型
    enum WheelType
    {
        normal,
        bigone,
        smallone,
        dbone
    }

    void Awake()
    {
        wheelBig = Resources.Load<Transform>("assets/prefbs/wheel_big");
        wheelNormal = Resources.Load<Transform>("assets/prefbs/wheel_normal");
        wheelSmall = Resources.Load<Transform>("assets/prefbs/wheel_small");
        wheelDb = Resources.Load<Transform>("assets/prefbs/dbWheel");
    }

	// Use this for initialization
	void Start () {
        pool = GameObject.Find("wheelPool").transform;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameControlScript.current.isGameOver) return;

        atime -= Time.deltaTime;

        if (atime <= 0)
        {
            Creator((WheelType)Random.Range(0, 4));

            atime = Random.Range(minCreatTime, maxCreatTime);
        }
	}

    void Creator(WheelType wt)
    {
        Transform tf = null;

        switch (wt)
        {
            case WheelType.normal:
                tf = Instantiate<Transform>(wheelNormal);
                break;
            case WheelType.bigone:
                tf = Instantiate<Transform>(wheelBig);
                break;
            case WheelType.smallone:
                tf = Instantiate<Transform>(wheelSmall);
                break;
            case WheelType.dbone:
                tf = Instantiate<Transform>(wheelDb);
                break;
            default:
                break;
        }

        tf.SetParent(pool);
        tf.position = transform.position;

        Wheel wheel = tf.GetComponent<Wheel>();
    }
}
