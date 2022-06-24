using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dissolveControl : MonoBehaviour
{
    //�ω���^����}�e���A��
    [SerializeField] Material myMaterial;
    //��]�̑���
    [SerializeField] float rotaTime;
    [SerializeField]float threshold;
    [SerializeField] Color color;
    //���Ԍo��
    float count;

    // Start is called before the first frame update
    void Start()
    {
        rotaTime = 1.0f;
        count = 0.0f;
        //マテリアルのパラメーターにセット
       
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.SetFloat("_OffsetX", count);
         myMaterial.SetColor("_Color",color);
        myMaterial.SetFloat("_Threshold",threshold);
        count += Time.deltaTime * rotaTime;
    }
}
