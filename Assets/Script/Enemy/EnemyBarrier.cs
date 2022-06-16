using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrier : MonoBehaviour
{
    [SerializeField] private float Hp = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.name == "Ball")
        {
            Hp -= collision.gameObject.GetComponent<Ball>().GetSpeed();
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
