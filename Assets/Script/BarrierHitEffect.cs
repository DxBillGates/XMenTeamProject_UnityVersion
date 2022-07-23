using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitEffect : MonoBehaviour
{
    //エフェクト用マテリアル
    [SerializeField] Material material;
    //歪みエフェクトオブジェクト
    [SerializeField] GameObject hitObj;
   
    struct RefractionHitEffect
    {
        public GameObject refractionClone;
        public float count;
        public bool isEffect;

    }
    //歪みエフェクトを同時に何個まで出せるか
    int hitNum = 3;


    //歪みエフェクトのクローン
    RefractionHitEffect[] refraction;
    //歪みエフェクトの歪む広さ
    float distance;
    //拡大する速さ
    [SerializeField] float AddSpeed;
    //エフェクトの拡大する大きさ
    [SerializeField] float MaxScale;

    // Start is called before the first frame update
    void Start()
    {

        //歪みエフェクト初期化
        refraction = new RefractionHitEffect[hitNum];
        for (int i = 0; i < hitNum; i++)
        {
            refraction[i].count = 0.0f;
            refraction[i].isEffect = false;

        }
        distance = 27.0f;

    }

    // Update is called once per frame
    void Update()
    {
        RefractionControl();
    }

    public void SetDistance(float f)
    {
        distance = f < 0 ? 0 : f > 100.0f ? 100.0f : f;
    }

    //歪みエフェクト制御
    void RefractionControl()
    {

        for (int i = 0; i < hitNum; i++)
        {

            if (refraction[i].isEffect)
            {
                if (refraction[i].count < AddSpeed)
                {
                    refraction[i].count++;
                    //値の制御
                    float result = Mathf.Lerp(0.0f, MaxScale, refraction[i].count / AddSpeed);
                    //拡大
                    refraction[i].refractionClone.transform.localScale = new Vector3(result, result, result);
                }
                else
                {
                    refraction[i].isEffect = false;
                    Destroy(refraction[i].refractionClone);
                    refraction[i].count = 0.0f;
                }
            }
        }
        material.SetFloat("_Distance", distance);
    }
    //歪みエフェクト展開
    public void Use(Vector3 position)
    {
        for (int i = 0; i < hitNum; i++)
        {
            if (!refraction[i].isEffect)
            {
                refraction[i].isEffect = true;
                refraction[i].refractionClone = Instantiate(hitObj, position, new Quaternion(0, 1, 0, 0));
                break;
            }
        }
    }
}