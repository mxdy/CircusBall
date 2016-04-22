using UnityEngine;
using System.Collections;

public class WheelDesroy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "wheel")
        {
            Destroy(other.gameObject);
        }
    }
}
