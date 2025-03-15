using System.Collections.Generic;
using UnityEngine;

public class CalcPivotDiff : MonoBehaviour
{
    public Sprite[] spriteImageArray;
    public GameObject[] spriteObject;

    [ContextMenu("Calc")]
    public void OnCalculate()
    {
        float offsetX = 0, offsetY = 0;
        for (int i = 0; i < spriteImageArray.Length; i++)
        {
            if (i == spriteImageArray.Length - 1)
                return;
            offsetX += (spriteImageArray[i].rect.width - spriteImageArray[i].pivot.x + spriteImageArray[i + 1].pivot.x) / 100;
            offsetY += (spriteImageArray[i + 1].pivot.y - spriteImageArray[i].pivot.y) / 100;
            Debug.Log(new Vector3(offsetX, offsetY, 0));
            spriteObject[i + 1].transform.localPosition = ((new Vector3(offsetX, offsetY, 0)));
        }
    }

}
