using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EasingType
{
    LERP,
    EASING,
}


// 必殺技発動時にカメラモーション（軸回転）起こす時間やし始めるアングルなどをまとめた構造体
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

    // カメラが必殺技発動付近まで行く時間(s)
    [SerializeField] private SkillCameraMotionInfo startMotionInfo;
    // カメラが必殺技発動付近でスローになる時間(s)
    [SerializeField] private SkillCameraMotionInfo centerMotionInfo;
    // カメラが元夫位置に行くまでの時間(s)
    [SerializeField] private SkillCameraMotionInfo endMotionInfo;

    [SerializeField] private float playerDistance;

    // カメラモーションに使用するデータ配列
    List<SkillCameraMotionInfo> motionInfos;
    // 現在使用しているカメラモーション配列の要素番号
    int currentSkillCameraMotionIndex;
    int beforeSkillCameraMotionIndex;
    // 前フレームの回転角度
    float beforeRotateAngle;

    // モーションの経過時間
    float motionElapsedTime;

    Vector3 originPosition;
    Quaternion originRotate;

    Vector3 ultTriggerPosition;

    [SerializeField] private Transform playerTransform;
    // カメラがもとの位置に戻る時間
    [SerializeField] private float backOriginPositionSetTime;

    // カメラがもとの位置に戻る経過時間
    private float backOriginPositionTime;
    private bool isBackOriginPosition;
    // スキル発動時のカメラの座標を保持する用
    private Vector3 skillCameraPosition;

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

        backOriginPositionTime = 0;
        isBackOriginPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.GetInstance().IsPause() == true) return;

        if(preUtlMotionFlag.flag == false)
        {
            transform.LookAt(playerTransform);
        }

        //UpdateUltMotion();
        UpdateSkillCameraMotion();
        preUtlMotionFlag.Update(Time.deltaTime);

        if (isBackOriginPosition == false) return;
        if(backOriginPositionTime >= backOriginPositionSetTime)
        {
            isBackOriginPosition = false;
            backOriginPositionTime = 0;
            Camera.main.transform.position = originPosition;
            return;
        }

        Camera.main.transform.position = Vector3.Lerp(skillCameraPosition, originPosition, EaseOutExpo(backOriginPositionTime));

        backOriginPositionTime += Time.deltaTime;
    }

    public void StartPreUltMotion(Vector3 triggerPosition,Transform ultTrigger)
    {
        preUtlMotionFlag.flag = true;
        preUtlMotionFlag.maxActiveTime = UltimateSkillManager.GetInstance().GetActiveFlagController().maxActiveTime;
        ultTriggerPosition = triggerPosition;
    }

    private void UpdateSkillCameraMotion()
    {
        if (preUtlMotionFlag.flag == false) return;

        Transform cameraTransform = Camera.main.transform;

        if (IsAnimationStartTrigger() == true) UltimateSkillGenerator.GetInstance().StartAddScale();
        beforeSkillCameraMotionIndex = currentSkillCameraMotionIndex;

        // 現在使用しているモーションデータの最大時間に達しているならモーションデータの要素番号を進める
        if (motionElapsedTime > motionInfos[currentSkillCameraMotionIndex].motionTime)
        {
            motionElapsedTime = 0;
            currentSkillCameraMotionIndex++;

            // モーションデータがこれ以降無いならモーションフラグを終了させ初期化を行う
            if(currentSkillCameraMotionIndex > motionInfos.Count - 1)
            {
                preUtlMotionFlag.Initialize();
                currentSkillCameraMotionIndex = 0;
                transform.position = originPosition;
                transform.rotation = originRotate;
                cameraTransform.rotation = originRotate;
                beforeRotateAngle = 0;
                motionElapsedTime = 0;
                return;
            }
        }


        float time = motionElapsedTime / motionInfos[currentSkillCameraMotionIndex].motionTime;
        float lerpTime = motionInfos[currentSkillCameraMotionIndex].easingType == EasingType.LERP ? time :
                                                                                                    EaseOutExpo(time);
        // 回転角度を取得
        float rotateAngle = Mathf.Lerp(motionInfos[currentSkillCameraMotionIndex].startDegreeAngle,
                                       motionInfos[currentSkillCameraMotionIndex].endDegreeAngle,
                                       lerpTime);

        float rotateAnglePerFrame = rotateAngle - beforeRotateAngle;

        const float HALF_LERP_POINT = 0.5f;
        // 回転軸の位置を取得
        Vector3 rotateAxisPosition = Vector3.Lerp(ultTriggerPosition, originPosition, HALF_LERP_POINT);
        // 発動場所へのベクトルを取得
        //Vector3 toTriggerPositionVector = ultTriggerPosition - originPosition;
        // 回転軸を計算
        Vector3 rotateAxis = transform.up;


        Quaternion rotate = Quaternion.AngleAxis(rotateAnglePerFrame, rotateAxis.normalized);
        Vector3 pos = transform.position;
        pos -= rotateAxisPosition;
        pos = rotate * pos;
        pos += rotateAxisPosition;

        transform.position = pos;
        transform.rotation = rotate * transform.rotation;

        // 後で修正
        cameraTransform.position = pos;
        cameraTransform.rotation = rotate * cameraTransform.rotation;

        if (currentSkillCameraMotionIndex == 1)
        {
            cameraTransform.LookAt(playerTransform);
        }
        if(currentSkillCameraMotionIndex == 2)
        {
            Vector3 playerPos = playerTransform.position + Vector3.up * UltimateSkillGenerator.GetInstance().GetCreatedObjectScale();
            playerPos -= Vector3.forward * playerDistance;
            playerPos.y = originPosition.y;

            cameraTransform.position = playerPos;
            skillCameraPosition = playerPos;
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

    public void Initialize()
    {
        Transform cameraTransform = Camera.main.transform;
        preUtlMotionFlag.Initialize();
        currentSkillCameraMotionIndex = 0;
        transform.position = originPosition;
        transform.rotation = originRotate;
        cameraTransform.position = originPosition;
        cameraTransform.rotation = originRotate;
        beforeRotateAngle = 0;
        motionElapsedTime = 0;
    }

    public void BackOriginPosition()
    {
        isBackOriginPosition = true;
    }
}
