using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RaycastResult
{
    // �q�b�g�������W
    public Vector3 point { get; set; }
    // �q�b�g�����@��
    public Vector3 normal { get; set; }
    // ���C�L���X�g����Ƃ��Ɏg�p�����x�N�g���ƃq�b�g�����@������擾�������˃x�N�g��
    public Vector3 reflectVector { get; set; }
    // �q�b�g�ɂ�����������
    public float distance { get; set; }

    public bool isHit { get; set; }
}


public class PredictionalLineDrawer : MonoBehaviour
{
    // �\�����̍ő咷��
    [SerializeField, Range(0, 500)] private float maxLength;
    // �\������\�����Ă���I�u�W�F�N�g���m�̊Ԋu
    [SerializeField, Range(0, 100)] private float objectDistance;
    [SerializeField] private GameObject lineObjectPrefab;
    // �\������\������I�u�W�F�N�g�̑���
    [SerializeField] private int lineObjectCount;

    // �\������\�����Ă���I�u�W�F�N�g�z��
    private List<GameObject> lineObjects;
    // �\�������J�n�����_
    public Vector3 drawOriginPosition { private get; set; }
    // �\�����̕����x�N�g��
    public Vector3 drawDirection { private get; set; }

    public bool isDraw { get; set; }
    private float setObjectDistance;

    [SerializeField, Range(1, 10)] int raycastCount;
    int beforeRaycastCount;
    private List<RaycastResult> raycastResults;

    private void Awake()
    {
        // �\�����\���I�u�W�F�N�g��ǉ�
        lineObjects = new List<GameObject>();
        CreateLineObjects(lineObjectCount);

        drawOriginPosition = Vector3.zero;
        drawDirection = new Vector3(0, 0, 1);
        setObjectDistance = 0;

        beforeRaycastCount = raycastCount;
        raycastResults = new List<RaycastResult>();
        for (int i = 0; i <= raycastCount; ++i) raycastResults.Add(new RaycastResult());
    }

    // ���t���[�����C�L���X�g�̓G�O���������炱���ł��
    private void FixedUpdate()
    {
        FixRaycastCount();
        beforeRaycastCount = raycastCount;

        if (isDraw == false) return;

        // ���C�L���X�g�̉񐔂ƃ��C�L���X�g�̏�����
        Vector3 rayOriginPosition = drawOriginPosition;
        Vector3 rayDirection = drawDirection;
        float rayDistance = 0;
        for (int i = 0; i < raycastCount; ++i)
        {
            // ���C�̍ő勗���ƃ��C�L���X�g���Ă������ۂ̍ő�̒���������l�𒴂��Ă���Ȃ烌�C�L���X�g�͏I��
            if (maxLength - rayDistance < 0)
            {
                InitializeRaycastResults(i);
                break;
            }
            if (Physics.Raycast(rayOriginPosition, rayDirection, out RaycastHit raycastHit, maxLength - rayDistance))
            {
                //// �ǃI�u�W�F�N�g����Ȃ��Ȃ烌�C�L���X�g�̌��ʂ͊i�[���Ȃ�
                //if (!raycastHit.collider.gameObject.CompareTag("Wall"))
                //{
                //    InitializeRaycastResults(i);
                //    break;
                //}

                // ���̃��[�v�Ŏg�����C�L���X�g�̃x�N�g����
                // ���C�L���X�g�����ۂ̃x�N�g���ƕǂ̖@���Ŕ��˃x�N�g�����擾
                Vector3 reflectVector = rayDirection - 2.0f * Vector3.Dot(rayDirection, raycastHit.normal) * raycastHit.normal;

                // �����[�v�̃��C�L���X�g�p�ɒl�����[�J���ϐ��ɏ㏑��
                rayOriginPosition = raycastHit.point;
                rayDirection = reflectVector.normalized;
                rayDistance = raycastHit.distance;

                // �e���C�L���X�g�̌��ʂ�z��Ɋi�[
                RaycastResult raycastResult = new RaycastResult
                {
                    point = rayOriginPosition,
                    normal = raycastHit.normal,
                    reflectVector = rayDirection,
                    distance = rayDistance,
                    isHit = true
                };
                raycastResults[i] = raycastResult;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        setObjectDistance = isDraw ? objectDistance : 0;
        UpdateLineObjects();
    }

    private void CreateLineObjects(int createValue)
    {
        for (int i = 0; i < createValue; ++i)
        {
            lineObjects.Add(Instantiate(lineObjectPrefab));
            lineObjects[i].transform.parent = transform;
        }
    }

    private void UpdateLineObjects()
    {
        // ���C�L���X�g�̔��ˉ񐔌��ʂ��������߂̃C���f�b�N�X
        int reflectCount = 0;
        // ���˂𔽉f���n�߂�z��̗v�f�ԍ�
        int reflectStartIndex = 0;

        Vector3 setPosition = drawOriginPosition;
        Vector3 useDirection = drawDirection;
        // ���C���I�u�W�F�N�g�����[�v���čX�V
        for (int i = 0; i < lineObjects.Count; ++i)
        {
            if (isDraw == true) lineObjects[i].SetActive(true);
            else lineObjects[i].SetActive(false);

            // ���ˉ񐔂����˂̌��ʂ��i�[���Ă���z��̗v�f�����傫���Ȃ�Ȃ��悤��
            if (reflectCount >= raycastResults.Count)
            {
                lineObjects[i].transform.position = setPosition;
                continue;
            }
            if (raycastResults[reflectCount].isHit == false)
            {
                lineObjects[i].transform.position = setPosition;
                continue;
            }

            Vector3 direction = (i - reflectStartIndex) * setObjectDistance * useDirection;

            // �x�N�g���̒��������C�L���X�g���ʂ̋�����蒷���Ȃ甽�ˉ񐔂��C���N�������g
            // setPosition useDirection���X�V
            if (direction.magnitude > raycastResults[reflectCount].distance)
            {

                setPosition = raycastResults[reflectCount].point;
                useDirection = raycastResults[reflectCount].reflectVector;
                reflectStartIndex = i;

                //Debug.DrawRay(setPosition, useDirection * 10, Color.red);
                //Debug.Log(reflectCount);
                ++reflectCount;
            }

            lineObjects[i].transform.position = setPosition + (i - reflectStartIndex) * setObjectDistance * useDirection;
        }
    }

    private void InitializeRaycastResults(int startIndex = 0)
    {
        for(int i = startIndex;i < raycastCount;++i)
        {
            RaycastResult raycastResult = new RaycastResult()
            {
                point = Vector3.zero,
                normal = Vector3.zero,
                reflectVector = Vector3.zero,
                distance = 0,
                isHit = false
            };
            raycastResults[i] = raycastResult;
        }
    }

    // �G�f�B�^�[���烌�C�L���X�g�̉񐔂𑝂₵���ۂɔz��I�[�o�[�ɂȂ��Ă��܂�����
    // ������������֐�
    private void FixRaycastCount()
    {
        if (beforeRaycastCount == raycastCount) return;

        raycastResults.Clear();
        Debug.Log(raycastResults.Count);
        for (int i = 0; i <= raycastCount; ++i) raycastResults.Add(new RaycastResult());
    }
}
