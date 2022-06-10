using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControl : MonoBehaviour
{
    Vector3 target { get; set; }
    Vector3 move { get; set; }
    bool isOpen { get; set; }
    [SerializeField] private GameObject barrierObject;
    private GameObject barrieClone;
    //半径
    [SerializeField] private float radius;
    //盾を展開する長さ
    [SerializeField] private int openSpan;
    //展開している長さ
    int time;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //展開中
        if (time < openSpan)
        {
            time++;
        }
        //展開してない間
        else
        {
            if (isOpen)
            {
                isOpen = false;
                Destroy(barrieClone);
                time = 0;
            }
        }
        //体の向きをmoveから求める
        float angle = Mathf.Atan2(move.x, move.z);
        Vector3 
        //バリアを展開する座標
        if (Input.GetKeyDown(KeyCode.Space))
        {
            barrieClone = Instantiate(barrierObject, new Vector3(-5.0f, 0.0f, 0.0f), new Quaternion(0, 1, 0, angle));
        }

        barrieClone.transform.rotation = new Quaternion(0, 1, 0, angle);
        barrieClone. transform.position = );
    }
}
