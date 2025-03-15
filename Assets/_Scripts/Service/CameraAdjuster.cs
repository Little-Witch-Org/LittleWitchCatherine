using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private SpriteRenderer templateSprite;
    private Camera Camera;

    private void Start()
    {
        Camera = Camera.main;
        AdjustCameraToBackground();
    }
    private void AdjustCameraToBackground()
    {
        //��������� �������� �������
        float spriteWidth = templateSprite.bounds.size.x;
        float spriteHeight = templateSprite.bounds.size.y;
        //������������ ����������� ������ ������� � ������
        float screenAspect = (float)Screen.width / Screen.height;
        float spriteAspect = spriteWidth / spriteHeight;
        //����������� ��������������� ������ ������
        if (screenAspect >= spriteAspect)
        {
            Camera.orthographicSize = spriteHeight / 2;
        }
        else
        {
            float differenceInSize = spriteAspect / screenAspect;
            Camera.orthographicSize = spriteHeight / 2 * differenceInSize;
        }
        //���������� ������ �� �����������
        Camera.transform.position = new Vector3(templateSprite.transform.position.x, 
                    templateSprite.transform.position.y, Camera.transform.position.z);
    }
}
