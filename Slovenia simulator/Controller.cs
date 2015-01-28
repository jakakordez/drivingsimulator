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
        public bool Accelerate, Brake, Left, Right;
        public bool Forward, Reverse;
        public bool ExteriorView, CabinView, RearView;

        public Controller() { }
        public Controller(KeyboardDevice device)
        {
            Accelerate = device[Key.W];
            Brake = device[Key.S];
            Left = device[Key.A];
            Right = device[Key.D];

            Forward = device[Key.F];
            Reverse = device[Key.R];

            CabinView = device[Key.Number1];
            ExteriorView = device[Key.Number2];
            
            RearView = device[Key.Number3];
        }
    }
}
