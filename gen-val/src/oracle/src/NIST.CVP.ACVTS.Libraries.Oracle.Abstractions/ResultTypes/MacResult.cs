﻿using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class MacResult
    {
        public BitString Message { get; set; }
        public BitString Key { get; set; }
        public BitString Iv { get; set; }
        public BitString Tag { get; set; }
        public bool TestPassed { get; set; } = true;
    }
}
