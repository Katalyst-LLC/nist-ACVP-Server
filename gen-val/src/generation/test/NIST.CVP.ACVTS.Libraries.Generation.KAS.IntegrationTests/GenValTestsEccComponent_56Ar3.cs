﻿using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEccComponent_56Ar3 : GenValTestsSingleRunnerBase
    {
        private readonly Random800_90 _random = new Random800_90();

        public override string Algorithm => "KAS-ECC";

        public override string Mode => "CDH-Component";

        public override string Revision => "Sp800-56Ar3";

        public override AlgoMode AlgoMode => AlgoMode.KAS_EccComponent_Sp800_56Ar3;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = ParameterValidator.ValidFunctions,
                Curve = new[] { "P-192" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = ParameterValidator.ValidFunctions,
                Curve = new[] { "P-192", "K-163", "B-163" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a z, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = _random.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
            }
        }
    }
}
