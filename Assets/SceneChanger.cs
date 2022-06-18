using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{

    //シーンチェンジオブジェクト
    [SerializeField] GameObject sceneChange;

    private int count;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ++count;
        if(count>150&&Input.GetButtonDown("PlayerAbility"))
        {
            sceneChange.SetActive(true);
        }
    }
}
