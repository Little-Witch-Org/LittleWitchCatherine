using UnityEngine;

public class EntryTriggerLocationDataComponent : MonoBehaviour
{

    //todo are we sure, to store location names to load, in PlayerPrefs(saves between sessions)?
    [SerializeField] private CatherineHouseLocationNames LocationNameToTransfer;

    public void SetScenePrefsData()
    {
        PlayerPrefs.SetString("LocationName", LocationNameToTransfer.ToString());
    }
}
