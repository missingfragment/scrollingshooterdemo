using System.Collections;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class SwoopAiControl : AiControl
    {
        [SerializeField]
        protected bool enterFromLeft;
        protected override IEnumerator RunAi()
        {
            mover.RotateWithMovement = true;

            float xDirection = enterFromLeft ? 1f : -1f;

            // Enter the screen at an angle
            mover.Inputs = new Vector2(xDirection / 2, -1f);
            yield return new WaitForSeconds(entryTime);

            // Begin firing
            mover.Inputs = new Vector2(xDirection, -0.1f);
            firing = true;
            yield return new WaitForSeconds(activeTime);

            // Change course
            mover.Inputs = enterFromLeft ? Vector2.right : Vector2.left;
            yield return null;
        }
    }
}
