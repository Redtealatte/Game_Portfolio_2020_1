using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : ObjMove {
    
	// Update is called once per frame
	void Update () {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("OnDamaged");
        }
    }
}
