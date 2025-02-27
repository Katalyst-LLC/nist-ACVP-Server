﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEccCurve CurveE { get; }

        /// <summary>
        /// How secrets are generated used by these <see cref="EccDomainParameters"/>.
        /// Generally this field doesn't matter but is needed for some group properties in gen/vals.
        /// Theoretically it should factor into KeyGen, but our tests are agnostic to this property.
        /// </summary>
        public SecretGenerationMode SecretGeneration { get; }

        public EccDomainParameters(IEccCurve e)
        {
            CurveE = e;
        }

        public EccDomainParameters(IEccCurve e, SecretGenerationMode secretMode)
        {
            CurveE = e;
            SecretGeneration = secretMode;
        }
    }
}
