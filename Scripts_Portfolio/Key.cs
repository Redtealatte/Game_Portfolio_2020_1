using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    SoundManager soundManager;
    public GameObject doorObject;
    int move = 1;//up = 1, down = -1
    Vector2 start, end;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        start = transform.position;
        end = new Vector2(start.x, start.y + 0.2f);
    }

    //위아래로 움직임 효과.
    void Update()
    {
        if(move == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, end, Time.deltaTime * 0.7f);
            if (Vector2.Distance(end, transform.position) < 0.01f)
                move = -1;
        }
        else if(move == -1)
        {
            transform.position = Vector2.MoveTowards(transform.position, start, Time.deltaTime * 0.7f);
            if (Vector2.Distance(start, transform.position) < 0.01f)
                move = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            GameObject.Find(doorObject.name).SendMessage("DoorOpen");
            gameObject.SetActive(false);
            //사운드출력.
            soundManager.VolumeControl(1.0f);
            soundManager.SoundEffectPlay("Key");
        }        
    }
}
