using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour {
    public ItemData.ItemType type;
    public float rate; // 레벨별 데이터

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId; // name은 GameObject의 Name
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear() // 무기가 생성 됐을때, 무기가 업그레이드 됐을때, 기어가 생겼을때, 기어가 레벨업 됐을때
    {
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0: // 근접 무기
                    //weapon.speed = weapon.speed + (weapon.speed * rate);
                    weapon.speed = 150 + (150 * rate);
                    break;
                default: // 원거리 무기
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
            
        }
    }

    void SpeedUp()
    {
        float speed = 3;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
