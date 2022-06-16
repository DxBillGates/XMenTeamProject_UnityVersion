using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 必殺技オブジェクトを作成
public class UltimateSkillGenerator : SingletonComponent<UltimateSkillGenerator>
{
    // 必殺技を表現するオブジェクトのプレハブ
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
