using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonComponent<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if(instance == null)
        {
            System.Type t = typeof(T);

            // T�̃N���X���A�^�b�`���Ă���I�u�W�F�N�g�̂��̃N���X���Q��
            instance = (T)FindObjectOfType(t);

            // �N���A�^�b�`���Ă��Ȃ��Ȃ�
            if(instance == null)
            {
                Debug.LogError(t + "���A�^�b�`���Ă���GameObject�͂���܂���");
            }
        }

        return instance;
    }

    // ���̃C���X�^���X�ɃA�^�b�`����Ă��邩���ׁA�A�^�b�`����Ă���ꍇ�͔j������
    virtual protected void Awake()
    {
        CheckInstance();
    }

    protected void CheckInstance()
    {
        if(instance == null)
        {
            instance = this as T;
            return;
        }
        else if(instance == this)
        {
            return;
        }

        Destroy(this);
        return;
    }
}
