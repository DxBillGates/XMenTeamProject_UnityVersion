using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionalLineDrawer : MonoBehaviour
{
    // 予測線の長さ
    [SerializeField,Range(0,100)] private float maxLength;
    // 予測線を表現しているオブジェクト同士の間隔
    [SerializeField,Range(0,100)] private float objectDistance;
    [SerializeField] private GameObject lineObjectPrefab;

    // 予測線を表現しているオブジェクト配列
    private List<GameObject> lineObjects;
    // 予測線を開始する基点
    public Vector3 drawOriginPosition { private get; set; }
    // 予測線の方向ベクトル
    public Vector3 drawDirection { private get; set; }

    public bool isDraw { get; set; }
    private float setObjectDistance;

    private Vector3 hitPoint;
    private Vector3 hitNormal;
    private float hitDistance;

    private void Awake()
    {
        // 予測線表現オブジェクトを追加
        const int OBJECTS_VALUE = 10;
        lineObjects = new List<GameObject>();
        CreateLineObjects(OBJECTS_VALUE);

        drawOriginPosition = Vector3.zero;
        drawDirection = new Vector3(0,0,1);
        setObjectDistance = 0;
    }

    // 毎フレームレイキャストはエグそうだからここでやる
    private void FixedUpdate()
    {
        hitNormal = Vector3.zero;
        hitDistance = 0;

        if(Physics.Raycast(drawOriginPosition,drawDirection,out RaycastHit raycastHit,maxLength))
        {
            if (!raycastHit.collider.gameObject.CompareTag("Wall")) return;

            hitPoint = raycastHit.point;
            hitNormal = raycastHit.collider.gameObject.transform.forward;
            hitDistance = raycastHit.distance;
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
        for(int i = 0;i < createValue; ++i)
        {
            lineObjects.Add(Instantiate(lineObjectPrefab));
            lineObjects[i].transform.parent = transform;
        }
    }

    private void UpdateLineObjects()
    {
        bool isSetStartReflectIndex = false;
        int startReflectIndex = 0;
        for(int i = 0;i < lineObjects.Count;++i)
        {
            Vector3 setPosition = drawOriginPosition + i * setObjectDistance * drawDirection;
            Vector3 direction = i * setObjectDistance * drawDirection;

            if (direction.magnitude > hitDistance && hitNormal.magnitude > 0)
            {
                if(isSetStartReflectIndex == false)
                {
                    startReflectIndex = i;
                    isSetStartReflectIndex = true;
                }

                Vector3 reflectVector = direction - 2.0f * Vector3.Dot(direction, hitNormal) * hitNormal;
                setPosition = hitPoint + (i - startReflectIndex) * setObjectDistance * reflectVector.normalized;
            }

            lineObjects[i].transform.position = setPosition;
        }
    }
}
