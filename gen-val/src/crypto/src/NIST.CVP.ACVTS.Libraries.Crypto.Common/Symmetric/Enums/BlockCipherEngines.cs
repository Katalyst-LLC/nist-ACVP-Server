﻿using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums
{
    /// <summary>
    /// Describes the block cipher engines/primitives.
    /// </summary>
    public enum BlockCipherEngines
    {
        [EnumMember(Value = "AES")]
        Aes,

        [EnumMember(Value = "TDES")]
        Tdes
    }
}
