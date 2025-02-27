﻿using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Helpers
{
    public static class DrbgAttributesHelper
    {
        #region DrbgAttributes
        public static List<DrbgAttributes> DrbgAttributesWithDerFunc =
            new List<DrbgAttributes>
            {
                #region AES
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES128,
                    maxSecurityStrength: 128,
                    minEntropyInputLength: 128,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128 / 2,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES192,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 192 / 2,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 256 / 2,
                    maxNonceLength: (long) 1 << 35
                ),
                #endregion AES
                #region TDES
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.TDES,
                    maxSecurityStrength: 112,
                    minEntropyInputLength: 112,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 13,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 32,
                    minNonceLength: 112 / 2,
                    maxNonceLength: (long) 1 << 35
                )
                #endregion TDES
            };

        public static List<DrbgAttributes> DrbgAttributesWithoutDerFunc =
            new List<DrbgAttributes>
            {
                #region AES
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES128,
                    maxSecurityStrength: 128,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: 256,
                    maxPersoStringLength: 256,
                    maxAdditStringLength: 256,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 0,
                    maxNonceLength: 0
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES192,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 320,
                    maxEntropyInputLength: 320,
                    maxPersoStringLength: 320,
                    maxAdditStringLength: 320,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 0,
                    maxNonceLength: 0
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 384,
                    maxEntropyInputLength: 384,
                    maxPersoStringLength: 384,
                    maxAdditStringLength: 384,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 0,
                    maxNonceLength: 0
                ),
                #endregion AES
                #region TDES
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.TDES,
                    maxSecurityStrength: 112,
                    minEntropyInputLength: 232,
                    maxEntropyInputLength: 232,
                    maxPersoStringLength: 232,
                    maxAdditStringLength: 232,
                    maxNumberOfBitsPerRequest: 1 << 13,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 32,
                    minNonceLength: 0,
                    maxNonceLength: (long) 1 << 35
                ),
                #endregion TDES
                #region Hash
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA1,
                    maxSecurityStrength: 128,
                    minEntropyInputLength: 128,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 64,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA384,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA512,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA512t224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA512t256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA3224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA3256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA3384,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.Hash,
                    mode: DrbgMode.SHA3512,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                #endregion Hash
                #region HMAC
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA1,
                    maxSecurityStrength: 128,
                    minEntropyInputLength: 128,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 64,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA384,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA512,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA512t224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA512t256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA3224,
                    maxSecurityStrength: 192,
                    minEntropyInputLength: 192,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 96,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA3256,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA3384,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                ),
                new DrbgAttributes(
                    mechanism: DrbgMechanism.HMAC,
                    mode: DrbgMode.SHA3512,
                    maxSecurityStrength: 256,
                    minEntropyInputLength: 256,
                    maxEntropyInputLength: (long) 1 << 35,
                    maxPersoStringLength: (long) 1 << 35,
                    maxAdditStringLength: (long) 1 << 35,
                    maxNumberOfBitsPerRequest: 1 << 19,
                    maxNumberOfRequestsBetweenReseeds: (long) 1 << 48,
                    minNonceLength: 128,
                    maxNonceLength: (long) 1 << 35
                )
                #endregion HMAC
            };
        #endregion DrbgAttributes

        #region CounterDrbgAttributes
        public static List<DrbgCounterAttributes> CounterDrbgAttributes =
            new List<DrbgCounterAttributes>
            {
                #region AES
                new DrbgCounterAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES128,
                    maxSecurityStrength: 128,
                    blockSize: 128,
                    outputLength: 128,
                    keyLength: 128
                ),
                new DrbgCounterAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES192,
                    maxSecurityStrength: 192,
                    blockSize: 128,
                    outputLength: 128,
                    keyLength: 192

                ),
                new DrbgCounterAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.AES256,
                    maxSecurityStrength: 256,
                    blockSize: 128,
                    outputLength: 128,
                    keyLength: 256
                ),
                #endregion AES
                #region TDES
                new DrbgCounterAttributes(
                    mechanism: DrbgMechanism.Counter,
                    mode: DrbgMode.TDES,
                    maxSecurityStrength: 112,
                    blockSize: 64,
                    outputLength: 64,
                    keyLength: 168
                ),
                #endregion TDES
            };
        #endregion CounterDrbgAttributes

        #region HashDrbgAttributes
        public static List<DrbgHashAttributes> HashDrbgAttributes =
            new List<DrbgHashAttributes>
            {
                #region SHA1
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA1, 128, 160, 440),
                #endregion SHA1
                #region SHA2
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA224, 192, 224, 440),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA256, 256, 256, 440),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA384, 256, 384, 888),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA512, 256, 512, 888),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA512t224, 192, 224, 440),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA512t256, 256, 256, 440),
                #endregion SHA2
                #region SHA3
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA3224, 192, 224, 440),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA3256, 256, 256, 440),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA3384, 256, 384, 888),
                new DrbgHashAttributes(DrbgMechanism.Hash, DrbgMode.SHA3512, 256, 512, 888),
                #endregion SHA3
            };
        #endregion HashDrbgAttributes

        public static DrbgCounterAttributes GetCounterDrbgAttributes(DrbgMode mode)
        {
            if (!CounterDrbgAttributes.TryFirst(w => w.Mode == mode, out var result))
            {
                throw new ArgumentException("Invalid mode");
            }

            return result;
        }

        public static DrbgCounterAttributes GetCounterDrbgAttributes(string mode)
        {
            if (!CounterDrbgAttributes
                    .TryFirst(w => w.ModeAsString.Equals(mode, StringComparison.OrdinalIgnoreCase), out var result))
            {
                throw new ArgumentException("Invalid mode");
            }

            return result;
        }

        public static DrbgHashAttributes GetHashDrbgAttributes(DrbgMode mode)
        {
            if (!HashDrbgAttributes.TryFirst(w => w.Mode == mode, out var result))
            {
                throw new ArgumentException("Invalid mode");
            }

            return result;
        }

        public static DrbgHashAttributes GetHashDrbgAttributes(string mode)
        {
            if (!HashDrbgAttributes.TryFirst(w => w.ModeAsString.Equals(mode, StringComparison.OrdinalIgnoreCase), out var result))
            {
                throw new ArgumentException("Invalid mode");
            }

            return result;
        }

        public static DrbgAttributes GetDrbgAttributes(DrbgMechanism mechanism, DrbgMode mode, bool derivationFunction = false)
        {
            DrbgAttributes result = null;

            if (derivationFunction)
            {
                if (!DrbgAttributesWithDerFunc.TryFirst(w => w.Mechanism == mechanism && w.Mode == mode, out result))
                {
                    throw new ArgumentException("Invalid mechanism and/or mode");
                }
            }
            else
            {
                if (!DrbgAttributesWithoutDerFunc.TryFirst(w => w.Mechanism == mechanism && w.Mode == mode, out result))
                {
                    throw new ArgumentException("Invalid mechanism and/or mode");
                }
            }

            return result;
        }

        public static DrbgAttributes GetDrbgAttributes(string mechanism, string mode, bool derivationFunction = false)
        {
            DrbgAttributes result = null;

            if (derivationFunction)
            {
                if (!DrbgAttributesWithDerFunc
                    .TryFirst(w => w.MechanismAsString.Equals(mechanism, StringComparison.OrdinalIgnoreCase) &&
                                   w.ModeAsString.Equals(mode, StringComparison.OrdinalIgnoreCase), out result))
                {
                    throw new ArgumentException("Invalid mechanism and/or mode");
                }
            }
            else
            {
                if (!DrbgAttributesWithoutDerFunc
                    .TryFirst(w => w.MechanismAsString.Equals(mechanism, StringComparison.OrdinalIgnoreCase) &&
                                   w.ModeAsString.Equals(mode, StringComparison.OrdinalIgnoreCase), out result))
                {
                    throw new ArgumentException("Invalid mechanism and/or mode");
                }
            }

            return result;
        }
    }
}
