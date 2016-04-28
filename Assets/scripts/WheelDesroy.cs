using UnityEngine;
using System.Collections;

public class WheelDesroy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag.CompareTo("oneWheel") > 0 
            || tag.CompareTo("dbWheel") > 0
            || tag.CompareTo("wheelOfMine") > 0)
        {
            Destroy(other.gameObject);
        }
    }
}
