using UnityEngine;
using System.Collections;

public class GateDataCsv : Singleton<GateDataCsv> {
    private CSVParse parse;

    // 出球模式 一个球，两个球，三个球
    public enum BallType
    {
        one_ball = 1,
        two_balls,
        three_balls
    }

    // 轮子类型
    public enum WheelType
    {
        normal = 1,
        bigone,
        smallone
    }

    public GateDataCsv()
    {
        parse = new CSVParse("assets/csv/gate_data");
    }

    // 总的level
    public int GetMaxLevel()
    {
        return parse.getLength();
    }

    /// <summary>
    /// 当前难度顶级的总时间
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float CreateBallLevelTime(int level)
    {
        return parse.getFloatByID(level, "Time");
    }

    /// <summary>
    /// 期间会出现多少球
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetBallsCount(int level)
    {
        return parse.getIntByID(level, "BallsCount");
    }

    // 获得当前等级的大球出现权重
    public float GetPBigBall(int level)
    {
        return parse.getFloatByID(level, "PBigBall");
    }

    // 获得当前等级的中球出现权重
    public float GetPNormalBall(int level)
    {
        return parse.getFloatByID(level, "PBigBall");
    }

    // 获得当前等级的小球出现权重
    public float GetPSmallBall(int level)
    {
        return parse.getFloatByID(level, "PBigBall");
    }

    // 一个球出现权重
    public float GetPOneBall(int level)
    {
        return parse.getFloatByID(level, "POneBall");
    }

    // 两个球出现权重
    public float GetPTwoBall(int level)
    {
        return parse.getFloatByID(level, "PTwoBall");
    }

    // 三个球出现权重
    public float GetPThreeBall(int level)
    {
        return parse.getFloatByID(level, "PThreeBall");
    }

    // 随机当前等级的出球模式 1球 2球 3球
    public BallType CheckType(int level)
    {
        float all_p = GetPOneBall(level) + GetPTwoBall(level) + GetPThreeBall(level);
        float idx_p = Random.Range(0, all_p);

        if (idx_p <= GetPOneBall(level))
        {
            return BallType.one_ball;
        }
        else if (idx_p <= GetPOneBall(level) + GetPTwoBall(level))
        {
            return BallType.two_balls;
        }
        else
        {
            return BallType.three_balls;
        }
    }

    // 当前随机出来的小球类型
    public WheelType CheckWheel(int level)
    {
        float all_p = GetPNormalBall(level) + GetPBigBall(level) + GetPSmallBall(level);
        float idx_p = Random.Range(0, all_p);

        if (idx_p <= GetPNormalBall(level))
        {
            return WheelType.normal;
        }
        else if (idx_p <= GetPNormalBall(level) + GetPBigBall(level))
        {
            return WheelType.bigone;
        }
        else
        {
            return WheelType.smallone;
        }
    }

    // 小球的范围
    public Vector2 GetVelocity(int level)
    {
        Vector2 velocity = Vector2.zero;

        string velocity_str = parse.getStringByID(level, "BallSpeedRange");
        string[] kid_strs = velocity_str.Split('|');

        if (kid_strs.Length > 1)
        {
            float.TryParse(kid_strs[0], out velocity.x);
            float.TryParse(kid_strs[1], out velocity.y);
        }
        else
        {
            float.TryParse(kid_strs[0], out velocity.x);
            float.TryParse(kid_strs[0], out velocity.y);
        }

        return velocity;
    }
}

// 游戏出球规则数据表解析脚本
// author: mxdy
// 16/5/4
