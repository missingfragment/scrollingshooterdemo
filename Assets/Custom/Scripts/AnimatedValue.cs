using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for providing a generic coroutine
/// that lerps a value from point A to point B
/// using a LerpValue delegate,
/// then runs an ApplyValue delegate on the result.
/// </summary>
/// <typeparam name="T">The type of the value to animate.</typeparam>
public class AnimatedValue<T>
{
    /// <summary>
    /// A function that uses the updated value.
    /// </summary>
    /// <param name="value">
    /// The function must take a value of type T.
    /// </param>
    public delegate void ApplyValue(T value);
    /// <summary>
    /// A function that Lerps the value between startValue and endValue
    /// by progress percent.
    /// </summary>
    /// <param name="startValue">The starting T value.</param>
    /// <param name="endValue">The end T value.</param>
    /// <param name="progress">A float from 0 to 1.</param>
    /// <returns>A T value.</returns>
    public delegate T LerpValue(T startValue, T endValue, float progress);

    protected float _speed = 2f;

    public AnimatedValue() { }
    public AnimatedValue(float speed)
    {
        _speed = speed;
    }

    /// <summary>
    /// Returns an IEnumerator so that a MonoBehavior-derived class
    /// can run this function as a coroutine.  Animates the value
    /// between startValue and endValue using the lerpValue and applyValue
    /// delegates.
    /// </summary>
    /// <param name="startValue">
    /// The value to start at.
    /// </param>
    /// <param name="endValue">
    /// The value to end at.
    /// </param>
    /// <param name="lerpValue">
    /// A delegate that lerps the value.
    /// </param>
    /// <param name="applyValue">
    /// A delegate that does something with the value as it animates.
    /// </param>
    /// <returns>An IEnumerator for use in coroutines.</returns>
    public IEnumerator Animate(T startValue, T endValue,
        LerpValue lerpValue, ApplyValue applyValue)
    {
        float progress = 0f;

        T value = startValue;

        while (progress < 1f)
        {
            value = lerpValue(startValue, endValue, progress);
            applyValue(value);
            progress += Time.deltaTime * _speed;
            yield return null;
        }

        applyValue(endValue);
    }
}
