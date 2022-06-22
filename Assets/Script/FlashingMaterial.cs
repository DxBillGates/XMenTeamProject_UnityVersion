using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ł�������}�e���A�����Ԋu�⎞�ԁA�F���w�肵�ē_�ł�����N���X
[System.Serializable]
public class FlashingMaterial
{
    // �_�ŐF
    [SerializeField] private Color color;
    private Color originalColor;

    // �_�ŊԊu�t���O
    [SerializeField] private FlagController flashingSpanFlagController;
    // �_�Ŏ��ԃt���O
    [SerializeField] private FlagController flashingFlagController;

    // �Q�Ƃ���}�e���A��
    private Material refMaterial;
    private List<Color> setColors;
    private bool changeFlag;

    public void Initialize()
    {
        flashingSpanFlagController.Initialize();
        flashingFlagController.Initialize();

        if (setColors == null)
        {
            setColors = new List<Color>();
            setColors.Add(color);
            setColors.Add(new Color());
        }

        changeFlag = false;
    }

    public void Update(float deltaTime)
    {
        if (refMaterial == null) return;

        if (flashingFlagController.IsEndTrigger() == true)
        {
            RestoreMaterialColor();
            Initialize();
            return;
        }

        if (flashingFlagController.flag == false) return;

        // �_�ł̃t���O���Ǘ����Ă���t���O�R���g���[���[��
        if (flashingSpanFlagController.flag == false) flashingSpanFlagController.flag = true;

        if(flashingSpanFlagController.IsEndTrigger() == true)
        {
            // �F�ύX�Ɏg���t���O��؂�ւ���
            changeFlag = !changeFlag;

            // �v�f�ԍ�0,1��؂�ւ����
            refMaterial.color = setColors[System.Convert.ToInt32(changeFlag)];

            // �_�Ńt���O�Ǘ���������
            flashingSpanFlagController.Initialize();
        }

        // �_�Ńt���O�Ǘ��̍X�V
        flashingSpanFlagController.Update(deltaTime);
        // �t���O�S�̊Ǘ��̍X�V
        flashingFlagController.Update(deltaTime);
    }

    public void SetMaterial(Material material)
    {
        refMaterial = material;
        originalColor = material.color;

        setColors[1] = originalColor;
    }

    // �_�ŊǗ��t���O���I���ɂ���
    public void Flash()
    {
        flashingFlagController.Initialize();
        flashingSpanFlagController.Initialize();

        flashingFlagController.flag = true;
    }

    public void RestoreMaterialColor()
    {
        if (refMaterial == null) return;
        refMaterial.color = originalColor;
    }
}
