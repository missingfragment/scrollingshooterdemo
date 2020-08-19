using System;

namespace SpaceShooterDemo
{
    public class ScoreChangedEventArgs : EventArgs
    {
        public int OldValue;
        public int NewValue;
    }
}
