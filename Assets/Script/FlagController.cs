using System.Collections;

public enum FlagActiveType
{
    PRE,
    ACTIVE,
    END
}


[System.Serializable]
public class FlagController
{
    public float maxActiveTime;
    public float activeTime;

    private bool beforeFlag;
    public bool flag;

    private bool beforeIsEnd;
    public bool isEnd;
    public FlagActiveType activeType;

    public void Initialize()
    {
        activeTime = 0;
        beforeFlag = false;
        flag = false;
        beforeIsEnd = false;
        isEnd = false;
    }

    public void Update(float deltaTime)
    {
        beforeFlag = flag;
        beforeIsEnd = isEnd;

        if (beforeFlag == false || flag == false) return;

        // ƒtƒ‰ƒO‚ÌÅ‘åŠÔ‚ªİ’è‚³‚ê‚Ä‚¢‚È‚¢ê‡‚Í‚·‚®I—¹
        if (maxActiveTime <= 0)
        {
            End();
            return;
        }

        if (activeTime >= maxActiveTime)
        {
            End();
            return;
        }

        activeTime += deltaTime;
    }

    public bool IsStartTrigger()
    {
        return beforeFlag == false && flag == true;
    }

    public bool IsEndTrigger()
    {
        return beforeIsEnd == false && isEnd == true;
    }

    private void End()
    {
        Initialize();
        isEnd = true;
    }
}

