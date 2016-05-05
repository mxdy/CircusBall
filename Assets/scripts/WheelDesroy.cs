using UnityEngine;
using System.Collections;

public class WheelDesroy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // 碰到的除了背景意外的任意东西都给它摧毁掉
        if (other.gameObject.tag != "Scenery")
        {
            Destroy(other.gameObject);
        }
    }
}

// 用来清理掉移出场景的物体