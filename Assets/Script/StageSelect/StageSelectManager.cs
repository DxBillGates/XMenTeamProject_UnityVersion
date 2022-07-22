using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : SingletonComponent<StageSelectManager>
{
    [SerializeField] int stageCount = 3;
    [SerializeField] Vector3 center = new Vector3(0, 0, 20);    //��]�̒��~���W
    [SerializeField] float radius = 20;                         //��]����~�̔��a
    [SerializeField] GameObject barrierPrefab;                  //�o���A�v���n�u
    [SerializeField] List<Image> UIStageNums;                   //�I�𒆂̃X�e�[�W�i���o�[UI
    [SerializeField] List<Sprite> sprNums;                      //0�`9�̐���
    [SerializeField] NextScene sceneChange;                     //�V�[���`�F���W�I�u�W�F�N�g
    [SerializeField] int rotationCount = 2;                     //�X�e�[�W���莞�̃I�u�W�F�N�g��]��
    [SerializeField] float limitMoveTimer = 1.5f;               //�ړ��^�C�}�[�̏���l
    [SerializeField] float limitDecideTimer = 2.5f;             //����^�C�}�[�̏���l
    [SerializeField] List<Texture> textures;                  //���ɃZ�b�g����}�e���A��

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
            GameObject localScopeGameObject =  Instantiate(barrierPrefab);
            StageSelectBarrier barrier = localScopeGameObject.GetComponent<StageSelectBarrier>();
            barrier.SetStageNum(i);
            GameObject child = barrier.transform.Find("MeshSub").gameObject;
            child.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[i]);
            circleShadowScriptObject.GetComponent<CircleShadow>().AddObject(localScopeGameObject);
        }
        moveTimer = 0;
        decideTimer = 0;
        isStartDecideTimer = false;
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

        //�|�[�Y�{�^���Ń^�C�g����
        if (Input.GetButtonDown("Pause"))
        {
            sceneChange.nextSceneName = "TitleScene";
            sceneChange.gameObject.SetActive(true);
        }
    }

    void UpdateTimer()
    {
        if (isStartMoveTimer)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > limitMoveTimer)
            {
                moveTimer = limitMoveTimer;
                isStartMoveTimer = false;
            }
        }
        if (isStartDecideTimer)
        {
            decideTimer += Time.deltaTime;
            if (decideTimer > limitDecideTimer)
            {
                decideTimer = limitDecideTimer;
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

    public int GetRotationCount()
    {
        return rotationCount;
    }
    public bool IsMoveLeft()
    {
        return isMoveLeft;
    }
    public float GetMoveTimer()
    {
        return moveTimer;
    }

    public bool IsStartMoveTimer()
    {
        return isStartMoveTimer;
    }
    public float GetLimitMoveTimer()
    {
        return limitMoveTimer;
    }
    public float GetDecideTimer()
    {
        return decideTimer;
    }
    public bool IsStartDecideTimer()
    {
        return isStartDecideTimer;
    }
    public float GetLimitDecideTimer()
    {
        return limitDecideTimer;
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
