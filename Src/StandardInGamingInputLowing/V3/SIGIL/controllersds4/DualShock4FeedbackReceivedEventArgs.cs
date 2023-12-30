using System;
using System.Diagnostics.CodeAnalysis;

namespace controllersds4
{

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DualShock4FeedbackReceivedEventArgs : EventArgs
    {
        public DualShock4FeedbackReceivedEventArgs(byte largeMotor, byte smallMotor, LightbarColor color)
        {
            LargeMotor = largeMotor;
            SmallMotor = smallMotor;
            LightbarColor = color;
        }

        public byte LargeMotor { get; }

        public byte SmallMotor { get; }

        public LightbarColor LightbarColor { get; }
    }
}