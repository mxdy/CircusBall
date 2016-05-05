using UnityEngine;
using System;
using System.Collections;

public class WheelFactory : MonoBehaviour {
    public float speed;     // 生成物移动速递
    private Transform wheelBig;         // 大轮子
    private Transform wheelNormal;      // 中轮子
    private Transform wheelSmall;       // 小轮子
    private Transform wheelDb;          // 双轮子

    private Transform pool;             // 轮子缓存池

    private float atime = 0f;           // 出球倒计时
    private float time_idx = 0f;        // 总计时器

    public float minCreatTime = 4.5f;   // 创建最小时间差 
    public float maxCreatTime = 6.1f;   // 创建最大时间差 

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

        // 游戏结束就不刷球了
        if (GameControlScript.current.isGameOver) return;

        atime -= Time.deltaTime;
        time_idx += Time.deltaTime;

        // 出球间隔
        if (atime <= 0)
        {
            atime = 0;

            int level = GameControlScript.current.gameLevel;

            GateDataCsv.BallType bt = GateDataCsv.Instance.CheckType(level);
            for (int i = 0; i < (int)bt; i++)
            {
                float ofs_x = UnityEngine.Random.Range(0.5f, 1.5f);
                Creator(GateDataCsv.Instance.CheckWheel(level), ofs_x * i);
            }

            // 当前没有达到满级的情况下到达下一个等级的节点
            if (time_idx >= GateDataCsv.Instance.CreateBallLevelTime(level))
            {
                if (level < GateDataCsv.Instance.GetMaxLevel())
                {
                    GameControlScript.current.gameLevel += 1;
                    level = GameControlScript.current.gameLevel;
                }

                time_idx = 0;
            }

            atime = (float)Math.Round(GateDataCsv.Instance.CreateBallLevelTime(level) / GateDataCsv.Instance.GetBallsCount(level),2);

        }

        // 出生点迁移
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // 出球情况
    void CheckBallModel()
    {

    }

    void Creator(GateDataCsv.WheelType wt, float ofs_x)
    {
        Transform tf = null;

        switch (wt)
        {
            case GateDataCsv.WheelType.normal:
                tf = Instantiate<Transform>(wheelNormal);
                break;
            case GateDataCsv.WheelType.bigone:
                tf = Instantiate<Transform>(wheelBig);
                break;
            case GateDataCsv.WheelType.smallone:
                tf = Instantiate<Transform>(wheelSmall);
                break;
            //case GateDataCsv.WheelType.dbone:
            //    tf = Instantiate<Transform>(wheelNormal);
            //    break;
            default:
                break;
        }

        tf.SetParent(pool);

        // 计算出生偏移
        float offset_h = tf.GetComponent<Wheel>().bornHoffset;
        tf.position = new Vector2(transform.position.x + ofs_x,transform.position.y + offset_h);

        Wheel wheel = tf.GetComponent<Wheel>();

        Vector2 vec2 = GateDataCsv.Instance.GetVelocity(GameControlScript.current.gameLevel);
        float v = UnityEngine.Random.Range(vec2.x, vec2.y);
        Debug.Log(v);
        wheel.SetVelocity(v);
    }
}

// 小球生成器
// mxdy
// 16/5/4