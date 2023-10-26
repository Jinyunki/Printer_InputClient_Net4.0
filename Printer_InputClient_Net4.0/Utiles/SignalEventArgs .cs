using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer_InputClient_Net4._0
{
    public class SignalEventArgs : EventArgs
    {
        public bool Signal { get; }

        public SignalEventArgs(bool signal)
        {
            Signal = signal;
        }
    }
}
