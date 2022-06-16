using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 必殺技オブジェクトを作成
public class UltimateSkillGenerator : SingletonComponent<UltimateSkillGenerator>
{
    // 必殺技を表現するオブジェクトのプレハブ
    [SerializeField] private GameObject prefabUltimateSkillObject;
    //[SerializeField]private GameObject

    private GameObject createdUltimateSkillObject;

    Vector3 startLerpScale;
    Vector3 endLerpScale;
    bool isAddScale;
    float lerpTime;
    float maxLerpTime;

    void Start()
    {
        startLerpScale = new Vector3();
        endLerpScale = new Vector3();
        isAddScale = false;
        lerpTime = 0;
        maxLerpTime = 1;
    }

    void Update()
    {
        if (isAddScale == false) return;
        if (createdUltimateSkillObject == null) return;

        bool isReturn = false;
        Vector3 setScale = Vector3.Lerp(startLerpScale, endLerpScale, lerpTime);
        if(lerpTime > 1)
        {
            isReturn = true;
            setScale = endLerpScale;
        }

        createdUltimateSkillObject.transform.localScale = setScale;

        if (isReturn == true) return;

        lerpTime += Time.deltaTime / maxLerpTime;
    }

    public void StartAddScale()
    {
        isAddScale = true;
    }

    public void CreateSkillObject(Vector3 position,float scale,float setMaxLerpTime)
    {
        createdUltimateSkillObject = Instantiate(prefabUltimateSkillObject);
        createdUltimateSkillObject.transform.position = position;
        createdUltimateSkillObject.transform.localScale = new Vector3();

        startLerpScale = new Vector3();
        endLerpScale = new Vector3(scale, scale,scale);
        lerpTime = 0;
        maxLerpTime = setMaxLerpTime;
    }

    public void DestroySkillObject()
    {
        if(createdUltimateSkillObject)
        {
            Destroy(createdUltimateSkillObject);

            isAddScale = false;
            startLerpScale = new Vector3();
            endLerpScale = new Vector3();
        }
    }

    public Vector3 GetCreatedObjectPosition()
    {
        if (createdUltimateSkillObject == null) return Vector3.zero;

        return createdUltimateSkillObject.transform.position;
    }

    public float GetCreatedObjectScale()
    {
        if (createdUltimateSkillObject == null) return 0;

        return createdUltimateSkillObject.transform.localScale.y;
    }

    private float EaseOutExpo(float t)
    {
        return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }
}
