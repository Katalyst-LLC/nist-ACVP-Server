﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaSignatureBitFlipCaseGrain : ObservableOracleGrainBase<EddsaSignatureResult>,
        IOracleObserverEddsaSignatureBitFlipCaseGrain
    {
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        private EddsaSignatureParameters _param;
        private const int BITS_IN_BYTE = 8;

        public OracleObserverEddsaSignatureBitFlipCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEdwardsCurveFactory curveFactory,
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }

        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);
            var edDsa = _dsaFactory.GetInstance(null);

            var message = _param.Message.GetDeepCopy();
            if (_param.Bit != -1)
            {
                message.Bits.Set(_param.Bit, !message.Bits.Get(_param.Bit));      // flip bit
            }

            var result = edDsa.Sign(domainParams, _param.Key, message, _param.Context, _param.PreHash);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EddsaSignatureResult
            {
                Message = message,
                Context = _param.Context,
                ContextLength = _param.ContextLength/BITS_IN_BYTE,
                Signature = result.Signature
            });
        }
    }
}
