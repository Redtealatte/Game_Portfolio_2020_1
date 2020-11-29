using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planty : MonoBehaviour {

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;
    BoxCollider2D coll;
    SoundManager soundManager;

    public GameObject bulletObj;

    Vector2 circlePos;
    float detectionRange;

    public float maxShotDelay;
    public float curShotDelay;

    bool isAttacking = false;
    bool isDead = false;


    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        circlePos = transform.position;        
        detectionRange = 6.0f;
    }
	
    void FixedUpdate()
    {
        if(!isDead)
            Think();
        if(isAttacking)
            Reload();
        if (transform.position.y < -20.0f)
            Destroy(gameObject);
    }
    
    //플레이어가 범위안에 있는 지 확인.
    void Think()
    {
        RaycastHit2D hit = Physics2D.CircleCast(circlePos, detectionRange, Vector2.zero, 0.0f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            Turn(hit.collider.transform.position);
            isAttacking = true;
            Fire();
        }
        else
        {
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
        }
    }
    
    //플레이어가 있는 방향을 바라본다.
    void Turn(Vector3 hitPosition)
    {
        Vector3 dir = new Vector3(hitPosition.x - transform.position.x, hitPosition.y - transform.position.y);

        //Debug.DrawRay(transform.position, dir, new Color(1.0f, 1.0f, 0.0f));
        if (dir.x > 0)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    //공격.
    void Fire()
    {
        if (curShotDelay > maxShotDelay - 1.0f)
            anim.SetBool("isAttacking", isAttacking);
        
        if (curShotDelay < maxShotDelay)
            return;

        Vector2 shotPos = new Vector2(transform.position.x, transform.position.y + 0.35f);
        GameObject bullet = Instantiate(bulletObj, shotPos, transform.rotation);
        Rigidbody2D rigidB = bullet.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteB = bullet.GetComponent<SpriteRenderer>();
        if (sprite.flipX)
        {
            spriteB.flipX = true;
            rigidB.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }
        else
        {
            rigidB.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
        anim.SetBool("isAttacking", false);
    }

    //재장전 타이머.
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    //사망.
    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", isDead);
    }

    //데미지를 받았을때.
    public void OnDamaged()
    {
        Die();
        //사운드 출력
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


    ////에디터 상에서 공격범위를 보여줌.
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;//색지정.
    //    Gizmos.DrawWireSphere(circlePos, detectionRange);
    //}
}
