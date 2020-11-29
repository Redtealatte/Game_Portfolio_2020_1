using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;
    BoxCollider2D coll;
    SoundManager soundManager;

    public int nextMove;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        Invoke("Think", 3);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform을 체크해서 떨어지지 않게함.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));
        if(rayHit.collider == null)
            Turn();

        if (transform.position.y < -20.0f)
            Destroy(gameObject);
    }

    void Think()
    {
        //다음 이동 방향 지정.
        nextMove = Random.Range(-1, 2);

        //애니메이션 출력.
        anim.SetInteger("WalkSpeed", nextMove);

        //방향이 바뀔 경우.
        if (nextMove != 0)
            sprite.flipX = nextMove == 1;

        //이동.
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    //낭떠러지에서 이동방향 반전.
    void Turn()
    {
        nextMove *= -1;
        sprite.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 2);
    }

    //몬스터 사망시.
    void Die()
    {
        CancelInvoke();
        nextMove = 0;
        anim.SetInteger("WalkSpeed", nextMove);
    }

    //데미지를 받았을 경우.
    public void OnDamaged()
    {
        Die();
        //사운드 출력.
        soundManager.VolumeControl(1.0f);
        soundManager.SoundEffectPlay("EnemyDie");
        //스프라이트 알파값
        sprite.color = new Color(1, 1, 1, 0.4f);
        //스프라이트 y축 반전.
        sprite.flipY = true;
        //콜라이더 제거.
        coll.enabled = false;
        //죽는 이펙트.
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //5초뒤 삭제.
        Invoke("DeActive", 5);
    }
}
