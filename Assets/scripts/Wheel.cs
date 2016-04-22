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
        if (other.gameObject.tag == "wheelOfMine")
        {
            GameControlScript.current.BirdDied();
        }
    }

    // 改变方向
    public virtual void ChangeDir()
    {

    }

    // 删除自己
    public virtual void DestoryMyself()
    {

    }

    // 提供一个小鸟踩的位置
    public virtual Vector2 GetBirdPos(Transform tran = null)
    {
        return Vector2.zero;
    }
}
