using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    public bool isLeft; // 오른손, 왼손 구분을 위한 용도
    public SpriteRenderer spriter;

    SpriteRenderer player;

    // 오른손
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);

    // 왼손
    Quaternion leftRot = Quaternion.Euler(0, 0, -35); // 회전
    Quaternion leftRotRevers = Quaternion.Euler(0, 0, -135); // 회전

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if(isLeft) { // 근접무기
            transform.localRotation = isReverse ? leftRotRevers : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
