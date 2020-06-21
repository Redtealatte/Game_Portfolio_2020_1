using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharacter : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            col.transform.parent = gameObject.transform;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && transform.localPosition.y + 0.9f > col.transform.localPosition.y)
            col.transform.parent = null;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            col.transform.parent = null;
    }
}
