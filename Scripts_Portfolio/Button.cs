using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public GameObject doorObject;
    SpriteRenderer spriteRenderer;
    public Sprite pushB;
    public Sprite popB;

    bool pushButton = false;

    Vector2 boxsize;
    Vector2 boxPos;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = popB;
        //Gizmos에 사용할 박스의 크기 좌표 지정.
        boxsize = new Vector2(0.8f, 0.1f);
        boxPos = new Vector2(transform.position.x, transform.position.y + 0.4f);
    }
	
	// Update is called once per frame
	void Update () {
        //투명한 사각형을 발사합니다.
        bool hit = Physics2D.BoxCast(boxPos, boxsize, 0.0f, Vector2.up, 0.1f, LayerMask.GetMask("Player", "MovingObject"));
        if (hit && !pushButton)// hit이고 !pushButton이라면 오브젝트에게서 "DoorOpen" 메서드를 실행시키고 pushButton을 true로 바꿔라.
        {
            GameObject.Find(doorObject.name).SendMessage("DoorOpen");
            pushButton = true;
            spriteRenderer.sprite = pushB;
            Debug.Log("pushButton : " + pushButton.ToString());
        }
        else if(!hit && pushButton)// !hit이고 pushButton이라면 오브젝트에게서 "DoorClose" 메서드를 실행시키고 pushButton을 false로 바꿔라.
        {
            GameObject.Find(doorObject.name).SendMessage("DoorClose");
            pushButton = false;
            spriteRenderer.sprite = popB;
            Debug.Log("pushButton : " + pushButton.ToString());
        }
    }

    //에디터 상에서만 보이는 도형을 그려주는 함수.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;//색지정.
        Gizmos.DrawWireCube(boxPos, boxsize);//와이어로된 사각형.
    }
}
