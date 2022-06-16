using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EasingType
{
    LERP,
    EASING,
}


// �K�E�Z�������ɃJ�������[�V�����i����]�j�N�������Ԃ₵�n�߂�A���O���Ȃǂ��܂Ƃ߂��\����
[System.Serializable]
public struct SkillCameraMotionInfo
{
    public float motionTime;
    public float startDegreeAngle;
    public float endDegreeAngle;
    public EasingType easingType;
}


public class CameraMotionManager : SingletonComponent<CameraMotionManager>
{
    [SerializeField] private FlagController preUtlMotionFlag;

    // �J�������K�E�Z�����t�߂܂ōs������(s)
    [SerializeField] private SkillCameraMotionInfo startMotionInfo;
    // �J�������K�E�Z�����t�߂ŃX���[�ɂȂ鎞��(s)
    [SerializeField] private SkillCameraMotionInfo centerMotionInfo;
    // �J���������v�ʒu�ɍs���܂ł̎���(s)
    [SerializeField] private SkillCameraMotionInfo endMotionInfo;

    // �J�������[�V�����Ɏg�p����f�[�^�z��
    List<SkillCameraMotionInfo> motionInfos;
    // ���ݎg�p���Ă���J�������[�V�����z��̗v�f�ԍ�
    int currentSkillCameraMotionIndex;
    int beforeSkillCameraMotionIndex;
    // �O�t���[���̉�]�p�x
    float beforeRotateAngle;

    // ���[�V�����̌o�ߎ���
    float motionElapsedTime;

    Vector3 originPosition;
    Quaternion originRotate;

    Vector3 ultTriggerPosition;
    Transform ultTriggerTransform;
    [SerializeField] Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = Camera.main.transform.position;
        originRotate = Camera.main.transform.rotation;

        transform.position = originPosition;
        transform.rotation = originRotate;

        motionInfos = new List<SkillCameraMotionInfo>();
        motionInfos.Add(startMotionInfo);
        motionInfos.Add(centerMotionInfo);
        motionInfos.Add(endMotionInfo);
        currentSkillCameraMotionIndex = 0;
        beforeSkillCameraMotionIndex = 0;
        motionElapsedTime = 0;
        beforeRotateAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(preUtlMotionFlag.flag == false)
        {
            playerTransform.position += playerTransform.forward + playerTransform.up * 5;
            transform.LookAt(playerTransform);
            playerTransform.position -= playerTransform.forward + playerTransform.up * 5;
        }

        //UpdateUltMotion();
        UpdateSkillCameraMotion();
        preUtlMotionFlag.Update(Time.deltaTime);
    }

    public void StartPreUltMotion(Vector3 triggerPosition,Transform ultTrigger)
    {
        preUtlMotionFlag.flag = true;
        preUtlMotionFlag.maxActiveTime = UltimateSkillManager.GetInstance().GetActiveFlagController().maxActiveTime;
        ultTriggerPosition = triggerPosition;
        ultTriggerTransform = ultTrigger;
    }

    private void UpdateSkillCameraMotion()
    {
        if (preUtlMotionFlag.flag == false) return;

        Transform cameraTransform = Camera.main.transform;

        if (IsAnimationStartTrigger() == true) UltimateSkillGenerator.GetInstance().StartAddScale();
        beforeSkillCameraMotionIndex = currentSkillCameraMotionIndex;

        // ���ݎg�p���Ă��郂�[�V�����f�[�^�̍ő厞�ԂɒB���Ă���Ȃ烂�[�V�����f�[�^�̗v�f�ԍ���i�߂�
        if (motionElapsedTime > motionInfos[currentSkillCameraMotionIndex].motionTime)
        {
            motionElapsedTime = 0;
            currentSkillCameraMotionIndex++;

            // ���[�V�����f�[�^������ȍ~�����Ȃ烂�[�V�����t���O���I���������������s��
            if(currentSkillCameraMotionIndex > motionInfos.Count - 1)
            {
                preUtlMotionFlag.Initialize();
                currentSkillCameraMotionIndex = 0;
                transform.position = originPosition;
                transform.rotation = originRotate;
                cameraTransform.position = originPosition;
                cameraTransform.rotation = originRotate;
                return;
            }
        }


        float time = motionElapsedTime / motionInfos[currentSkillCameraMotionIndex].motionTime;
        float lerpTime = motionInfos[currentSkillCameraMotionIndex].easingType == EasingType.LERP ? time :
                                                                                                    EaseOutExpo(time);
        // ��]�p�x���擾
        float rotateAngle = Mathf.Lerp(motionInfos[currentSkillCameraMotionIndex].startDegreeAngle,
                                       motionInfos[currentSkillCameraMotionIndex].endDegreeAngle,
                                       lerpTime);

        float rotateAnglePerFrame = rotateAngle - beforeRotateAngle;

        const float HALF_LERP_POINT = 0.5f;
        // ��]���̈ʒu���擾
        Vector3 rotateAxisPosition = Vector3.Lerp(ultTriggerPosition, originPosition, HALF_LERP_POINT);
        // �����ꏊ�ւ̃x�N�g�����擾
        //Vector3 toTriggerPositionVector = ultTriggerPosition - originPosition;
        // ��]�����v�Z
        Vector3 rotateAxis = transform.up;


        Quaternion rotate = Quaternion.AngleAxis(rotateAnglePerFrame, rotateAxis.normalized);
        Vector3 pos = transform.position;
        pos -= rotateAxisPosition;
        pos = rotate * pos;
        pos += rotateAxisPosition;

        transform.position = pos;
        transform.rotation = rotate * transform.rotation;

        // ��ŏC��
        cameraTransform.position = pos;
        cameraTransform.rotation = rotate * cameraTransform.rotation;

        if (currentSkillCameraMotionIndex == 1)
        {
            Vector3 toTriggerTransform = ultTriggerTransform.position - transform.position;
            cameraTransform.LookAt(playerTransform);
        }
        if(currentSkillCameraMotionIndex == 2)
        {
            cameraTransform.rotation = originRotate;
        }

        motionElapsedTime += Time.deltaTime;

        beforeRotateAngle = rotateAngle;
    }

    private float EaseOutExpo(float t)
    {
        return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }

    public FlagController GetFlagController()
    {
        return preUtlMotionFlag;
    }

    public bool IsAnimationTime()
    {
        return currentSkillCameraMotionIndex == 1;
    }

    public bool IsAnimationStartTrigger()
    {
        return beforeSkillCameraMotionIndex == 0 && currentSkillCameraMotionIndex == 1;
    }

    public float GetAnimationMaxTime()
    {
        return motionInfos[1].motionTime;
    }

}