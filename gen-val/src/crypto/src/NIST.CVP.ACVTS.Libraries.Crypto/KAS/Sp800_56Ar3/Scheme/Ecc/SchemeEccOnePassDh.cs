﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal class SchemeEccOnePassDh : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccOnePassDh(
            SchemeParameters schemeParameters,
            ISecretKeyingMaterial thisPartyKeyingMaterial,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters keyConfirmationParameter,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman)
            : base(
                schemeParameters,
                thisPartyKeyingMaterial,
                fixedInfoFactory,
                fixedInfoParameter,
                kdfFactory,
                kdfParameter,
                keyConfirmationFactory,
                keyConfirmationParameter)
        {
            _diffieHellman = diffieHellman;
        }

        protected override BitString ComputeSharedSecret(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return _diffieHellman.GenerateSharedSecretZ(
                    (EccDomainParameters)ThisPartyKeyingMaterial.DomainParameters,
                    (EccKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                    (EccKeyPair)otherPartyKeyingMaterial.StaticKeyPair
                ).SharedSecretZ;
            }

            return _diffieHellman.GenerateSharedSecretZ(
                (EccDomainParameters)ThisPartyKeyingMaterial.DomainParameters,
                (EccKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;
        }
    }
}
