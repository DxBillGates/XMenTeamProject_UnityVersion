using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeControl : MonoBehaviour
{
    //盾を展開する向きのベクトル

    bool isOpen { get; set; }
    //盾のモデルをセットするオブジェクト
    [SerializeField] private GameObject domeObject;
    //スポーンさせるクローン的なオブジェクト
    private GameObject domeClone;

    //展開する長さ(時間)
    [SerializeField] private int openSpan;
    //展開してからの経過時間
    private int time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        domeObject.transform.localScale = new Vector3(8,8,8);
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
                Destroy(domeClone);
                time = 0;
            }
        }
        Vector3 barrierPosition = transform.position;

        if (!isOpen)
        {
            //スポーン
            if (Input.GetKeyDown(KeyCode.O))
            {
                isOpen = true;
                domeClone = Instantiate(domeObject, barrierPosition, new Quaternion(0, 0, 0, 0));
            }
        }

    }
}
