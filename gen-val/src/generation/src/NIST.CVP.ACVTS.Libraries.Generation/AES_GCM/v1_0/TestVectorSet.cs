﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "AES";
        public string Mode { get; set; } = "GCM";
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
