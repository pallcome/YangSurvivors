using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;
    public int per; // 관통력

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per >= 0) // 근접무기는 -1
        {
            rigid.velocity = dir * 15f; // velocity는 속도 / 방향 * 속도
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -100) // 몬스터가 아닌 경우, 근접무기인 경우
        {
            return;
        }

        per--;
        if(per < 0) // 관통력을 다 한 경우 비활성화
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100) // 몬스터가 아닌 경우, 근접무기인 경우
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
