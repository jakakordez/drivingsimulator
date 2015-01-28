using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Slovenia_simulator
{
    class Controller
    {
        public bool Accelerate, Brake, Left, Right, CruiseControl, CControlInc, CControlDec;
        public bool Forward, Reverse;
        public bool ExteriorView, CabinView, RearView;

        public Controller() { }
        public Controller(KeyboardDevice device)
        {
            Accelerate = device[Key.W];
            Brake = device[Key.S];
            Left = device[Key.A];
            Right = device[Key.D];

            CruiseControl = device[Key.C];
            CControlInc = device[Key.KeypadPlus];
            CControlDec = device[Key.KeypadMinus];

            Forward = device[Key.F];
            Reverse = device[Key.R];

            CabinView = device[Key.Number1];
            ExteriorView = device[Key.Number2];
            
            RearView = device[Key.Number3];
        }
    }
}
