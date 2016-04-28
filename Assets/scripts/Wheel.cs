using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
    public enum Direction { left, right }       // 方向
    public float velocity = 2.0f;               // 速度

    [HideInInspector]
    public float validPosY;                     // 有效高度
    public float heightOffset = 0.3f;           // 有效高度偏移

    public ConstantEnum.WheelType wheelType;           // 轮子类型
    // 自己的检测
    void OnCollisionEnter2D(Collision2D other)
    {
        // 和角色脚下的轮子碰撞到了 游戏结束
        if (other.gameObject.tag == "wheelOfMine")
        {
            GameControlScript.current.BirdDied();

            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    // 改变方向
    public virtual void ChangeDir()
    {

    }

    // 改变速度
    public virtual void SetVelocity(float v)
    {}

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
