using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���G�t���O�̃R���|�[�l���g
public class InvincibleFlag : MonoBehaviour
{
    [SerializeField] private FlagController flagController;
    [SerializeField] private FlashingMaterial flashingMaterial;

    // Start is called before the first frame update
    void Start()
    {
        flashingMaterial.Initialize();
        flashingMaterial.SetMaterial(GetComponentInChildren<Renderer>().material);
        flashingMaterial.SetExternalFlagController(flagController);

        flagController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (flagController.flag == false) return;

        flagController.Update(Time.deltaTime);
        flashingMaterial.Update(Time.deltaTime);
    }

    // ���G��Ԃ��ǂ����̃`�F�b�N�p
    public bool IsInvincible()
    {
        return flagController.flag;
    }

    public void Invincible()
    {
        flagController.Initialize();
        flashingMaterial.Initialize();

        flagController.flag = true;
    }

}
