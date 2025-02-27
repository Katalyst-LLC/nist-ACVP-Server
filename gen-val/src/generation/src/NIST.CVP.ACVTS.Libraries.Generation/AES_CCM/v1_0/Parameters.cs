﻿using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public int[] KeyLen { get; set; }
        public MathDomain PayloadLen { get; set; }
        public MathDomain IvLen { get; set; }
        public MathDomain AadLen { get; set; }
        public int[] TagLen { get; set; }
        public bool SupportsAad2Pow16 { get; set; }
    }
}
