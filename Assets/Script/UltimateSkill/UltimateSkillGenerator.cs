using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �K�E�Z�I�u�W�F�N�g���쐬
public class UltimateSkillGenerator : SingletonComponent<UltimateSkillGenerator>
{
    // �K�E�Z��\������I�u�W�F�N�g�̃v���n�u
    [SerializeField] private GameObject prefabUltimateSkillObject;

    private GameObject createdUltimateSkillObject;

    public void CreateSkillObject(Vector3 position,float scale)
    {
        createdUltimateSkillObject = Instantiate(prefabUltimateSkillObject);
        createdUltimateSkillObject.transform.position = position;
        createdUltimateSkillObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void DestroySkillObject()
    {
        if(createdUltimateSkillObject)
        {
            Destroy(createdUltimateSkillObject);
        }
    }
}
