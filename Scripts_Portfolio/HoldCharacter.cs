using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharacter : MonoBehaviour {

    //움직이는 발판 위에 플레이어가 올라갔을때, 같이 움직이도록함.
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
