using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitEffect : MonoBehaviour
{
    //�G�t�F�N�g�p�}�e���A��
    [SerializeField] Material material;
    //�}�e���A����ێ����Ă���I�u�W�F�N�g
    [SerializeField] GameObject gameObject;
    struct HitEffect
    {
        public GameObject cloneObject;
        public float count;
        public bool isEffect;

    }
    int num = 3;
    HitEffect[] hitEffect;

    float distance;
    //�g�傷�鑬��
    [SerializeField] float AddSpeed;
    //�G�t�F�N�g�̊g�傷��傫��
    [SerializeField] float MaxScale;

    // Start is called before the first frame update
    void Start()
    {
        hitEffect = new HitEffect[5];
        for (int i = 0; i < num; i++)
        {
            hitEffect[i].count = 0.0f;
            hitEffect[i].isEffect = false;

        }
        distance = 27.0f;
        AddSpeed = 10.0f;
        MaxScale = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < num; i++)
        {

            if (hitEffect[i].isEffect)
            {
                if (hitEffect[i].count < AddSpeed)
                {
                    hitEffect[i].count++;
                    //�l�̐���
                    float result = Mathf.Lerp(0.0f, MaxScale, hitEffect[i].count / AddSpeed);
                    //�g��
                    hitEffect[i].cloneObject.transform.localScale = new Vector3(result, result, result);
                }
                else
                {
                    hitEffect[i].isEffect = false;
                    Destroy(hitEffect[i].cloneObject);
                    hitEffect[i].count = 0.0f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Use(Vector3.zero);
        }

        material.SetFloat("_Distance", distance);
    }

    public void SetDistance(float f)
    {
        distance = f < 0 ? 0 : f > 100.0f ? 100.0f : f;
    }
    public void Use(Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {

            if (!hitEffect[i].isEffect)
            {
                hitEffect[i].isEffect = true;
                hitEffect[i].cloneObject = Instantiate(gameObject, position, new Quaternion(0, 1, 0, 0));
                break;
            }
        }
    }
}