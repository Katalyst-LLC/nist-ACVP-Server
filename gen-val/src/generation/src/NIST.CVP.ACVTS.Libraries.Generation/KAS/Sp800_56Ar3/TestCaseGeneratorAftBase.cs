﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestCaseGeneratorAftBase<TTestGroup, TTestCase, TKeyPair> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;

        protected TestCaseGeneratorAftBase(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 10;
        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample, int caseNo = -1)
        {
            try
            {
                var result = await _oracle.GetKasAftTestAsync(new KasAftParameters()
                {
                    DomainParameters = group.DomainParameters,
                    KasDpGeneration = group.DomainParameterGenerationMode,
                    KasScheme = group.Scheme,
                    KasAlgorithm = group.KasAlgorithm,
                    IutGenerationRequirements = group.KeyNonceGenRequirementsIut,
                    ServerGenerationRequirements = group.KeyNonceGenRequirementsServer,
                    KdfConfiguration = group.KdfConfiguration,
                    PartyIdServer = group.ServerId,

                    ServerEphemeralKey = group.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair ? group.ShuffleKeys.Pop() : default,
                    ServerStaticKey = group.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair ? group.ShuffleKeys.Pop() : default,
                });

                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase()
                {
                    Deferred = true,
                    TestPassed = true,
                    EphemeralKeyServer = GetKey(result.ServerSecretKeyingMaterial.EphemeralKeyPair),
                    StaticKeyServer = GetKey(result.ServerSecretKeyingMaterial.StaticKeyPair),
                    DkmNonceServer = result.ServerSecretKeyingMaterial.DkmNonce,
                    EphemeralNonceServer = result.ServerSecretKeyingMaterial.EphemeralNonce,
                    KdfParameter = result.KdfParameter
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(ex.Message);
            }
        }

        protected abstract TKeyPair GetKey(IDsaKeyPair keyPair);

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
