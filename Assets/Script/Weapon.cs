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
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update() {
        if(!GameManager.instance.isLive)
        {
            return;
        }

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
        this.damage = damage * Character.Damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // BroadcastMessage 특정 함수 호출을 모든 자식에게 방송하는 함수
    }
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero; // localPosition :지역위치(부모기준 위치)

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int index=0; index<GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed; // -로 회전을 Vector3.forward로 하던 +로 하고 Vector3.back을 해야 시계방향으로 돔
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // 근접은 왼속, 원거리은 오른손
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per(-1은 무한 관통)

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
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

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
