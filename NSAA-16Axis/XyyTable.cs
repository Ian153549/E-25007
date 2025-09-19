using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiAutomaticAligner
{
    class XyyTable
    {
        PlcCommunication Plc;

        public XyyTable(PlcCommunication plc)
        {
            Plc = plc;
        }

        public void Move(int x, int y, int degree)
        {

        } 
    }

    class XyyTableMotorPulses
    {
        public int X, Y1, Y2;

        public XyyTableMotorPulses(int x, int y1, int y2)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
        }
    }
}
