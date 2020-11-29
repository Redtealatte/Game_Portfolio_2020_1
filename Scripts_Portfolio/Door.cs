using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Sprite openSprite;
    public Sprite closeSprite;

    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeSprite;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void DoorOpen()//문을 여는 sprite로 변경하고 콜라이더를 false로 변경.
    {
        Debug.Log( name +" : Door open.");
        spriteRenderer.sprite = openSprite;
        boxCollider2D.enabled = false;
    }

    void DoorClose()//문을 닫는 sprite로 변경하고 콜라이더를 true로 변경.
    {
        Debug.Log(name + " : Door close.");
        spriteRenderer.sprite = closeSprite;
        boxCollider2D.enabled = true;
    }
}
