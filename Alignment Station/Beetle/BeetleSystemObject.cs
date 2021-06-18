namespace Beetle
{
    class BeetleSystemObject
    {
        public Parameters parameters = new Parameters();
        public BeetleMathModel mathModel;
        public BeetleControl beetleControl;
        public OnebyNAlignment ba;
        public OnebyNCuring bc;
        public PiezoControl piezoControl;
        private BeetleDetection beetleDetection;
        public WOAAlignment woa;

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
            ba = new OnebyNAlignment(parameters, beetleControl, piezoControl);
            bc = new OnebyNCuring(parameters, beetleControl, piezoControl);
        }

        public void WOAInit() => woa = new WOAAlignment(parameters, beetleControl, piezoControl);

    }
}
