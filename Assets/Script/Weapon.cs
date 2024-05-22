using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update() {
        switch (id) {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime); // Update에서 무언가 변경될때는 Time.deltaTime 필요
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage += damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; // -로 회전을 Vector3.forward로 하던 +로 하고 Vector3.back을 해야 시계방향으로 돔
                Batch();
                break;
            case 1:
                speed = 0.3f;
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index); // 있는거 재사용
            }else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform; // 새로 생성
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero; // 위치의 초기화 값
            bullet.localRotation = Quaternion.identity; // 회전의 초기화 값

            Vector3 rotVec = Vector3.back * 360 * index / count; // 360도를 갯수만큼 나눠서 각도를 부여
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World); // up이 되었다는게 이해가 안감
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per(-1은 무한 관통)
        }
    }

    void Fire()
    {
        // 방법1. GameManager.instance.player
        if (!player.scanner.nearestTarget) // Weapon은 Player의 자식이므로 부모를 찾아갈수있다.
        {
            return;
        }

        // 총알이 나아가야하는 방향
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // 방향
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // -1 is Infinity Per(-1은 무한 관통)
    }
}
