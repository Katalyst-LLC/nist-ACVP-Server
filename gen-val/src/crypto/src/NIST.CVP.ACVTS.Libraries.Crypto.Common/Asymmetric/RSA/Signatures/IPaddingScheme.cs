﻿using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface IPaddingScheme
    {
        (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen);
        PaddingResult Pad(int nlen, BitString message);
        BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey);
        VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey);
    }
}
