using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectManager : SingletonComponent<FieldObjectManager>
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private uint wallValue;
    [SerializeField] private bool isRotate;

    // �X�V���ɐV�����ǂ�����Ă݂����Ƃ��p
    [SerializeField] private bool reCreate;

    List<GameObject> wallObjects;
    // Start is called before the first frame update
    void Start()
    {
        wallObjects = new List<GameObject>();
        CreateWall();
    }

    // Update is called once per frame
    void Update()
    {
        if (reCreate)
        {
            CreateWall();
            reCreate = false;
        }
    }

    // start��awake�ŌĂяo���Ă�������
    private void CreateWall()
    {
        foreach (var gameObject in wallObjects)
        {
            Destroy(gameObject);
        }

        wallObjects.Clear();

        for (uint i = 0; i < wallValue; i++)
        {
            uint nextIndex = i + 1;

            if (nextIndex > wallValue) nextIndex = 0;

            Vector3 currntCalcPosition = CalclatePosition(wallValue, i);
            Vector3 nextCalcPosition = CalclatePosition(wallValue, nextIndex);
            const float HALF_LERP_POINT = 0.5f;

            GameObject newWallObject = Instantiate(wallPrefab
                                                 , Vector3.Lerp(currntCalcPosition, nextCalcPosition, HALF_LERP_POINT)
                                                 , new Quaternion());

            // �|�W�V�����𒆐S�_��y����ANGLE�x��]
            if (isRotate)
            {
                RotatePosition(newWallObject.transform);
            }

            // ���̂܂܂ł�Rotate�����������̂Œ��S�������悤�ɏC��
            CalclateRotation(newWallObject.transform);
            ReSize(newWallObject.transform);
            RePosition(newWallObject.transform);

            wallObjects.Add(newWallObject);

            // �e�̐ݒ�
            newWallObject.transform.parent = transform;
        }
    }

    // n�p�`��value�Ԗڂ̃|�W�V������Ԃ�
    private Vector3 CalclatePosition(uint n, uint index)
    {
        const float TWO_PI = 2 * Mathf.PI;
        float x = Mathf.Sin((TWO_PI / n) * index);
        float y = 0;
        float z = Mathf.Cos((TWO_PI / n) * index);

        Vector3 returnValue = new Vector3(x, y, z);
        return returnValue;
    }

    // ���S�������悤�Ɏp���𐧌䂷��
    private void CalclateRotation(Transform argTransform)
    {
        // ���S�ւ̃x�N�g���𐶐�
        Vector3 toCenterVector = -argTransform.position.normalized;

        Vector3 rotateAxis = Vector3.up;
        float radian = Mathf.Atan2(toCenterVector.x, toCenterVector.z);
        float degree = radian * 180 / Mathf.PI;
        argTransform.rotation = Quaternion.AngleAxis(degree, rotateAxis);
    }

    // ������transform�̍��W���N���X�ɐݒ肳��Ă���n�p�`���Ƃɂ������l����]������
    private void RotatePosition(Transform argTransform)
    {
        float angle = Mathf.PI / wallValue;

        // ���W�A������p�x�ɏC��
        angle = angle * 180 / Mathf.PI;

        Vector3 axis = new Vector3(0, 1, 0);
        Quaternion rotateQuaternion = Quaternion.AngleAxis(angle, axis);

        Vector3 pos = argTransform.position;
        argTransform.position = rotateQuaternion * pos;
    }

    // n�p�`���Ƃ̕ӂ̒����ɑΉ�������
    private void ReSize(Transform argTransform)
    {
        float trueWidthSize = 2 * Mathf.Sin(Mathf.PI / wallValue) * wallPrefab.transform.localScale.x;
        Vector3 beforeScale = argTransform.transform.localScale;
        argTransform.transform.localScale = new Vector3(trueWidthSize, beforeScale.y, beforeScale.z);
    }

    // �ǂ̒����ɂ������ʒu�ɏC������
    private void RePosition(Transform argTransform)
    {
        float halfSize = wallPrefab.transform.localScale.x;
        argTransform.position *= halfSize;
    }

    public int GetFieldObjectsCount()
    {
        return wallObjects.Count;
    }

    public float GetWallSize()
    {
        return wallPrefab.transform.localScale.z;
    }
}
