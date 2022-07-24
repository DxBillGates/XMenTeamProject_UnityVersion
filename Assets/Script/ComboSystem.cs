using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboSystem
{
    // ���݂̃R���{��
    [SerializeField] private int combo;
    // �R���{��������񐔂ɓ��B������
    private int comboCount;

    // ���R���{���ɑ傫���������邩
    [SerializeField, Range(1, 10)] private int increaseSizeNCombo;
    // ���R���{���ɂǂꂾ���傫�������邩
    [SerializeField, Range(1, 10)] private float increaseSize;
    // �傫��������I�u�W�F�N�g�̍ő�T�C�Y
    [SerializeField] private float maxSize;

    // �R���{���ɑ傫��������I�u�W�F�N�g
    private GameObject ballObject;
    private Vector3 initSize;

    [SerializeField] private List<UnityEngine.UI.Image> images;
    [SerializeField] private List<Sprite> numTextures;
    [SerializeField] private UnityEngine.UI.Image prefabImage;

    // Start is called before the first frame update
    public void Initialize()
    {
        combo = 0;
        comboCount = 0;
        initSize = Vector3.zero;

        //// Inspector�ŃZ�b�g����Ă��Ȃ��ꍇ�͎�����UI���쐬����
        //if (images.Count == 0)
        //{
        //    // �O�����쐬
        //    images = new List<UnityEngine.UI.Image>();

        //    for (int i = 0; i < 3; ++i)
        //    {
        //        var image = GameObject.Instantiate(prefabImage);
        //        image.rectTransform.SetParent(GameObject.Find("Canvas").transform);
        //        images.Add(image);
        //    }
        //}
    }

    // Update is called once per frame
    public void Update()
    {
        if (images.Count == 0) return;

        int startNumber = 1000;
        int startNumber2 = 100;
        foreach(var image in images)
        {
            int textureIndex = 0;
            textureIndex = combo == 0 ? 0 : combo % startNumber / startNumber2;
            image.sprite = numTextures[textureIndex];
            startNumber /= 10;
            startNumber2 /= 10;
        }
    }

    public void SetBallObject(GameObject argObject)
    {
        ballObject = argObject;
        initSize = argObject.transform.localScale;
    }

    public void IncreaseCombo()
    {
        ++combo;

        // �R���{���{�[�����傫���Ȃ邽�߂ɕK�v�ȃR���{�������肳����
        if (combo > increaseSizeNCombo + increaseSizeNCombo * comboCount)
        {
            ++comboCount;
            ballObject.transform.localScale += Vector3.one * increaseSize;

            // ������UI���傫���Ȃ�n�߂�悤�ȏ������L�q


            // �I�u�W�F�N�g�̃T�C�Y�𑝉�������
            if (ballObject.transform.localScale.x >= maxSize)
            {
                ballObject.transform.localScale = Vector3.one * maxSize;
            }
        }
    }

    public void ResetCombo()
    {
        combo = 0;
        comboCount = 0;
        ballObject.transform.localScale = initSize;
    }
}
