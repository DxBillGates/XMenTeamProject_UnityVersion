using System.Collections.Generic;
using UnityEngine;

public class CollisionManager
{
    static public float CollisionBoxAndPlane(Transform transform, Bounds bounds, Transform otherTransform, Vector3 hitNormal)
    {
        // 平面にどれだけ貫通しているか
        float resultPenetrateDistance = 0;

        // ヒットしている平面にobbの各軸を射影し加算した結果
        float r = 0;
        // バウンディングボックスの軸を取得
        List<Vector3> axis = new List<Vector3>();
        axis.Add(transform.right);
        axis.Add(transform.up);
        axis.Add(transform.forward);

        // obbの各軸を平面に射影し加算する    
        for (int i = 0; i < 3; ++i)
        {
            r += Mathf.Abs(Vector3.Dot(axis[i] * bounds.size[i] / 2, hitNormal));
        }

        // 平面とobbの中心点の距離
        Vector3 otherPosition = otherTransform.position + otherTransform.forward * otherTransform.localScale.z / 2;
        float distance = Vector3.Dot(transform.position - otherPosition, hitNormal);

        // 平面との距離がobbの中心も含んでめり込んでいる場合は内積の結果が鈍角になるのでその結果を
        // 利用して押し出しの符号を操作する
        if(distance > 0)
        {
            resultPenetrateDistance = r - Mathf.Abs(distance);
        }
        else
        {
            resultPenetrateDistance = r + Mathf.Abs(distance);
        }


        return resultPenetrateDistance;
    }
}

