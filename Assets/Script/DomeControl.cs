using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeControl : MonoBehaviour
{
    //����W�J��������̃x�N�g��

    bool isOpen { get; set; }
    //���̃��f�����Z�b�g����I�u�W�F�N�g
    [SerializeField] private GameObject domeObject;
    //�X�|�[��������N���[���I�ȃI�u�W�F�N�g
    private GameObject domeClone;

    //�W�J���钷��(����)
    [SerializeField] private int openSpan;
    //�W�J���Ă���̌o�ߎ���
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
        //�o�ߎ��Ԃ��w�肵�����Ԃ��o�߂��Ă��Ȃ��������
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
            //�X�|�[��
            if (Input.GetKeyDown(KeyCode.O))
            {
                isOpen = true;
                domeClone = Instantiate(domeObject, barrierPosition, new Quaternion(0, 0, 0, 0));
            }
        }

    }
}
