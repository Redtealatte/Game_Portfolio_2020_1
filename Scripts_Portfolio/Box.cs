using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    SoundManager soundManager;

    bool push = false;
    bool fall = false;

    Vector2 targetPos;
    Vector2 moveSpeed;

    float fallSpeed = 5.0f;
    float moveTime = 0.1f;
    float dir = -1;
    float checkRange = 1.4f;

    // Use this for initialization
    void Start () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (push && !fall)
            Moving();
        if (fall)
            Falling();
    }

    void Moving()//박스 이동.
    {
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref moveSpeed, moveTime);//부드럽게 targetPos로 이동.
        if (Vector2.Distance(targetPos, transform.position) <= 0.0001f)//오차를 최소로해서 push를 false로 만듦.
        {
            push = false;
            transform.position = targetPos;//오차를 없애기위에 마지막으로 targetPos로 이동.

            //바닥으로 ray를 쏴서 부딪혔는지 여부를 hit에 저장.
            bool hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.6f), Vector2.down, 0.9f);

            if (!hit)//box가 물체 위에 있는것이 아니라면 box가 바닥으로 떨어짐.
                fall = true;
        }
    }

    void Falling()
    {
        Debug.DrawRay(transform.position, new Vector2(0.0f, -checkRange), new Color(0, 1, 0)); //바닥으로 ray를 쏘는 것을 시각화.
        //ray를 1.0f길이로 쏴서 맞는지 확인.
        RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5001f), Vector2.down, 1.0f);
        //ray가 맞았고 길이가 0.01f 미만이라면.
        if(raycastHit2D.collider != null && raycastHit2D.distance < 0.01f)
        {
            fall = false;//낙하를 종료.
            //소수점이하를 버리고 0.5f를 더한 위치로 box의 위치 변경.
            transform.position = new Vector2(transform.position.x, Mathf.Floor(transform.position.y) + 0.5f);
        }
        else
        {
            //targetPos는 현재 box 좌표의 y값이 1.0f 더 낮은 값이다.
            targetPos.x = transform.position.x;
            targetPos.y = transform.position.y - 1.0f;
            //targetPos로 이동. 중력에 영향을 받지않고 동일한속도로 떨어짐.
            transform.position = Vector2.MoveTowards(transform.position, targetPos, fallSpeed * Time.deltaTime);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {        
        //Debug.Log("col name : " + coll.gameObject.name);//부딪힌 오브젝트 이름 반환.

        bool left = Physics2D.Raycast(transform.position, Vector2.left, 1.0f, LayerMask.GetMask("Player"));//좌
        bool right = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, LayerMask.GetMask("Player"));//우
        if (left)//좌측에서 밀고 있으니 우측으로 밀린다.
            dir = 1.0f;
        if (right)//우측에서 밀고 있으니 좌측으로 밀린다.
            dir = -1.0f;

        //밀리는 방향에 물체가 있는지 확인하는 ray. Default와 Platform을 제외한 나머지는 무시.
        bool unmovable = Physics2D.Raycast(transform.position, new Vector2(dir, 0), checkRange, LayerMask.GetMask("Default", "Platform", "Enenmy", "Trap"));

        //충돌한 것이 "Player"이고, 좌 또는 우가 true이고 이동불가가 아니라면.
        if (coll.gameObject.tag == "Player" && (left || right) && !unmovable && !push && !fall)
        {
            push = true;
            targetPos = new Vector2(transform.position.x + dir, transform.position.y);//목표좌표
            float playerY = coll.gameObject.transform.position.y;
            float boxY = transform.position.y;
            //사운드 출력.
            if (playerY <= boxY + 0.5f)
            {
                soundManager.VolumeControl(1.0f);
                soundManager.SoundEffectPlay("Box");
            }                
        }        
    }
}
