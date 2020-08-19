﻿using System.Collections;
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
        protected GameObject[] wavePrefabs = default;
        [SerializeField]
        protected float waveDuration = 5f;

        protected IEnumerator Start()
        {
            for(var i = 0; i < wavePrefabs.Length; i++)
            {
                Instantiate(wavePrefabs[i], parent : transform,
                    instantiateInWorldSpace : true);
                yield return new WaitForSeconds(waveDuration);
            }
        }
    }
}
