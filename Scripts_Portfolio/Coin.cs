using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public int coinType;
    Animator animator;
    GameManager manager;
    SoundManager soundManager;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        if (animator.GetInteger("coinType") != coinType)
            animator.SetInteger("coinType", coinType);        
    }
	
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            manager.MakeMoney(coinType);
            //사운드 출력.
            soundManager.VolumeControl(0.2f);
            soundManager.SoundEffectPlay("Coin");
        }
    }
}
