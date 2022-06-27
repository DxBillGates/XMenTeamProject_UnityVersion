using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 無敵フラグのコンポーネント
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

    // 無敵状態かどうかのチェック用
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
