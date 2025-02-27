﻿using NIST.CVP.ACVTS.Libraries.Common;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.IntegrationTests
{
    public class GenValTestsGmac : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_GMAC_v1_0;
        public override string Algorithm => "ACVP-AES-GMAC";
    }
}
