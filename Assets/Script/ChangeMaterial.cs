using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    //�ʏ펞�̃}�e���A��
    [SerializeField] private Material normalMat;
    //�A���e�B���b�g�X�L���W�J���̃}�e���A��(�����Â�����p)
    [SerializeField] private Material blackMat;

    private MeshRenderer meshRenderer;
    //Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = normalMat;

    }

    //Update is called once per frame
    void Update()
    {
 
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
