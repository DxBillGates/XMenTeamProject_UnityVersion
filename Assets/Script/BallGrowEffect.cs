using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGrowEffect : MonoBehaviour
{
    //エフェクトの親オブジェクト
    [SerializeField] GameObject obj;
    //ボール
    [SerializeField] GameObject ball;
    //移動する速さ
    [SerializeField] float AddSpeed;
    //開始地点のばらつき
    [SerializeField] float radius;
    //個数
    int num = 5;
    //ボール拡大エフェクトのクローン
    GameObject[] ballGlowCloneObject;

    Vector3[] randPos;
    //
    float count;
    //
    bool isEffect;

    //バリアのポジション
    Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        ballGlowCloneObject = new GameObject[num];

        randPos = new Vector3[num];
        count = 0.0f;
        isEffect = false;

        for (int i = 0; i < num; i++)
        {
            randPos[i] = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEffect)
        {
            if (count < AddSpeed)
            {
                count++;
                float division = (628.0f / ballGlowCloneObject.Length) / 100;

                for (int i = 0; i < ballGlowCloneObject.Length; i++)
                {
                    //開始地点のばらつき
                    //Vector3 randPos = new Vector3(Mathf.Sin(i * division) * radius, 0.0f, Mathf.Cos(i * division) * radius);
                    ballGlowCloneObject[i].transform.position = new Vector3(
                        Mathf.Lerp(startPosition.x + randPos[i].x, ball.transform.position.x, count / AddSpeed),
                        Mathf.Lerp(startPosition.y + randPos[i].y, ball.transform.position.y, count / AddSpeed),
                        Mathf.Lerp(startPosition.z + randPos[i].z, ball.transform.position.z, count / AddSpeed));
                }
            }
            else
            {
                for (int i = 0; i < ballGlowCloneObject.Length; i++)
                {
                    Destroy(ballGlowCloneObject[i]);
                }
                isEffect = false;
                count = 0.0f;
            }

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Use(Vector3.zero);
        }
    }
    public void Use(Vector3 pos)
    {
        if (!isEffect)
        {
            startPosition = pos;
            for (int i = 0; i < ballGlowCloneObject.Length; i++)
            {
                ballGlowCloneObject[i] = Instantiate(obj);
            }
            isEffect = true;
        }
    }
}


