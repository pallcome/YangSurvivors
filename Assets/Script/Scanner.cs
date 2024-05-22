using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour {
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // CircleCastAll() 원형의 캐스트를 쏘고 모든 결괄르 반환하는 함수
        // 1. 캐스팅 시작 위치
        // 2. 원의 반지름
        // 3. 캐스팅 방향
        // 4. 캐스팅 길이
        // 5. 대상레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange,Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
