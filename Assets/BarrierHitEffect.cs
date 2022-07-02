using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitEffect : MonoBehaviour
{
    //エフェクト用マテリアル
    [SerializeField] Material material;
    //マテリアルを保持しているオブジェクト
    [SerializeField] GameObject gameObject;
    float distance;
    float count;
    [SerializeField] float AddSpeed; 
    [SerializeField] float maxSize; 
    bool isEffect;
    // Start is called before the first frame update
    void Start()
    {
        distance = 27.0f;
        count = 0.0f;
        AddSpeed = 20.0f;
        maxSize = 100.0f;
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        isEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region テスト
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isEffect = true;
        }
        #endregion
        if(isEffect)
        {
            HitEffect(AddSpeed, maxSize, Vector3.zero);
        }

        material.SetFloat("_Distance", distance);
    }

    public void SetDistance(float f)
    {
        distance = f < 0 ? 0 : f > 100.0f ? 100.0f : f;
    }

    public void HitEffect(float speed, float MaxScale,Vector3 position)
    {
        //カウントの値がspeedを超えないよう制御
        count++;
        //値の制御
        float result = Mathf.Lerp(0.0f, MaxScale, count / speed);
        //座標指定
        gameObject.transform.position = position;
        //拡大
        gameObject.transform.localScale = new Vector3(result, result, result);
        //終了
        if(count>speed)
        {
            isEffect = false;
            count = 0.0f;
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    public void IsOpen()
    {
        isEffect = true;
    }
}
