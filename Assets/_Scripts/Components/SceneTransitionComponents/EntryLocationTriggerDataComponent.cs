using UnityEngine;
using UnityEngine.Serialization;

public class EntryLocationTriggerDataComponent : MonoBehaviour
{

    //todo are we sure, to store location names to load, in PlayerPrefs(saves between sessions)?
    [FormerlySerializedAs("LocationNameToTransfer")] [SerializeField] private CatherineHouseNovelLocationNames novelLocationNameToTransfer;

    public void SetScenePrefsData()
    {
        PlayerPrefs.SetString("LocationName", novelLocationNameToTransfer.ToString());
    }
}
