using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class TapGesture : GestureBase
    {

        public TapGesture(int x, int y)
        {
            TapPosition = new Point(x, y);       
        }

        public static TapGesture TapOnPosition(int x, int y) 
        {
            return new TapGesture(x,y);
        } 

        public Point TapPosition { get; set; }

        public override void Perform(EmulatorDisplayInputController emulatorDisplayInputController)
        {
            var p = emulatorDisplayInputController.TranslatePhonePositionToHostPosition(TapPosition);
            emulatorDisplayInputController.PerformMouseDownUp(p);
        }
    }
}
