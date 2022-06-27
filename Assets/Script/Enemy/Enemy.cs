using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    // �m�b�N�o�b�N�̑��x
    [SerializeField] protected float knock_back_speed = 0.5f;
    // �ړ����x
    [SerializeField] protected float moveSpeed = 2.0f;

    // hp
    [SerializeField] protected float hp = 10;

    // �G���m�ŋ߂Â��Ȃ�����
    [SerializeField] protected float dontHitDistance = 3.0f;

    [SerializeField] protected List<AudioClip> SE;

    //player�����
    protected GameObject targetObject;

    // ���t���[���̈ړ���
    protected Vector3 movedVector;

    //SE��GetComponent�p
    protected SEPlayManager sePlayManagerComponent;

    // �A�j���[�V�����̑��x�ύX�p
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�����ɏ�����
        //PlayerFollow();
    }

    // �ړ��p�֐�
    protected virtual void Move()
    {

    }

    // ���S���֐�
    protected virtual void Defaat()
    {
    }

    protected void PlayerFollow()
    {
        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();
        moveVector.y = 0;

        // �����x�N�g�����v�Z
        Vector3 leaveV = new Vector3(0, 0, 0);

        // �G���m�ł̔�������x�N�g�����v�Z
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
            // �w�肵���͈͂��G���߂����ɗ����
            if (distance <= dontHitDistance)
            {
                Vector3 calcLeaveV = transform.position - enemys[i].transform.position;

                calcLeaveV.y = 0;
                leaveV += calcLeaveV.normalized;
            }
        }
        leaveV.Normalize();

        // �ǂƂ̔���p�ɒl��ۑ�
        movedVector = (moveVector + leaveV) * moveSpeed;
        movedVector.y = 0;

        // �ړ���̃|�W�V�������������
        transform.position += movedVector * GameTimeManager.GetInstance().GetTime();

        // �ړ��� �����ύX
        // �v���C���[�̕����x�N�g�����擾���A������g����]������
        Vector3 playerV = targetObject.transform.position - transform.position;
        playerV.y = 0;

        if (playerV != Vector3.zero) transform.rotation = Quaternion.LookRotation(playerV);
    }


    /// <summary>
    /// �����ɓ��������Ƃ��̃m�b�N�o�b�N����
    /// </summary>
    /// <param name="">���������G�̈ʒu</param>
    public void KnockBack(Collider collision)
    {
        Vector3 hitPos = collision.gameObject.transform.position;
        // �m�b�N�o�b�N����ʒu�����߂�
        Vector3 moveVector = -1 * (hitPos - transform.position);
        // ���K��������
        moveVector = knock_back_speed * moveVector.normalized;

        transform.position += moveVector;

        movedVector += moveVector;

        Damage(collision.gameObject.GetComponent<Ball>().GetSpeed());
    }

    protected void Damage(float damage)
    {
        hp -= damage;

        sePlayManagerComponent.SESeting(SE[0]);

        if (hp < 0)
        {
            EnemyManager.DecrementAliveCount();
            Destroy(transform.root.gameObject);
            const float ADD_GAUGE_VALUE = 25;

            // �|�����Ƃ��Ƀq�b�g�X�g�b�v
            HitStopManager.GetInstance().HitStop();

            // �X�L���𔭓����Ă��Ȃ��Ƃ��ɓG��|�����Ȃ�X�L���Q�[�W��������
            if (UltimateSkillManager.GetInstance().IsUse() == false)
            {
                UltimateSkillManager.GetInstance().AddGauge(ADD_GAUGE_VALUE);
            }
        }
    }

    public void WallCollsion(Transform wallTransform)
    {
        // �q�b�g������Q���̃q�b�g�����@�������ɉ����o���������炻�̖@�����擾
        Vector3 hitNormal = wallTransform.forward;

        // ���W���ʒu�t���[���O�ɖ߂�
        const float PUSH_VALUE = 3.0f;
        transform.position -= movedVector * PUSH_VALUE;

        // �ǂ���x�N�g�����v�Z
        Vector3 moveVector = movedVector - Vector3.Dot(movedVector, hitNormal) * hitNormal;
        transform.position += moveVector;
    }
}
