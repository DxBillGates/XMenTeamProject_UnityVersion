using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.Rendering;

public class DomeEffectControl : MonoBehaviour
{
    private PostProcessVolume postProcessVolume;

    // Start is called before the first frame update
    private Bloom bloom;
    [SerializeField] float intensity = 0.0f;
    [SerializeField] float threshold = 0.0f;
    [SerializeField] Color color;
    void Start()
    {
        // Bloom効果のインスタンスの作成
        bloom = ScriptableObject.CreateInstance<Bloom>();
        //書き換えを可能にしている
        bloom.enabled.Override(true);
        bloom.intensity.Override(intensity);
        bloom.threshold.Override(threshold);
        bloom.color.Override(color);
        //　ポストプロセスボリュームに反映
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, bloom);
    }
    private void Update()
    {
        bloom.color.Override(color);

    }
    void OnDestroy()
    {
        //　作成したボリュームの削除
        RuntimeUtilities.DestroyVolume(postProcessVolume, true, true);
    }
    public void SetBloom(float intencity)
    {
        bloom.intensity.Override(intencity);
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, bloom);

    }
}