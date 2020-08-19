using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Spawns waves of enemies at set intervals.
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField]
        protected Wave[] waves = default;
        [SerializeField]
        protected float preDelay = 3f;

        protected IEnumerator Start()
        {
            yield return new WaitForSeconds(preDelay);

            for(var i = 0; i < waves.Length; i++)
            {
                for (var ii = 0; ii < waves[i].repetitions; ii++)
                {
                    Instantiate(waves[i].prefab, parent : transform,
                        instantiateInWorldSpace : true);
                    if (waves[i].repeatDelay > 0)
                    {
                        yield return new WaitForSeconds(waves[i].repeatDelay);
                    }
                }
                yield return new WaitForSeconds(waves[i].duration);
            }
        }
    }

    [System.Serializable]
    public struct Wave
    {
        public GameObject prefab;
        public float duration;
        public int repetitions;
        public float repeatDelay;
    }
}
