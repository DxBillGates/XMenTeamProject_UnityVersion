using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : SingletonComponent<StageSelectManager>
{
    [SerializeField] int stageCount = 5;
    [SerializeField] Vector3 center = new Vector3(0, 0, 20);    //��]�̒��~���W
    [SerializeField] float radius = 20;                         //��]����~�̔��a
    [SerializeField] GameObject barrierPrefab;                  //�o���A�v���n�u
    [SerializeField] List<Image> UIStageNums;                   //�I�𒆂̃X�e�[�W�i���o�[UI
    [SerializeField] List<Sprite> sprNums;                      //0�`9�̐���
    [SerializeField] GameObject sceneChange;                    //�V�[���`�F���W�I�u�W�F�N�g

    static int staticStageCount;                        //�X�e�[�W���@�����ꎩ���J�E���g�ł���悤�ɂ�����
    static int nowSelectStageNum = 0;                                  //���ݑI�𒆂̃X�e�[�W�C���f�b�N�X
    [SerializeField] float timer = 0.5f;                        //���o�^�C�}�[
    bool isStartTimer = false;                                  //�^�C�}�[���J�n���Ă��邩
    bool isMoveLeft = false;                                    //�o���A�����ɓ����Ă��邩


    protected override void Awake()
    {
        staticStageCount = stageCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stageCount; i++)
        {
            Instantiate(barrierPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���E�L�[�������ꂽ�Ƃ��A��]������
        if (isStartTimer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                nowSelectStageNum--;
                timer = 0;
                isStartTimer = true;
                isMoveLeft = false;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                nowSelectStageNum++;
                timer = 0;
                isStartTimer = true;
                isMoveLeft = true;

            }
        }
        if (timer > 0.5f)
        {
            timer = 0.5f;
            isStartTimer = false;
        }

        SetSpriteNum(GetNowSelectStageNum(true) + 1);

        //����
        if (Input.GetButtonDown("PlayerAbility"))
        {
            //�ǂ̐����w�肵�č�蒼��
            sceneChange.SetActive(true);
        }
    }

    public int GetStageCount()
    {
        return stageCount;
    }

    static public int GetNowSelectStageNum(bool isNormalize)
    {
        int result = nowSelectStageNum;
        if (isNormalize)
        {
            while (result < 0)
            {
                result += staticStageCount;
            }

            return result % staticStageCount;
        }
        else
        {
            return nowSelectStageNum;
        }

    }

    public float GetRadius()
    {
        return radius;
    }

    public Vector3 GetCenterPos()
    {
        return center;
    }

    public float GetTimer()
    {
        return timer;
    }

    public bool IsStartTimer()
    {
        return isStartTimer;
    }

    public bool IsMoveLeft()
    {
        return isMoveLeft;
    }

    void SetSpriteNum(int num)
    {
        if (num > 99)
        {
            num = 99;
        }
        else if (num < 0)
        {
            num = 0;
        }

        int cal = num;
        int index = cal % 10;
        //��̈�
        UIStageNums[0].sprite = sprNums[index];

        cal /= 10;
        index = cal % 10;
        //�\�̈�
        UIStageNums[1].sprite = sprNums[index];
    }
}
