using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearBGscript : MonoBehaviour
{
    // �N���A���̔w�i������
    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject clearImage;

    [SerializeField] private GameObject player;

    private RectTransform imageRect;
    private float startTime;

    private bool easingStart = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        imageRect = bgImage.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // ����G�t�F�N�g�I�������A�C�[�W���O�J�n��
        if (player.GetComponent<ClearPlayerMove>().IsEffectEnd && !easingStart)
        {
            startTime = Time.time;
            easingStart = true;

            bgImage.GetComponent<Image>().enabled = true;
        }

        if (easingStart)
        {

            // ���݂̃C�[�W���O�J�n����̃g�[�^���J�n����
            float totalTime = Time.time - startTime;
            
            // timeRate���P�̂Ƃ�
            if (totalTime >= 1)
            {
                totalTime = 1;

                clearImage.GetComponent<Image>().enabled = true;
            }
            Vector2 easingSize = Easing(totalTime, new Vector2(0, 900), new Vector2(0, 0));

            // �摜�̑傫���p
            //imageRect.sizeDelta = easingSize;
            imageRect.anchoredPosition = new Vector3(0, easingSize.y, 0);
        }
    }

    private Vector2 Easing(float timeRate, Vector2 min, Vector2 max)
    {
        Vector2 result;
        max -= min;

        result.x = max.x * timeRate * timeRate * timeRate * timeRate + min.x;
        result.y = max.y * timeRate * timeRate * timeRate * timeRate + min.y;

        return result;
    }
}
