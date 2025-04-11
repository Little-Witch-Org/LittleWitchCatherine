using System;

public class GoldEvents_Test
{
    public event Action<int> OnGoldGained;
    public void GoldGained(int gold) //separated method with body which invokes event needed for invoking this event outside of this class
    {
       OnGoldGained?.Invoke(gold);
    }

    public event Action<int> OnGoldChange;
    public void GoldChange(int gold) 
    {
        OnGoldChange?.Invoke(gold);
    }
}