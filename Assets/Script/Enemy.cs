using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        // GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는 함수
        // anim.GetCurrentAnimatorStateInfo(0) : 0번째 레이어

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.instance.isLive)
        {
            return;
        }

        // Player 방향으로 이동
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        if (!isLive || !GameManager.instance.isLive)
        {
            return;
        }

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true; // 활성화
        rigid.simulated = true; // 활성화
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // 애니메이터를 변경해주는 방법
        speed = data.speed;
        maxHealth = data.health;
        health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive) // 총알이 아닌거와 충돌 했는가
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); // 코루틴 호출 방식 StartCoroutine("KnockBack");

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false; // 비활성화
            rigid.simulated = false; // 비활성화
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if(GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    IEnumerator KnockBack()
    {
        // yield return null; // 1프레임 쉬기
        // yield return new WaitForSeconds(2f); // 2초 쉬기

        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // Impulse 즉각적인
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
