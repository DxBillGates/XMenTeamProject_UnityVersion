using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    ////通常時のマテリアル
    //[SerializeField] private Material normalMat;
    ////アルティメットスキル展開時のマテリアル(回りを暗くする用)
    //[SerializeField] private Material blackMat;
    //private GameObject gameObject;

    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject = GetComponent<GameObject>();
        //meshRenderer = GetComponent<MeshRenderer>();

        //meshRenderer.material = normalMat;

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    //public void SetNormalMaterial()
    //{
    //    //普通のマテリアルをセット
    //    meshRenderer.material = normalMat;
    //}
    //public void SetBlackMaterial()
    //{
    //    //黒いマテリアルをセット
    //    meshRenderer.material = blackMat;

    //}
}
