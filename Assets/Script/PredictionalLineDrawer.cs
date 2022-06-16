using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RaycastResult
{
    // ヒットした座標
    public Vector3 point { get; set; }
    // ヒットした法線
    public Vector3 normal { get; set; }
    // レイキャストするときに使用したベクトルとヒットした法線から取得した反射ベクトル
    public Vector3 reflectVector { get; set; }
    // ヒットにかかった距離
    public float distance { get; set; }

    public bool isHit { get; set; }
}


public class PredictionalLineDrawer : MonoBehaviour
{
    // 予測線の最大長さ
    [SerializeField, Range(0, 500)] private float maxLength;
    // 予測線を表現しているオブジェクト同士の間隔
    [SerializeField, Range(0, 100)] private float objectDistance;
    [SerializeField] private GameObject lineObjectPrefab;
    // 予測線を表現するオブジェクトの総数
    [SerializeField] private int lineObjectCount;

    // 予測線を表現しているオブジェクト配列
    private List<GameObject> lineObjects;
    // 予測線を開始する基点
    public Vector3 drawOriginPosition { private get; set; }
    // 予測線の方向ベクトル
    public Vector3 drawDirection { private get; set; }

    public bool isDraw { get; set; }
    private float setObjectDistance;

    [SerializeField, Range(1, 10)] int raycastCount;
    int beforeRaycastCount;
    private List<RaycastResult> raycastResults;

    private void Awake()
    {
        // 予測線表現オブジェクトを追加
        lineObjects = new List<GameObject>();
        CreateLineObjects(lineObjectCount);

        drawOriginPosition = Vector3.zero;
        drawDirection = new Vector3(0, 0, 1);
        setObjectDistance = 0;

        beforeRaycastCount = raycastCount;
        raycastResults = new List<RaycastResult>();
        for (int i = 0; i <= raycastCount; ++i) raycastResults.Add(new RaycastResult());
    }

    // 毎フレームレイキャストはエグそうだからここでやる
    private void FixedUpdate()
    {
        FixRaycastCount();
        beforeRaycastCount = raycastCount;

        if (isDraw == false) return;

        // レイキャストの回数とレイキャストの初期化
        Vector3 rayOriginPosition = drawOriginPosition;
        Vector3 rayDirection = drawDirection;
        float rayDistance = 0;
        for (int i = 0; i < raycastCount; ++i)
        {
            // レイの最大距離とレイキャストしていった際の最大の長さが既定値を超えているならレイキャストは終了
            if (maxLength - rayDistance < 0)
            {
                InitializeRaycastResults(i);
                break;
            }
            if (Physics.Raycast(rayOriginPosition, rayDirection, out RaycastHit raycastHit, maxLength - rayDistance))
            {
                //// 壁オブジェクトじゃないならレイキャストの結果は格納しない
                //if (!raycastHit.collider.gameObject.CompareTag("Wall"))
                //{
                //    InitializeRaycastResults(i);
                //    break;
                //}

                // 次のループで使うレイキャストのベクトルを
                // レイキャストした際のベクトルと壁の法線で反射ベクトルを取得
                Vector3 reflectVector = rayDirection - 2.0f * Vector3.Dot(rayDirection, raycastHit.normal) * raycastHit.normal;

                // 次ループのレイキャスト用に値をローカル変数に上書き
                rayOriginPosition = raycastHit.point;
                rayDirection = reflectVector.normalized;
                rayDistance = raycastHit.distance;

                // 各レイキャストの結果を配列に格納
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
        // レイキャストの反射回数結果を扱うためのインデックス
        int reflectCount = 0;
        // 反射を反映し始める配列の要素番号
        int reflectStartIndex = 0;

        Vector3 setPosition = drawOriginPosition;
        Vector3 useDirection = drawDirection;
        // ラインオブジェクト分ループして更新
        for (int i = 0; i < lineObjects.Count; ++i)
        {
            if (isDraw == true) lineObjects[i].SetActive(true);
            else lineObjects[i].SetActive(false);

            // 反射回数が反射の結果を格納している配列の要素数より大きくならないように
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

            // ベクトルの長さがレイキャスト結果の距離より長いなら反射回数をインクリメント
            // setPosition useDirectionを更新
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

    // エディターからレイキャストの回数を増やした際に配列オーバーになってしまうため
    // それを回避する関数
    private void FixRaycastCount()
    {
        if (beforeRaycastCount == raycastCount) return;

        raycastResults.Clear();
        Debug.Log(raycastResults.Count);
        for (int i = 0; i <= raycastCount; ++i) raycastResults.Add(new RaycastResult());
    }
}
