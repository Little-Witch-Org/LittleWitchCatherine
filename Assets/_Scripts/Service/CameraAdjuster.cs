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
        //Получение размеров спрайта
        float spriteWidth = templateSprite.bounds.size.x;
        float spriteHeight = templateSprite.bounds.size.y;
        //Рассчитываем соотношение сторон спрайта и экрана
        float screenAspect = (float)Screen.width / Screen.height;
        float spriteAspect = spriteWidth / spriteHeight;
        //Настраиваем ортографический размер камеры
        if (screenAspect >= spriteAspect)
        {
            Camera.orthographicSize = spriteHeight / 2;
        }
        else
        {
            float differenceInSize = spriteAspect / screenAspect;
            Camera.orthographicSize = spriteHeight / 2 * differenceInSize;
        }
        //Центрируем камеру на изображении
        Camera.transform.position = new Vector3(templateSprite.transform.position.x, 
                    templateSprite.transform.position.y, Camera.transform.position.z);
    }
}
