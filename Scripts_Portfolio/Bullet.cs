using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Vector2 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if(transform.position.x > startPos.x + 6.0f || transform.position.x < startPos.x - 6.0f)
            Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("OnDamaged");
            Destroy(gameObject);
        }
    }
}
