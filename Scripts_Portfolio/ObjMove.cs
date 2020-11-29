using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : MonoBehaviour {

    [SerializeField]
    private Vector2 start;

    [SerializeField]
    private Vector2 end;

    [SerializeField]
    private float moveSpeed;

    bool back;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void Move()
    {
        if (!back)
        {
            transform.position = Vector2.MoveTowards(transform.position, end, Time.deltaTime * moveSpeed);
            if (Vector2.Distance(end, transform.position) < 0.01f)
                back = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, start, Time.deltaTime * moveSpeed);
            if (Vector2.Distance(start, transform.position) < 0.01f)
                back = false;
        }
    }    
}
