using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private uint wallValue;
    [SerializeField] private bool isRotate;

    // 更新中に新しく壁を作ってみたいとき用
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

    // startかawakeで呼び出してください
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

            // ポジションを中心点のy軸でANGLE度回転
            if (isRotate)
            {
                RotatePosition(newWallObject.transform);
            }

            // このままではRotateがおかしいので中心を向くように修正
            CalclateRotation(newWallObject.transform);
            ReSize(newWallObject.transform);
            RePosition(newWallObject.transform);

            wallObjects.Add(newWallObject);

            // 親の設定
            newWallObject.transform.parent = transform;
        }
    }

    // n角形のvalue番目のポジションを返す
    private Vector3 CalclatePosition(uint n, uint index)
    {
        const float TWO_PI = 2 * Mathf.PI;
        float x = Mathf.Sin((TWO_PI / n) * index);
        float y = 0;
        float z = Mathf.Cos((TWO_PI / n) * index);

        Vector3 returnValue = new Vector3(x, y, z);
        return returnValue;
    }

    // 中心を向くように姿勢を制御する
    private void CalclateRotation(Transform argTransform)
    {
        // 中心へのベクトルを生成
        Vector3 toCenterVector = -argTransform.position.normalized;

        Vector3 rotateAxis = Vector3.up;
        float radian = Mathf.Atan2(toCenterVector.x, toCenterVector.z);
        float degree = radian * 180 / Mathf.PI;
        argTransform.rotation = Quaternion.AngleAxis(degree, rotateAxis);
    }

    // 引数のtransformの座標をクラスに設定されているn角形ごとにあった値分回転させる
    private void RotatePosition(Transform argTransform)
    {
        float angle = Mathf.PI / wallValue;

        // ラジアンから角度に修正
        angle = angle * 180 / Mathf.PI;

        Vector3 axis = new Vector3(0, 1, 0);
        Quaternion rotateQuaternion = Quaternion.AngleAxis(angle, axis);

        Vector3 pos = argTransform.position;
        argTransform.position = rotateQuaternion * pos;
    }

    // n角形ごとの辺の長さに対応させる
    private void ReSize(Transform argTransform)
    {
        float trueWidthSize = 2 * Mathf.Sin(Mathf.PI / wallValue) * wallPrefab.transform.localScale.x;
        Vector3 beforeScale = argTransform.transform.localScale;
        argTransform.transform.localScale = new Vector3(trueWidthSize, beforeScale.y, beforeScale.z);
    }

    // 壁の長さにあった位置に修正する
    private void RePosition(Transform argTransform)
    {
        float halfSize = wallPrefab.transform.localScale.x;
        argTransform.position *= halfSize;
    }
}
