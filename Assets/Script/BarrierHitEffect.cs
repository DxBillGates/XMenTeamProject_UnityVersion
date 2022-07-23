using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitEffect : MonoBehaviour
{
    //�G�t�F�N�g�p�}�e���A��
    [SerializeField] Material material;
    //�c�݃G�t�F�N�g�I�u�W�F�N�g
    [SerializeField] GameObject hitObj;
   
    struct RefractionHitEffect
    {
        public GameObject refractionClone;
        public float count;
        public bool isEffect;

    }
    //�c�݃G�t�F�N�g�𓯎��ɉ��܂ŏo���邩
    int hitNum = 3;


    //�c�݃G�t�F�N�g�̃N���[��
    RefractionHitEffect[] refraction;
    //�c�݃G�t�F�N�g�̘c�ލL��
    float distance;
    //�g�傷�鑬��
    [SerializeField] float AddSpeed;
    //�G�t�F�N�g�̊g�傷��傫��
    [SerializeField] float MaxScale;

    // Start is called before the first frame update
    void Start()
    {

        //�c�݃G�t�F�N�g������
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

    //�c�݃G�t�F�N�g����
    void RefractionControl()
    {

        for (int i = 0; i < hitNum; i++)
        {

            if (refraction[i].isEffect)
            {
                if (refraction[i].count < AddSpeed)
                {
                    refraction[i].count++;
                    //�l�̐���
                    float result = Mathf.Lerp(0.0f, MaxScale, refraction[i].count / AddSpeed);
                    //�g��
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
    //�c�݃G�t�F�N�g�W�J
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