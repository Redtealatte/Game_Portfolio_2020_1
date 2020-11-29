using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float maxSpeed;
    public float jumpPower;

    bool isJumping = false;
    bool isWalking = false;
    
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    GameObject scanObject;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;
    AudioClip audioClip;

    Vector2 dir;

    public GameManager manager;

    bool isDead;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //audio 설정.
        audioSource = GetComponent<AudioSource>();
        audioClip = Resources.Load("Sounds/DM-CGS-07") as AudioClip;
        dir = new Vector2(-1, 0);

        if(!PlayerPrefs.HasKey("PlayerX") && !PlayerPrefs.HasKey("isReset"))
            manager.HPInitialize(3);//맵이 바뀌었을때 이전 맵에서 남아있던 hp를 가져와야 된다. 수정 필요.

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {        
        //점프
        if (Input.GetButtonDown("Jump") && !isJumping && !manager.isAction)
        {
            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", isJumping);
            audioSource.PlayOneShot(audioClip);
        }

        //좌우 방향키에서 손을 떼면 속도가 천천히 감소해서 정지하게됨.
        if (Input.GetButtonUp("Horizontal"))//Horizontal : 좌 -1, 우 1, 안눌림 0
        {
            //normalized : 벡터의 크기를 1로 만든 상태.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 1.0f, rigid.velocity.y);
        }

        //sprite의 방향전환.
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !manager.isAction)//좌
        {
            spriteRenderer.flipX = false;
            dir.x = -1; 
        }
                    
        if (Input.GetKeyDown(KeyCode.RightArrow) && !manager.isAction)//우
        {
            spriteRenderer.flipX = true;
            dir.x = 1;
        }
            
        
        if(Input.GetButton("Horizontal"))
            isWalking = true;
        else
            isWalking = false;

        if(Mathf.Abs(rigid.velocity.x) < 0.3)//속도가 0일때 false.
            animator.SetBool("isWalking", false);
        else // 나머진 true 반환
            animator.SetBool("isWalking", true);

        if(Input.GetKeyDown(KeyCode.LeftControl) && (scanObject != null || manager.isAction))
            manager.Action(scanObject);

        if (transform.position.y < -10.0f && !isDead)
        {
            isDead = true;
            manager.GameFailed();
        } 
    }

    //일정 시간 간격차마다 업데이트.
    void FixedUpdate () {
        float h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //상호작용할 오브젝트가 있는지 확인.
        Debug.DrawRay(rigid.position, dir, new Color(0, 0, 1));
        RaycastHit2D rayHitObj = Physics2D.Raycast(rigid.position, dir, 1.0f, LayerMask.GetMask("MovingObject", "Default"));

        if (rayHitObj.collider != null)
            scanObject = rayHitObj.collider.gameObject;
        else
            scanObject = null;

        //Platform에 착지할 때.
        if (rigid.velocity.y < 0 && isJumping)
        {
            Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));//에디터상에서 Ray를 그려줌.

            //관통이 안되는 ray를 쏨. 쏘는위치, 방향, 거리, (원하는 Layer만). 마스크 효과.
            RaycastHit2D rayHitC = Physics2D.Raycast(rigid.position, Vector2.down, 1.0f, LayerMask.GetMask("Platform", "MovingObject", "Default"));

            if (rayHitC.collider != null && rayHitC.distance < 0.75f)//평지에 있을때.
            {
                isJumping = false;
                Debug.Log(rayHitC.collider.name);
                animator.SetBool("isJumping", isJumping);                
            }
            else//캐릭터의 중심 기준으로 platform이 닿아있지 않은 경우. 즉, 경사로.
            {
                Debug.DrawRay(new Vector2(rigid.position.x - 0.38f, rigid.position.y), new Vector2(0, -0.72f), new Color(0, 1, 0));//좌측경사면에 캐릭터의 캡슐과 닿는 ray
                Debug.DrawRay(new Vector2(rigid.position.x + 0.38f, rigid.position.y), new Vector2(0, -0.72f), new Color(0, 1, 0));//우측경사면에 캐릭터의 캡슐과 닿는 ray

                RaycastHit2D rayHitL = Physics2D.Raycast(new Vector2(rigid.position.x - 0.38f, rigid.position.y), Vector2.down, 1.0f, LayerMask.GetMask("Platform", "MovingObject", "Default"));
                RaycastHit2D rayHitR = Physics2D.Raycast(new Vector2(rigid.position.x + 0.38f, rigid.position.y), Vector2.down, 1.0f, LayerMask.GetMask("Platform", "MovingObject", "Default"));

                if (rayHitL.collider != null || rayHitR.collider != null)
                    //rayHitL과 rayHitR 중 어느 하나라도 경사면에 닿이면 점프를 종료시킴.
                    if (rayHitL.distance < 0.72f || rayHitR.distance < 0.72f)
                    {
                        isJumping = false;
                        animator.SetBool("isJumping", isJumping);
                    }
            }
        }

        //걷지 않는다면 천천히 속도 감소.
        if (!isWalking)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
            rigid.velocity = new Vector2(rigid.velocity.x - rayHit.normal.x * 0.35f, rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Trap")
        {
            //공격
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y && collision.gameObject.tag != "Trap")
                OnAttack(ref collision);
            //피격
            else
                OnDamaged();
        }
    }

    //공격.
    void OnAttack(ref Collision2D enemy)
    {
        //반발력.
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        //enemy 사망.
        enemy.gameObject.SendMessage("OnDamaged");
    }

    //피격 시 무적
    void OnDamaged()
    {
        //hp 감소.
        manager.HPChange(true);

        //피격시 layer PlayerDamaged로 바꿔 무적상태로 변경.
        gameObject.layer = 12;

        //알파값을 0.4f로 바꿔서 흐릿하게 보이게.
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc;
        
        dirc = dir.x > 0 ? -1 : 1;

        rigid.AddForce(new Vector2(dirc, 1)*10, ForceMode2D.Impulse);

        animator.SetTrigger("doDamaged");
        Invoke("OffDamaged", 3);
        
    }

    //무적해제
    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);//투명해제
    }

    //플레이어 사망.
    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}
