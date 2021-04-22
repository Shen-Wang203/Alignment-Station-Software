using System;

namespace Beetle
{
    class BeetleSystemObject
    {
        public Parameters parameters = new Parameters();
        public BeetleMathModel mathModel;
        public BeetleControl beetleControl;
        public BeetleAlignment ba;
        public BeetleCuring bc;
        public PiezoControl piezoControl;
        private BeetleDetection beetleDetection;

        public BeetleSystemObject()
        {
            mathModel = new BeetleMathModel(parameters);
            beetleControl = new BeetleControl(parameters, mathModel);
            piezoControl = new PiezoControl(parameters);
        }

        public bool Detect()
        {
            beetleDetection = new BeetleDetection(parameters, beetleControl, piezoControl);
            return beetleDetection.AssignPorts();
        }

        public bool Connection() => beetleControl.Connection() & piezoControl.Connection();

        public void AlignCuringInit()
        {
            ba = new BeetleAlignment(parameters, beetleControl, piezoControl);
            bc = new BeetleCuring(parameters, beetleControl, piezoControl);
        }

    }
}
