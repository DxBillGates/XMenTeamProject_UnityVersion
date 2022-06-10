using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movePower;

    private Vector3 velocity;
    private Vector3 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3();

        Move();
        RotateDirection();
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                // ヒットした障害物のヒットした法線方向に押し出したいからその法線を取得
                Vector3 hitNormal = other.transform.forward;

                // 座標を位置フレーム前に戻す
                const float PUSH_VALUE = 1.5f;
                transform.position -= velocity * PUSH_VALUE;

                // 壁ずりベクトルを計算
                Vector3 moveVector = velocity - Vector3.Dot(velocity, hitNormal) * hitNormal;
                transform.position += moveVector;
                break;
            case "Ball":
                break;
            case "Enemy":
                break;
        }

    }

    // 移動処理
    private void Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isInput;

        velocity = movePower * moveVector.normalized;

        transform.position += velocity;

        isInput = inputDirection.magnitude > 0.5f;
        if (isInput) currentDirection = inputDirection.normalized;
    }

    // プレイヤーを移動ベクトル方向に向かせる
    private void RotateDirection()
    {
        if (currentDirection.magnitude == 0) return;

        transform.rotation = Quaternion.LookRotation(currentDirection);
    }
}
