using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ボールを投げる際の強さ
    [SerializeField] private float throwPower;
    // 減衰力
    [SerializeField] private float attenuationPower;
    Vector3 velocity;
    bool isThrow;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3();
        isThrow = false;

        Throw(new Vector3(0.5f, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrow) Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitNormal = collision.contacts[0].normal;
        Reflection(hitNormal);
    }

    // ゲームオブジェクトをその向きに対する速度を与える
    public void Throw(Vector3 direction)
    {
        velocity = direction * throwPower;

        isThrow = true;
    }

    // ボールの移動処理
    private void Move()
    {
        velocity -= velocity.normalized * attenuationPower;

        transform.position += velocity;
    }

    private void Reflection(Vector3 normal)
    {
        Vector3 reflectVector = velocity - 2.0f * Vector3.Dot(velocity,normal) * normal;
        velocity = reflectVector;
    }
}
