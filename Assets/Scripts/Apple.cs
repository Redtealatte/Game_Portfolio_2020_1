using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {
    GameManager manager;
    CircleCollider2D cirCollider;
    SoundManager soundManager;
    SpriteRenderer sprite;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cirCollider = GetComponent<CircleCollider2D>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //Apple 비활성화.
    void DestroyApple()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            manager.HPChange(false);            
            cirCollider.enabled = false;
            sprite.enabled = false;
            //사운드 출력.
            soundManager.VolumeControl(1.0f);
            soundManager.SoundEffectPlay("Apple");
            Invoke("DestroyApple", 2.0f);
        }
    }
}
