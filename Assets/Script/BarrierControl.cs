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
    //展開する長さ(時間)
    [SerializeField] private int openSpan;
    //
    [SerializeField] private int angle=0;
    //展開してからの経過時間
    int time;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3();
        time=0;
    }

    // Update is called once per frame
    void Update()
    {
        //生きている間
        if (time < openSpan)
        {
            time++;
        }
        else
        {
            if (isOpen)
            {
                isOpen = false;
               //Destroy(barrieClone);
                time = 0;
            }
        }
        //move ベクトルを度数法に変換
        //float angle = Mathf.Atan2(move.x, move.z);
        Vector3 barrierPosition = target + new Vector3(Mathf.Sin(angle) * radius, 0.0f, Mathf.Cos(angle) * radius);

       //スポーン
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isOpen = true;
            barrieClone = Instantiate(barrierObject, barrierPosition, new Quaternion(0, 1, 0, angle));
        }
       //展開中なら
        if (isOpen)
        {
            barrieClone.transform.rotation = new Quaternion(0, 1, 0, angle);
            barrieClone.transform.position = barrierPosition;
        }
    }
}
