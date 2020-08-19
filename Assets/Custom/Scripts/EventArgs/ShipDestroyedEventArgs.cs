using System;

namespace SpaceShooterDemo
{
    public class ShipDestroyedEventArgs : EventArgs
    {
        public Team Alignment { get; set; }
        public int ScoreValue { get; set; }
    }

}
