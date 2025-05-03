using System.Collections;
using UnityEngine;

namespace _Scripts.Components.Misc
{
    /// <summary>
    /// Used for object fade on map //todo to remake?
    /// </summary>
    public class TransparencyChangerComponent : MonoBehaviour, IFadeable
    {
        [SerializeField] private float minValue = 0.5f;
        [SerializeField] private float stepValue = 0.05f;
        [SerializeField] private float stepTime = 0.05f;
        [SerializeField] private SpriteRenderer[] spriteRendArray;

        private Coroutine _myCoroutine;
        private float _currentValue = 1f;

        private const float MaxValue = 1f;

        public void FadeIn()
        {
            //остановка других корутин, чтобы избежать багов с мерцанием спрайта, багов при одновременном включении двух корутин
            StopCoroutine();
            _myCoroutine = StartCoroutine(FadeOutProcces());
        }
        public void FadeOut()
        {
            if (!gameObject.activeInHierarchy) return; //handle fast change scene situation (unFade try to start but scene is new now)
            StopCoroutine();
            _myCoroutine = StartCoroutine(UnFadeProcces());
        }
        private IEnumerator UnFadeProcces()
        {
            for (; _currentValue < MaxValue;)
            {
                _currentValue += stepValue;
                for (int j = 0; j < spriteRendArray.Length; j++)
                {
                    spriteRendArray[j].color = new Color(1f, 1f, 1f, _currentValue);
                }
                yield return new WaitForSeconds(stepTime);
            }
        }
        private IEnumerator FadeOutProcces()
        {
            //”величение прозрачности, текущее«начение не хардкодитс€ ни к min, ни к max
            for (; _currentValue > minValue; _currentValue -= stepValue)
            {
                for (int j = 0; j < spriteRendArray.Length; j++)
                {
                    spriteRendArray[j].color = new Color(1f, 1f, 1f, _currentValue);
                }
                yield return new WaitForSeconds(stepTime);
            }
        }
        private void StopCoroutine()
        {
            if (_myCoroutine == null)
                return;
            StopCoroutine(_myCoroutine);
            _myCoroutine = null;
        }
    }
}
