using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
    [HideInInspector]
    public float velocity;                      // 速度
    protected const float angleA = -100;        // 轮子根据速度提升旋转的增量
    public float bornHoffset;                   // 出生高度偏移值

    [HideInInspector]
    public bool isChecked = false;

    [HideInInspector]
    public float validPosY;                     // 有效高度（角色踩在上面的一个有效范围，之后需要优化成上半球的角度夹角）
    public float heightOffset = 0.3f;           // 有效高度偏移 （相对于球中心的y的偏移量）
    public ConstantEnum.WheelType wheelType;    // 轮子类型

    // 自己的检测
    void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerWheel(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isChecked = false;
    }

    public virtual void OnTriggerWheel(GameObject other)
    {
        // 和角色脚下的轮子碰撞到了 游戏结束
        if (other.tag == "wheelOfMine")
        {
            GameControlScript.current.BirdDied();
        }
    }

    public virtual void RandomVelocity(Vector2 v_range)
    {
        velocity = Random.Range(v_range.x, v_range.y);
    }

    public virtual void Move()
    { }

    // 改变方向
    public virtual void ChangeDir()
    {

    }

    // 改变速度
    public virtual void SetVelocity(float v)
    {
        Debug.Log("set velocity -- parent call back");
    }

    // 删除自己
    public virtual void DestoryMyself()
    {

    }

    // 提供一个小鸟踩的位置
    public virtual Vector2 GetTargetPos(Transform tran = null)
    {
        return Vector2.zero;
    }
}
