using UnityEngine;
using System;

public class VisibilityChecker : MonoBehaviour
{
    public event Action<bool> VisibilityChanged;

    private void OnBecameInvisible()
    {
        VisibilityChanged?.Invoke(false);
    }

    private void OnBecameVisible()
    {
        VisibilityChanged?.Invoke(true);
    }
}
