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
        // Bloom���ʂ̃C���X�^���X�̍쐬
        bloom = ScriptableObject.CreateInstance<Bloom>();
        //�����������\�ɂ��Ă���
        bloom.enabled.Override(true);
        bloom.intensity.Override(intensity);
        bloom.threshold.Override(threshold);
        bloom.color.Override(color);
        //�@�|�X�g�v���Z�X�{�����[���ɔ��f
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, bloom);
    }
    private void Update()
    {
        bloom.color.Override(color);
        //�@�|�X�g�v���Z�X�{�����[���ɔ��f
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, bloom);
    }
    void OnDestroy()
    {
        //�@�쐬�����{�����[���̍폜
        RuntimeUtilities.DestroyVolume(postProcessVolume, true, true);
    }
    public void SetBloom(float intencity)
    {
        bloom.intensity.Override(intencity);
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, bloom);

    }
}