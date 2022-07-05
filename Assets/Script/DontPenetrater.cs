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

        // �Q�Ƃ��Ă��鑬�x�x�N�g�����ǂ̃T�C�Y�������Ă��Ȃ��Ȃ�P�x�������邱�Ƃ͂Ȃ��̂ő����^�[��
        float wallSize = FieldObjectManager.GetInstance().GetWallSize();
        if (velocity.magnitude <= wallSize) return velocity;


        if (Physics.Raycast(position, velocity.normalized, out RaycastHit raycastHit))
        {
            // �ǂƂ̋������ǂ̃T�C�Y��菬�����Ȃ甲����\��������
            if (raycastHit.distance >= wallSize) return velocity;

            float velocityValue = velocity.magnitude;

            // �ǂ𒴂��Ȃ��悤�ɑ��x�𐧌�
            float subVelocityValue = velocityValue - wallSize;
            velocity = velocity.normalized * subVelocityValue;
        }

        return velocity;
    }
}
