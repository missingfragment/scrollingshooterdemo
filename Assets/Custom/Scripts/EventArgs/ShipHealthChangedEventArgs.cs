using System;

namespace SpaceShooterDemo
{
    public class ShipHealthChangedEventArgs : EventArgs
    {
        public int OldHealthValue { get; set; }
        public int NewHealthValue { get; set; }
    }

}
