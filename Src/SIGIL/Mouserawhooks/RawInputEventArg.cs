using System;

namespace RawInput_dll
{
    public class RawInputEventArg : EventArgs
    {
        public RawInputEventArg(ButtonPressEvent arg)
        {
            ButtonPressEvent = arg;
        }
        
        public ButtonPressEvent ButtonPressEvent { get; private set; }
    }
}
