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
    [SerializeField] NextScene sceneChange;                     //�V�[���`�F���W�I�u�W�F�N�g
    [SerializeField] int rotationCount = 2;                     //�X�e�[�W���莞�̃I�u�W�F�N�g��]��

    static int staticStageCount;                                //�X�e�[�W���@�����ꎩ���J�E���g�ł���悤�ɂ�����
    static int nowSelectStageNum = 0;                           //���ݑI�𒆂̃X�e�[�W�C���f�b�N�X
    bool isMoveLeft = false;                                    //�o���A�����ɓ����Ă��邩
    float moveTimer = 0.75f;                                    //�ړ��^�C�}�[
    bool isStartMoveTimer = false;                              //�ړ��^�C�}�[���J�n���Ă��邩
    float decideTimer = 0;                                      //�ړ��^�C�}�[
    bool isStartDecideTimer = false;                            //�ړ��^�C�}�[���J�n���Ă��邩

    [SerializeField] GameObject circleShadowScriptObject;       //�ۉe�̃X�N���v�g�擾�p

    protected override void Awake()
    {
        staticStageCount = stageCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stageCount; i++)
        {
            circleShadowScriptObject.GetComponent<CircleShadow>().AddObject(Instantiate(barrierPrefab));
        }
        moveTimer = 0.5f;
        decideTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�X�V
        UpdateTimer();

        //���E�L�[�������ꂽ�Ƃ��A��]������
        if (isStartMoveTimer == false)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                nowSelectStageNum++;
                moveTimer = 0;
                isStartMoveTimer = true;
                isMoveLeft = false;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                nowSelectStageNum--;
                moveTimer = 0;
                isStartMoveTimer = true;
                isMoveLeft = true;
            }
        }

        //Stagenn�̃X�v���C�g�Z�b�g
        SetSpriteNum(GetNowSelectStageNum(true) + 1);

        //����
        if (Input.GetButtonDown("PlayerAbility"))
        {
            isStartDecideTimer = true;
        }
        //���o�r���Ŏ��̃V�[����
        if (decideTimer >= 1.0f && sceneChange.gameObject.activeSelf == false)
        {
            //�I�΂ꂽ�X�e�[�W�V�[���ɑJ��
            sceneChange.nextSceneName = "Stage" + (GetNowSelectStageNum(true) + 1).ToString();
            sceneChange.gameObject.SetActive(true);
        }

        Debug.Log(sceneChange.gameObject.activeSelf);
    }

    void UpdateTimer()
    {
        if (isStartMoveTimer)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > 0.75f)
            {
                moveTimer = 0.75f;
                isStartMoveTimer = false;
            }
        }
        if (isStartDecideTimer)
        {
            decideTimer += Time.deltaTime;
            if (decideTimer > 2.5f)
            {
                decideTimer = 2.5f;
                isStartDecideTimer = false;
            }
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

            return staticStageCount != 0 ? result % staticStageCount : 0;
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

    public float GetMoveTimer()
    {
        return moveTimer;
    }

    public int GetRotationCount()
    {
        return rotationCount;
    }

    public bool IsStartMoveTimer()
    {
        return isStartMoveTimer;
    }

    public float GetDecideTimer()
    {
        return decideTimer;
    }
    public bool IsStartDecideTimer()
    {
        return isStartDecideTimer;
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
