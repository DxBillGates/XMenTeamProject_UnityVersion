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

    private void OnTriggerEnter(Collider collision)
    {
        // É{Å[ÉãÇ…ìñÇΩÇ¡ÇΩÇ∆Ç´ÇÃèàóù
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            if (ballComponent.GetSpeed() > 0)
            {
                Hp -= collision.gameObject.GetComponent<Ball>().GetSpeed();
                if (Hp <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
