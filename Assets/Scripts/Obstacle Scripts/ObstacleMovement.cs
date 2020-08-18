using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = -3f;

    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(speed, myRigidbody.velocity.y);
    }

    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Collector") {
            gameObject.SetActive(false);
        }
    }

    public float GetSpeed() {
        return speed;
    }

    public void SetSpeed(float addSpeed) {
        if(speed > -30f) {
            speed += addSpeed;
        }
    }
}
