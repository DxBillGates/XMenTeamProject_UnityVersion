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
    //”¼Œa
    [SerializeField] private float radius;
    //‚‚ğ“WŠJ‚·‚é’·‚³
    [SerializeField] private int openSpan;
    //“WŠJ‚µ‚Ä‚¢‚é’·‚³
    int time;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //“WŠJ’†
        if (time < openSpan)
        {
            time++;
        }
        //“WŠJ‚µ‚Ä‚È‚¢ŠÔ
        else
        {
            if (isOpen)
            {
                isOpen = false;
                Destroy(barrieClone);
                time = 0;
            }
        }
        //‘Ì‚ÌŒü‚«‚ğmove‚©‚ç‹‚ß‚é
        float angle = Mathf.Atan2(move.x, move.z);
        Vector3 
        //ƒoƒŠƒA‚ğ“WŠJ‚·‚éÀ•W
        if (Input.GetKeyDown(KeyCode.Space))
        {
            barrieClone = Instantiate(barrierObject, new Vector3(-5.0f, 0.0f, 0.0f), new Quaternion(0, 1, 0, angle));
        }

        barrieClone.transform.rotation = new Quaternion(0, 1, 0, angle);
        barrieClone. transform.position = );
    }
}
