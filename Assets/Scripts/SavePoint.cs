using UnityEngine;

public class SavePoint : MonoBehaviour {
    GameManager manager;
    SoundManager soundManager;
    BoxCollider2D boxCollider;
    Animator animator;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            soundManager.VolumeControl(0.40f);
            soundManager.SoundEffectPlay("Flag");
            Vector2 savePos = new Vector2(transform.position.x, transform.position.y + 1.0f);
            manager.SetSavePoint(savePos);
            animator.SetBool("isSaved", true);
            boxCollider.enabled = false;
        }        
    }
}
