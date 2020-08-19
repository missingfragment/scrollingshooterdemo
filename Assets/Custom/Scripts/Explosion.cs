using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace SpaceShooterDemo
{
    /// <summary>
    /// An explosion effect that uses pooling and adjusts its Light2D.
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        private const int LIGHT_INTENSITY = 4;

        [SerializeField]
        private Light2D explosionLight = default;

        public void OnEnable()
        {
            explosionLight.intensity = LIGHT_INTENSITY;
            IEnumerator DestroyAfterSetTime()
            {
                //yield return new WaitForSeconds(1);
                while (explosionLight.intensity > 0)
                {
                    explosionLight.intensity -= 4f * Time.deltaTime;
                    yield return null;
                }
                Remove();
            }

            StartCoroutine(DestroyAfterSetTime());
            
        }

        public void Remove()
        {
            ExplosionPool.Instance.ReturnToPool(this);
        }
    }
}
