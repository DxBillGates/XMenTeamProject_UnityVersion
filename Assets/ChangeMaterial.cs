using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    //�ʏ펞�̃}�e���A��
    [SerializeField] private Material normalMat;
    //�A���e�B���b�g�X�L���W�J���̃}�e���A��(�����Â�����p)
    [SerializeField] private Material blackMat;
    private GameObject gameObject;

    private MeshRenderer meshRenderer;
    //Start is called before the first frame update
    void Start()
    {
        gameObject = GetComponent<GameObject>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = normalMat;

    }

    //Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SetBlackMaterial();
        }
    }
    public void SetNormalMaterial()
    {
        //���ʂ̃}�e���A�����Z�b�g
        meshRenderer.material = normalMat;
    }
    public void SetBlackMaterial()
    {
        //�����}�e���A�����Z�b�g
        meshRenderer.material = blackMat;

    }
}
