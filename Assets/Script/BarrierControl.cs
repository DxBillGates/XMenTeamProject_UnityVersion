using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControl : MonoBehaviour
{
    //盾を展開する向きのベクトル
    public Vector3 direction { get; set; }

    bool isOpen { get; set; }
    //盾のモデルをセットするオブジェクト
    [SerializeField] private GameObject barrierObject;
    //スポーンさせるクローン的なオブジェクト
    private GameObject barrieClone;
    
    //盾を展開する半径
    [SerializeField] private float radius;
    //展開する長さ(時間)
    [SerializeField] private int openSpan;
    //展開してからの経過時間
    private int time;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //経過時間が指定した時間を経過していなかったら間
        if (time < openSpan)
        {
            time++;
        }
        else
        {
            if (isOpen)
            {
                isOpen = false;
                Destroy(barrieClone);
                time = 0;
            }
        }
        //move ベクトルを度数法に変換
        float angle = Mathf.Atan2(direction.x, direction.z);
        Vector3 barrierPosition = transform.position + new Vector3(Mathf.Sin(angle) * radius, 0.0f, Mathf.Cos(angle) * radius);

        if (!isOpen)
        {
            //スポーン
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isOpen = true;
                barrieClone = Instantiate(barrierObject, barrierPosition, new Quaternion(0, 1, 0, angle));
            }
        }
        //展開中なら
        if (isOpen)
        {
            barrieClone.transform.rotation = Quaternion.LookRotation(direction);
            barrieClone.transform.position = barrierPosition+new Vector3(0,-3,0);
        }
    }
}
