using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontPenetrater
{
    // Update is called once per frame
    static public Vector3 CalcVelocity(Vector3 position,Vector3 velocity)
    {
        if (velocity == null) return Vector3.zero;
        if (velocity.magnitude <= 0) return Vector3.zero;

        // 参照している速度ベクトルが壁のサイズを上回っていないならケベ抜けすることはないので即リターン
        float wallSize = FieldObjectManager.GetInstance().GetWallSize();
        if (velocity.magnitude <= wallSize) return velocity;


        if (Physics.Raycast(position, velocity.normalized, out RaycastHit raycastHit))
        {
            // 壁との距離が壁のサイズより小さいなら抜ける可能性がある
            if (raycastHit.distance >= wallSize) return velocity;

            float velocityValue = velocity.magnitude;

            // 壁を超えないように速度を制限
            float subVelocityValue = velocityValue - wallSize;
            velocity = velocity.normalized * subVelocityValue;
        }

        return velocity;
    }
}
