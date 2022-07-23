using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitChangeScene : MonoBehaviour
{
    // 待機時間が一定以上だとシーンを変える
    [SerializeField] private float waitTimer = 10;

    [SerializeField] private NextScene nextScene;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // 経過時間計測用
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間用
        float nowTime = Time.time;

        if(Input.anyKey)
        {
            startTime = Time.time;
        }

        // 指定時間何も操作されてなかったら
        if(nowTime - startTime > waitTimer)
        {
            nextScene.nextSceneName = "TitleScene";
            nextScene.gameObject.SetActive(true);
        }
    }
}
