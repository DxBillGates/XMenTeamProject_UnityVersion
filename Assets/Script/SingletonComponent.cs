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

            // Tのクラスをアタッチしているオブジェクトのそのクラスを参照
            instance = (T)FindObjectOfType(t);

            // 誰もアタッチしていないなら
            if(instance == null)
            {
                Debug.LogError(t + "をアタッチしているGameObjectはありません");
            }
        }

        return instance;
    }

    // 他のインスタンスにアタッチされているか調べ、アタッチされている場合は破棄する
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
