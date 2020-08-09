using System;
using System.Collections.Generic;
using SpaceShooterDemo;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    // fields
    [SerializeField]
    private T prefab = default;

    private readonly Queue<T> objects = new Queue<T>();

    // static properties
    public static ObjectPool<T> Instance { get; private set; }

    // methods

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            AddObjects(1);
        }
        return objects.Dequeue();
    }

    public void ReturnToPool(T usedObject)
    {
        usedObject.gameObject.SetActive(false);
        objects.Enqueue(usedObject);
    }

    private void AddObjects(int amount)
    {
        for(var i = 0; i < amount; i++)
        {
            T newObject = (T) Instantiate(
                prefab,
                parent : transform,
                instantiateInWorldSpace : true
                );

            newObject.gameObject.SetActive(false);
            objects.Enqueue(newObject);
        }
        
    }
}
