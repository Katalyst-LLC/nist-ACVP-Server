﻿using System.Collections.Generic;
using System.Numerics;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ecc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests
{
    [TestFixture, FastCryptoTest]
    public class KasFunctionalTestsEcc
    {
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();

        private readonly MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();

        private KasBuilderEcc _subject;
        private Mock<IDsaEcc> _dsa;
        private Mock<IDsaEccFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaEcc>();
            _dsaFactory = new Mock<IDsaEccFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider(false);
            _entropyProviderOtherInfo = new TestableEntropyProvider(false);

            _subject = new KasBuilderEcc(
                new SchemeBuilderEcc(
                    _dsaFactory.Object,
                    _curveFactory,
                    new KdfOneStepFactory(
                        new NativeShaFactory(), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper())
                    ),
                    new KeyConfirmationFactory(new KeyConfirmationMacDataCreator()),
                    new NoKeyConfirmationFactory(new NoKeyConfirmationMacDataCreator()),
                    new OtherInfoFactory(_entropyProviderOtherInfo),
                    _entropyProviderScheme,
                    new DiffieHellmanEcc(),
                    new MqvEcc()
                )
            );
        }

        private static object[] _test_componentOnly = new object[]
        {
            #region full unified
            new object[]
            {
                // label
                "fullUnified P224",
                // scheme
                EccScheme.FullUnified,
                // Curve
                Curve.P224, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("afa0bac49bb60f81fbfdfdc53ffb5213aef7329165f8958427df9150").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("4cd7db4086ddcada30c74e34e5a8399ccdef01f817ce872f5f6abbaa").ToPositiveBigInteger(),
                    new BitString("f0b63a7dee12f1b052c784cf5454a4bedaa8431e5251977cd305901a").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("87ed862898f8949e4c36ee93db8594c0a5bf972293b869d11de3ecff").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("eaf736542e7e562a231b8cc4cde0460aef79d3014bdccaa2099db7ad").ToPositiveBigInteger(),
                    new BitString("b7eb4d0b393d3a6511c14cbd34ae82a29e3f3d67d634b332d73ed169").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("b3856dd8b0aa7a5b8a1c4d987c0eba11f2823c5f5411a4f5fc3d31ea").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("b138148c01de7a05d567fed2534e04a2967044a7e111cb7e626df469").ToPositiveBigInteger(),
                    new BitString("b166aab7a1deeca20d5efff7495374bdb0225cd37d08013b11de587f").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("426f17cda61b605bb58fc37a177946c2aa62ffcc6de698ba6f227bb4").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("b654614239fdf9e420d6485536f4eeae69ee541b07a7519200829f87").ToPositiveBigInteger(),
                    new BitString("19d7eb059b9cdf0a264b1dbec05102fc9d4681819ee21a6b112a0097").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("76789da1208094ba4f2228d3eb4c58b18e4c5b4dd3b3fa935b70c7293861672f12f51917c6c21d3c9f00540aa5c626fffae536f01524f171"),
                // expected Z hash
                new BitString("c01b3b1532189cc55255e6368afdd8b55cdfea44fc3a3092e9cc030ce8e90547a70389bf9b01121604152ebff4ff5550ed14bef2f85897580f3b20f5466933ed")
            },
            new object[]
            {
                // label
                "fullUnified P224 inverse",
                // scheme
                EccScheme.FullUnified,
                // Curve
                Curve.P224, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("b3856dd8b0aa7a5b8a1c4d987c0eba11f2823c5f5411a4f5fc3d31ea").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("b138148c01de7a05d567fed2534e04a2967044a7e111cb7e626df469").ToPositiveBigInteger(),
                    new BitString("b166aab7a1deeca20d5efff7495374bdb0225cd37d08013b11de587f").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("426f17cda61b605bb58fc37a177946c2aa62ffcc6de698ba6f227bb4").ToPositiveBigInteger(), 
                // public ephem this party
                new EccPoint(
                    new BitString("b654614239fdf9e420d6485536f4eeae69ee541b07a7519200829f87").ToPositiveBigInteger(),
                    new BitString("19d7eb059b9cdf0a264b1dbec05102fc9d4681819ee21a6b112a0097").ToPositiveBigInteger()
                ),
                // other party Id
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("afa0bac49bb60f81fbfdfdc53ffb5213aef7329165f8958427df9150").ToPositiveBigInteger(),
                // public static other party
                new EccPoint(
                    new BitString("4cd7db4086ddcada30c74e34e5a8399ccdef01f817ce872f5f6abbaa").ToPositiveBigInteger(),
                    new BitString("f0b63a7dee12f1b052c784cf5454a4bedaa8431e5251977cd305901a").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("87ed862898f8949e4c36ee93db8594c0a5bf972293b869d11de3ecff").ToPositiveBigInteger(),
                // public ephem other party
                new EccPoint(
                    new BitString("eaf736542e7e562a231b8cc4cde0460aef79d3014bdccaa2099db7ad").ToPositiveBigInteger(),
                    new BitString("b7eb4d0b393d3a6511c14cbd34ae82a29e3f3d67d634b332d73ed169").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("76789da1208094ba4f2228d3eb4c58b18e4c5b4dd3b3fa935b70c7293861672f12f51917c6c21d3c9f00540aa5c626fffae536f01524f171"),
                // expected Z hash
                new BitString("c01b3b1532189cc55255e6368afdd8b55cdfea44fc3a3092e9cc030ce8e90547a70389bf9b01121604152ebff4ff5550ed14bef2f85897580f3b20f5466933ed")
            },
            #endregion full unified

            #region fullMqv
            new object[]
            {
                // label
                "fullMqv B233",
                // scheme
                EccScheme.FullMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("000000967fb12273a0ff296dc91b809ff470a02478be51f477bb2ed326d1c031").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000b85095ebef7af7b35a630a43033fdc19b9feb9b44caf23d258c2add087").ToPositiveBigInteger(),
                    new BitString("0000008fa40e6af465839df3909eab40ac288e4d7a519745e3f2c9ddb1cbf1d6").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("000000f0dfa240c97f677ed554b342759be35a323dc1cb32da0a722a0322b123").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000536707130abccd2b094dff90c1570472edda6d879a3fad6b4a9319fc2f").ToPositiveBigInteger(),
                    new BitString("000000760d211de1291b970e873f7c29e3f7ccea31c1d4a05b6ab6ee2e8c3c66").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("0000002cd87049a9cc5045017c7bd3d7ead0e3d72c3b6137e967030ac9d0e06d").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("00000056954ec0f3458ca9661240420dc77a05d845f4583267391ca18da6ec75").ToPositiveBigInteger(),
                    new BitString("000000694f096dcfaeb57924b5bba879c05d0e99a36c86de22e83d80f4b3470c").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("0000009ee4daec27f1b93e6dd88e0371a0173c67df74ab7e0144a9ea76cbdc4a").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("000000e5c1b24b38e26bb17cb7174061968add442ec6df8f34a525924fa7ee68").ToPositiveBigInteger(),
                    new BitString("000000884982550956b1f4e87fc5aac790a4da91809bbb0e53511453c4724193").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("005eeb557dff8bde571a1394a5d6c568e5478b3885cf25f8d39f56930b11"),
                // expected Z hash
                new BitString("0461921f494ef8db3207ca6a825d30610f510dc33b72b19f26bc152a4a434e5a01b6bf37cdc8375c9e240217f77c9854e25ff1251260a41ffb4570e2e03f953b")
            },
            new object[]
            {
                // label
                "fullMqv B233 inverse",
                // scheme
                EccScheme.FullMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                
                // this Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("0000002cd87049a9cc5045017c7bd3d7ead0e3d72c3b6137e967030ac9d0e06d").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("00000056954ec0f3458ca9661240420dc77a05d845f4583267391ca18da6ec75").ToPositiveBigInteger(),
                    new BitString("000000694f096dcfaeb57924b5bba879c05d0e99a36c86de22e83d80f4b3470c").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("0000009ee4daec27f1b93e6dd88e0371a0173c67df74ab7e0144a9ea76cbdc4a").ToPositiveBigInteger(), 
                // public ephem this party
                new EccPoint(
                    new BitString("000000e5c1b24b38e26bb17cb7174061968add442ec6df8f34a525924fa7ee68").ToPositiveBigInteger(),
                    new BitString("000000884982550956b1f4e87fc5aac790a4da91809bbb0e53511453c4724193").ToPositiveBigInteger()
                ),

                // other party Id
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("000000967fb12273a0ff296dc91b809ff470a02478be51f477bb2ed326d1c031").ToPositiveBigInteger(),
                // public static other party
                new EccPoint(
                    new BitString("000000b85095ebef7af7b35a630a43033fdc19b9feb9b44caf23d258c2add087").ToPositiveBigInteger(),
                    new BitString("0000008fa40e6af465839df3909eab40ac288e4d7a519745e3f2c9ddb1cbf1d6").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("000000f0dfa240c97f677ed554b342759be35a323dc1cb32da0a722a0322b123").ToPositiveBigInteger(),
                // public ephem other party
                new EccPoint(
                    new BitString("000000536707130abccd2b094dff90c1570472edda6d879a3fad6b4a9319fc2f").ToPositiveBigInteger(),
                    new BitString("000000760d211de1291b970e873f7c29e3f7ccea31c1d4a05b6ab6ee2e8c3c66").ToPositiveBigInteger()
                ),

                // expected Z
                new BitString("005eeb557dff8bde571a1394a5d6c568e5478b3885cf25f8d39f56930b11"),
                // expected Z hash
                new BitString("0461921f494ef8db3207ca6a825d30610f510dc33b72b19f26bc152a4a434e5a01b6bf37cdc8375c9e240217f77c9854e25ff1251260a41ffb4570e2e03f953b")
            },
            #endregion fullMqv

            #region ephemeralUnified
            new object[]
            {
                // label
                "ephemeralUnified B233",
                // scheme
                EccScheme.EphemeralUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem this party
                new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd").ToPositiveBigInteger(),
                    new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem other party
                new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79").ToPositiveBigInteger(),
                    new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e"),
                // expected Z hash
                new BitString("694eabfc6465a165e20ca9dc9b605de69ed28f4582e18d785c6136ad2b61636907d4c724d66541d540fc23008f94e3c18917105d848cdd1ed3f5708ab26dd1cf")
            },
            new object[]
            {
                // label
                "ephemeralUnified B233 inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem this party
                new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79").ToPositiveBigInteger(),
                    new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem other party
                new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd").ToPositiveBigInteger(),
                    new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e"),
                // expected Z hash
                new BitString("694eabfc6465a165e20ca9dc9b605de69ed28f4582e18d785c6136ad2b61636907d4c724d66541d540fc23008f94e3c18917105d848cdd1ed3f5708ab26dd1cf")
            },
            #endregion ephemeralUnified

            #region onePassUnified
            new object[]
            {
                // label
                "onePassUnified P224",
                // scheme
                EccScheme.OnePassUnified,
                // Curve
                Curve.P224, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d224,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("327003d26deb2e8f99268e81c3397202b06e8810d6fbae3fd31b5dab").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("71d6434952ae67b1c29d6a2d150441c0a963d4d6158e6c795729ab08").ToPositiveBigInteger(),
                    new BitString("6c3e32c743f869b03a534c8af838803bf6667ccc47d653b29cc72b84").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("22999a08268248e9fbae8324f06b86bbb9589c7ce1881f573634a3ef").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("2a384048294a6216be1edd461dd83f3c22baf01758fae0970ecc7a1d").ToPositiveBigInteger(),
                    new BitString("38f933065943cfd9bee8e5d71afc0f53f43c7840c40af8c0d794f725").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("94541a493ba8f5ec06553ad6693b87c5edff1f48c5bc0b6519a36c67").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("e5cc351179cfc5535ed31ce61b76f10109aa951ef82cd130fd5a5103").ToPositiveBigInteger(),
                    new BitString("fc6c49607742010422bf34ae61d4eb892eadf4fe4daed4087950233b").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("197f129e5723fbb5bad9b2f72d4a51a3ab0445854f991105eef5c2a1119dbee5a04667034542651ac97c4f59ef72073bdd7c340eb8a2da97"),
                // expected Z hash
                new BitString("1f22f9797de103aefbba3443297da89bb1e8389c77550b44ed413c77")
            },
            new object[]
            {
                // label
                "onePassUnified P224 inverse",
                // scheme
                EccScheme.OnePassUnified,
                // Curve
                Curve.P224, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d224,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                
                // this Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("94541a493ba8f5ec06553ad6693b87c5edff1f48c5bc0b6519a36c67").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("e5cc351179cfc5535ed31ce61b76f10109aa951ef82cd130fd5a5103").ToPositiveBigInteger(),
                    new BitString("fc6c49607742010422bf34ae61d4eb892eadf4fe4daed4087950233b").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem this party
                null,

                // other party Id
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("327003d26deb2e8f99268e81c3397202b06e8810d6fbae3fd31b5dab").ToPositiveBigInteger(),
                // public static other party
                new EccPoint(
                    new BitString("71d6434952ae67b1c29d6a2d150441c0a963d4d6158e6c795729ab08").ToPositiveBigInteger(),
                    new BitString("6c3e32c743f869b03a534c8af838803bf6667ccc47d653b29cc72b84").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("22999a08268248e9fbae8324f06b86bbb9589c7ce1881f573634a3ef").ToPositiveBigInteger(),
                // public ephem other party
                new EccPoint(
                    new BitString("2a384048294a6216be1edd461dd83f3c22baf01758fae0970ecc7a1d").ToPositiveBigInteger(),
                    new BitString("38f933065943cfd9bee8e5d71afc0f53f43c7840c40af8c0d794f725").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("197f129e5723fbb5bad9b2f72d4a51a3ab0445854f991105eef5c2a1119dbee5a04667034542651ac97c4f59ef72073bdd7c340eb8a2da97"),
                // expected Z hash
                new BitString("1f22f9797de103aefbba3443297da89bb1e8389c77550b44ed413c77")
            },
            #endregion onePassUnified

            #region onePassMqv
            new object[]
            {
                // label
                "onePassMqv B233",
                // scheme
                EccScheme.OnePassMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00000052c23ea9f876d6d847c0c54578a16612ce384f69d6d4751bda6c107bc3").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("0000017f5b2f350ffc44026977cbbe5a9f33f8d3a1beace814eb0e3693c8202b").ToPositiveBigInteger(),
                    new BitString("000001623a8e762e7b21e10359a046b64aca711c146e253dd2fa35a2f12cd5ab").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("000000059bf1926cda4bd3823f3e0ed440a18c1a54a33701eafd053a2e6ffae5").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("00000181cfd86c22f5d417a0228418581e7ce910ce8cc04ff56593af460da035").ToPositiveBigInteger(),
                    new BitString("000000586324d10c8715dbe36dd7b8ff01c9c95340bd5e7c0d229e7279887c0b").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("0000007ad5b7faad03268f34de6cef03527efc1ca333ffa66172d386a0b4ab88").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000a24f0077bee53e51fd02beddadec5744f48068195313c5060ba93accbf").ToPositiveBigInteger(),
                    new BitString("000001cb61023be720448673bcd653ee1b17e8426b7467160d89a4bce9eea19d").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("007bf51ab995289a403b20dc669c509af1eeadbe6d4e710d9420964a4849"),
                // expected Z hash
                new BitString("60fb697f0f2e456df819aa5e38730410cc8cd712d7f4bf1827f787912bcf73d4")
            },
            new object[]
            {
                // label
                "onePassMqv B233 inverse",
                // scheme
                EccScheme.OnePassMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("0000007ad5b7faad03268f34de6cef03527efc1ca333ffa66172d386a0b4ab88").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000a24f0077bee53e51fd02beddadec5744f48068195313c5060ba93accbf").ToPositiveBigInteger(),
                    new BitString("000001cb61023be720448673bcd653ee1b17e8426b7467160d89a4bce9eea19d").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00000052c23ea9f876d6d847c0c54578a16612ce384f69d6d4751bda6c107bc3").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("0000017f5b2f350ffc44026977cbbe5a9f33f8d3a1beace814eb0e3693c8202b").ToPositiveBigInteger(),
                    new BitString("000001623a8e762e7b21e10359a046b64aca711c146e253dd2fa35a2f12cd5ab").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("000000059bf1926cda4bd3823f3e0ed440a18c1a54a33701eafd053a2e6ffae5").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("00000181cfd86c22f5d417a0228418581e7ce910ce8cc04ff56593af460da035").ToPositiveBigInteger(),
                    new BitString("000000586324d10c8715dbe36dd7b8ff01c9c95340bd5e7c0d229e7279887c0b").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("007bf51ab995289a403b20dc669c509af1eeadbe6d4e710d9420964a4849"),
                // expected Z hash
                new BitString("60fb697f0f2e456df819aa5e38730410cc8cd712d7f4bf1827f787912bcf73d4")
            },
            #endregion onePassMqv

            #region onePassDh
            new object[]
            {
                // label
                "onePassDh K233",
                // scheme
                EccScheme.OnePassDh,
                // Curve
                Curve.K233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d384,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                null,
                // private ephem this party
                new BitString("00000051b52a6634723501c6c61ab3b05c40989badaf8b10a25d8dab76cf3b5d").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000be4cee69300f764d5b0609ed2e1fc2d6309e84381024f99b353761bfd6").ToPositiveBigInteger(),
                    new BitString("0000018a523dc6187fbf5b255edd680b0ef17c2f868391acdc25d460c57b81ce").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("0000005e97ad3ce7e0e2afd280713a89f1ce80f3ec1cbb9620b3ffefce444a4a").ToPositiveBigInteger(),
                // public static other party
                new EccPoint(
                    new BitString("0000001187f250bd7b606c8dbcccb9d5b2f4e6db55855789d736c56270605fd5").ToPositiveBigInteger(),
                    new BitString("000001a521b393b34e262020be20af81bbe12ca21003c9292942478e132ed6cc").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("0138fb63412715f4b9e2c4b4ad3f9a8a3c10d9e2d9a2a971808683bbca97"),
                // expected Z hash
                new BitString("28d618ddb986be45e024f331ba2adce2390a8bb5f034eaaad1191bd7bd13e1974aac9faeabcc608060b00ed3e04715d1")
            },
            new object[]
            {
                // label
                "onePassDh K233 inverse",
                // scheme
                EccScheme.OnePassDh,
                // Curve
                Curve.K233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d384,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                
                // this Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("0000005e97ad3ce7e0e2afd280713a89f1ce80f3ec1cbb9620b3ffefce444a4a").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("0000001187f250bd7b606c8dbcccb9d5b2f4e6db55855789d736c56270605fd5").ToPositiveBigInteger(),
                    new BitString("000001a521b393b34e262020be20af81bbe12ca21003c9292942478e132ed6cc").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem this party
                null,

                // other party Id
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                null,
                // private ephem other party
                new BitString("00000051b52a6634723501c6c61ab3b05c40989badaf8b10a25d8dab76cf3b5d").ToPositiveBigInteger(),
                // public ephem other party
                new EccPoint(
                    new BitString("000000be4cee69300f764d5b0609ed2e1fc2d6309e84381024f99b353761bfd6").ToPositiveBigInteger(),
                    new BitString("0000018a523dc6187fbf5b255edd680b0ef17c2f868391acdc25d460c57b81ce").ToPositiveBigInteger()
                ),

                // expected Z
                new BitString("0138fb63412715f4b9e2c4b4ad3f9a8a3c10d9e2d9a2a971808683bbca97"),
                // expected Z hash
                new BitString("28d618ddb986be45e024f331ba2adce2390a8bb5f034eaaad1191bd7bd13e1974aac9faeabcc608060b00ed3e04715d1")
            },
            #endregion onePassDh

            #region StaticUnified
            new object[]
            {
                // label
                "staticUnified B233",
                // scheme
                EccScheme.StaticUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("0000007b530ac501d9f2bb4a0e52b95f87809b468f69cfb143a7a63f34ee03be").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000c67f22adeafe0be6a69d16ce6782f3949008589082c6104b6dbc613c32").ToPositiveBigInteger(),
                    new BitString("000000393543e586d2b4039c0a80ae6054c90663548e1fec381967244f4b7b33").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("00000042f7332ced6378f3b36c490cb7f6f5ff2782082c37e744463c5ed40429").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("00000152e639c21fb0659b381cd46fc7eaa4ad34a5be7581d62dd5c60c7fc0b3").ToPositiveBigInteger(),
                    new BitString("00000037ab7ab4b51a0053487cf951b3840fe5941dcc7a53fb04e9d29845bf1c").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("01850e5218501c977edef608abb1af00b34f06d09b118b7cfaa30b372468"),
                // expected Z hash
                new BitString("ed49fd480ecbbf3dc46a67ea581f26a1a661996184cd2b5ce29a6f25eb6d7cbcc7565c4193decd7183f967abbb3487e998647ee9a8d6d6347c1757f7b7eecece")
            },
            new object[]
            {
                // label
                "staticUnified B233 inverse",
                // scheme
                EccScheme.StaticUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("00000042f7332ced6378f3b36c490cb7f6f5ff2782082c37e744463c5ed40429").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("00000152e639c21fb0659b381cd46fc7eaa4ad34a5be7581d62dd5c60c7fc0b3").ToPositiveBigInteger(),
                    new BitString("00000037ab7ab4b51a0053487cf951b3840fe5941dcc7a53fb04e9d29845bf1c").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("0000007b530ac501d9f2bb4a0e52b95f87809b468f69cfb143a7a63f34ee03be").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000c67f22adeafe0be6a69d16ce6782f3949008589082c6104b6dbc613c32").ToPositiveBigInteger(),
                    new BitString("000000393543e586d2b4039c0a80ae6054c90663548e1fec381967244f4b7b33").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("01850e5218501c977edef608abb1af00b34f06d09b118b7cfaa30b372468"),
                // expected Z hash
                new BitString("ed49fd480ecbbf3dc46a67ea581f26a1a661996184cd2b5ce29a6f25eb6d7cbcc7565c4193decd7183f967abbb3487e998647ee9a8d6d6347c1757f7b7eecece")
            },
            #endregion StaticUnified
        };

        [Test]
        [TestCaseSource(nameof(_test_componentOnly))]
        public void ShouldComponentOnlyCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
            ModeValues modeValue,
            DigestSizes digestSize,
            KeyAgreementRole keyAgreementRole,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString expectedZ,
            BitString expectedHashZ
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var vPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    otherPartyPublicStaticKey != null ? new EccKeyPair(otherPartyPublicStaticKey) : null,
                    otherPartyPublicEphemKey != null ? new EccKeyPair(otherPartyPublicEphemKey) : null,
                    null,
                    null,
                    null
                );

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new NativeShaFactory().GetShaInstance(new HashFunction(modeValue, digestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(thisPartyId)
                .BuildNoKdfNoKc()
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(vPartySharedInformation);

            Assert.That(result.Success, nameof(result.Success));
            Assert.That(result.Z, Is.EqualTo(expectedZ), nameof(result.Z));
            Assert.That(result.Tag, Is.EqualTo(expectedHashZ), nameof(result.Tag));
        }

        private static object[] _test_NoKeyConfirmation = new object[]
        {
            #region full unified
            #region ccm
            new object[]
            {
                // label
                "fullUnified P-224 ccm",
                // scheme
                EccScheme.FullUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("61e2e059c76193e85b1c3bab21dd046c"),
                // aesCcmNonce
                new BitString("67edd7d7099194"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("82d096b03d30261c8c9b3a05837cd4af0b042c8dade0085714b2fe97").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("ab8f54bf88863e78f3dde95e76229486fb7f0c794c08a53853503b3d").ToPositiveBigInteger(),
                    new BitString("f78714390e3838278d04b7dd86033ed8739543ca088e908903d17f06").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("446570496f59b51fefa623e5596351a9e81d86624abf46619cf3653a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("1794ef4915a996eb0cf04efa690d765d64b3e8485e4714eb8dbbf453").ToPositiveBigInteger(),
                    new BitString("f8c9ac97681b2d92f47eee47c3476b783ca718aa75904ea23a14212e").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("c2aa133e38b46975ac5cd4d08b37c2862f4d38af76c78069e8e9eb9d").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("54900cc1ebc1abdfc89f1542b727108a4a7381e20078c975291f824a").ToPositiveBigInteger(),
                    new BitString("7f2ec773047ced7b0750ddf00eb42275ae1dfed60b834f4552ab777a").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("fe59e96ad4383af7f545e3bbbb6e4b2e491cb02df8b6b262ad9e2dff").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("c669be888dd147db1b10d40c80d07843139a60f165716a11c8635945").ToPositiveBigInteger(),
                    new BitString("c9fe1aa97c9e307180d8421e594eca4f9cacb335f0ee90926596b33a").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("00b44bea10b6c28a525a033dcc2c6df40fa7b0d3e4cbcbb8682f7fe3605fbee412b64d42bd715fb1ffab0f37a0b79182c6c87a57ee583959"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696401da157ea6f2f5a423f3c9e8598a950b971c402d0d65a290845fdea04c65f23075988c07"),
                // expectedDkm
                new BitString("e57457bd98c2645e69c07edaeee36ac1"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676561e2e059c76193e85b1c3bab21dd046c"),
                // expectedTag
                new BitString("24c648c54767283adc5df1227dfa"),
            },
            new object[]
            {
                // label
                "fullUnified P-224 ccm inverse",
                // scheme
                EccScheme.FullUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("61e2e059c76193e85b1c3bab21dd046c"),
                // aesCcmNonce
                new BitString("67edd7d7099194"),
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("c2aa133e38b46975ac5cd4d08b37c2862f4d38af76c78069e8e9eb9d").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("54900cc1ebc1abdfc89f1542b727108a4a7381e20078c975291f824a").ToPositiveBigInteger(),
                    new BitString("7f2ec773047ced7b0750ddf00eb42275ae1dfed60b834f4552ab777a").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("fe59e96ad4383af7f545e3bbbb6e4b2e491cb02df8b6b262ad9e2dff").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("c669be888dd147db1b10d40c80d07843139a60f165716a11c8635945").ToPositiveBigInteger(),
                    new BitString("c9fe1aa97c9e307180d8421e594eca4f9cacb335f0ee90926596b33a").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("82d096b03d30261c8c9b3a05837cd4af0b042c8dade0085714b2fe97").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("ab8f54bf88863e78f3dde95e76229486fb7f0c794c08a53853503b3d").ToPositiveBigInteger(),
                    new BitString("f78714390e3838278d04b7dd86033ed8739543ca088e908903d17f06").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("446570496f59b51fefa623e5596351a9e81d86624abf46619cf3653a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("1794ef4915a996eb0cf04efa690d765d64b3e8485e4714eb8dbbf453").ToPositiveBigInteger(),
                    new BitString("f8c9ac97681b2d92f47eee47c3476b783ca718aa75904ea23a14212e").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("00b44bea10b6c28a525a033dcc2c6df40fa7b0d3e4cbcbb8682f7fe3605fbee412b64d42bd715fb1ffab0f37a0b79182c6c87a57ee583959"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696401da157ea6f2f5a423f3c9e8598a950b971c402d0d65a290845fdea04c65f23075988c07"),
                // expectedDkm
                new BitString("e57457bd98c2645e69c07edaeee36ac1"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676561e2e059c76193e85b1c3bab21dd046c"),
                // expectedTag
                new BitString("24c648c54767283adc5df1227dfa"),
            },
            #endregion ccm

            #region hmac
            new object[]
            {
                // label
                "fullUnified P-224 hmac",
                // scheme
                EccScheme.FullUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("7766c0318fd1a74152f53acf1799b27d"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("1d2804c33c2baafff6ef82c02a14663d6d36f7361d00084f75c912db").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("b226a87390bdbcbb2f4f5977251f429084328199eea67fd29b59cddd").ToPositiveBigInteger(),
                    new BitString("7c40e9abf884da1029905f19162057a5df9da1cc40e174c14a7d29d4").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("08933020a46085bdffb652aace66e0ec70811572a2da14837b465854").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("af3cf6bf6b0e0895e26d10c300121f7436a00d849a8e6829f1588a0a").ToPositiveBigInteger(),
                    new BitString("bbbe17d213c05967b85cff998d91ed689d7f91fee8c6f047a15805d1").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("c9af0aeea11d84093af017af55b9a6dcb299bf0be8ea39428074af21").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("16435075d0290f276e976d717f0d878aa4c758644ec4893966147232").ToPositiveBigInteger(),
                    new BitString("f2e4141e7c4699fdcd2fb778f8d409c9e6ce9e4e6bc9defbe3c787df").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("59d06408217856b16fe0b79fedcbe4081f4cf1f9f209d600ee6e8dec").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("d8e6e3507fd384b35fcd61f0bd3cf1087b49aeb4ef47e93db32a7d6d").ToPositiveBigInteger(),
                    new BitString("de838ee89655d2cf32e4e4a6e1a3a1217218c063f3d25106b764579e").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("d89e645dfb4ecac7ac1956e20d47cc4a6dfe9d0186a6e1d5bfd7ec51c2ea0da28e7b2a68ae65f30de388e53efaa336133db140d02b44bddf"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369640f4228649543a9942f39b9d079a1ef20ec5eb61a555a58a986705c0d4c8a1223a1066ce7"),
                // expectedDkm
                new BitString("00fa87d01d5cb81b22e730084835"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167657766c0318fd1a74152f53acf1799b27d"),
                // expectedTag
                new BitString("8e45d8b400321e0e2f0ea904b836"),
            },
            new object[]
            {
                // label
                "fullUnified P-224 hmac inverse",
                // scheme
                EccScheme.FullUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("7766c0318fd1a74152f53acf1799b27d"),
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("c9af0aeea11d84093af017af55b9a6dcb299bf0be8ea39428074af21").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("16435075d0290f276e976d717f0d878aa4c758644ec4893966147232").ToPositiveBigInteger(),
                    new BitString("f2e4141e7c4699fdcd2fb778f8d409c9e6ce9e4e6bc9defbe3c787df").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("59d06408217856b16fe0b79fedcbe4081f4cf1f9f209d600ee6e8dec").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("d8e6e3507fd384b35fcd61f0bd3cf1087b49aeb4ef47e93db32a7d6d").ToPositiveBigInteger(),
                    new BitString("de838ee89655d2cf32e4e4a6e1a3a1217218c063f3d25106b764579e").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("1d2804c33c2baafff6ef82c02a14663d6d36f7361d00084f75c912db").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("b226a87390bdbcbb2f4f5977251f429084328199eea67fd29b59cddd").ToPositiveBigInteger(),
                    new BitString("7c40e9abf884da1029905f19162057a5df9da1cc40e174c14a7d29d4").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("08933020a46085bdffb652aace66e0ec70811572a2da14837b465854").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("af3cf6bf6b0e0895e26d10c300121f7436a00d849a8e6829f1588a0a").ToPositiveBigInteger(),
                    new BitString("bbbe17d213c05967b85cff998d91ed689d7f91fee8c6f047a15805d1").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("d89e645dfb4ecac7ac1956e20d47cc4a6dfe9d0186a6e1d5bfd7ec51c2ea0da28e7b2a68ae65f30de388e53efaa336133db140d02b44bddf"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369640f4228649543a9942f39b9d079a1ef20ec5eb61a555a58a986705c0d4c8a1223a1066ce7"),
                // expectedDkm
                new BitString("00fa87d01d5cb81b22e730084835"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167657766c0318fd1a74152f53acf1799b27d"),
                // expectedTag
                new BitString("8e45d8b400321e0e2f0ea904b836"),
            },
            #endregion hmac
            #endregion full unified

            #region fullMqv
            #region ccm
            new object[]
            {
                // label
                "fullMqv P-224 ccm",
                // scheme
                EccScheme.FullMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("4fd06d29728e05302c2ff61c67c3a513"),
                // aesCcmNonce
                new BitString("dfc6b2b7373b17"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("ac68e30e8c8f9d1104bbe8afb928170a317e70e236139f60c9640410").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("1fd7b3cdd8c1db71715284d5ab6ac9a3eb0bc17cc399adc6c8f19dff").ToPositiveBigInteger(),
                    new BitString("66d9a41a897400a2f77dfb66fe2e4c258e52eed025f2abd57d47956b").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("e965f34393edd4332ae22cd9510ebfc3580abf2c78341a4174188353").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("367a421e16c8f5cba37a6673b09c1bb4288a3e870e9e5dcf7e88155e").ToPositiveBigInteger(),
                    new BitString("5639292f29eaf5a7517cbd4a1cd668edecce5d8b275438f1b7f5967b").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("6db908ad40319d2cd9862de529438b132ff7f8040a051086413084a6").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("de687a3cba64d7d082c0aa88a8996d7146b93b6072c99230bb6491ac").ToPositiveBigInteger(),
                    new BitString("386cfd993281bfbf98c1e5e54615386ef97a3ca349e9a1f9730e9ee4").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("58436662ef032b8d6bfcac05392bf02f15c976c77b854735fc9905ae").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("647a745495a37761c19cc25d3f73b50ae3bf1093578ff22cc9a91a21").ToPositiveBigInteger(),
                    new BitString("c2a364da961e8863ae382b15ad60ff7f9593c8d644da7c304a65c347").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("da5a20861c762f327158016851b8063df8e14f8ed356007a1ce6c5ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964aa833b760f1a685d51ade29bd798e193487cdafe4aed5c3a906b38e51394e4ab9f97215c"),
                // expectedDkm
                new BitString("ded5915843575c033439d37fb2877885"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167654fd06d29728e05302c2ff61c67c3a513"),
                // expectedTag
                new BitString("b224511d6bc007c39f32371cf5c5"),
            },
            new object[]
            {
                // label
                "fullMqv P-224 ccm inverse",
                // scheme
                EccScheme.FullMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("4fd06d29728e05302c2ff61c67c3a513"),
                // aesCcmNonce
                new BitString("dfc6b2b7373b17"),
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("6db908ad40319d2cd9862de529438b132ff7f8040a051086413084a6").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("de687a3cba64d7d082c0aa88a8996d7146b93b6072c99230bb6491ac").ToPositiveBigInteger(),
                    new BitString("386cfd993281bfbf98c1e5e54615386ef97a3ca349e9a1f9730e9ee4").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("58436662ef032b8d6bfcac05392bf02f15c976c77b854735fc9905ae").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("647a745495a37761c19cc25d3f73b50ae3bf1093578ff22cc9a91a21").ToPositiveBigInteger(),
                    new BitString("c2a364da961e8863ae382b15ad60ff7f9593c8d644da7c304a65c347").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("ac68e30e8c8f9d1104bbe8afb928170a317e70e236139f60c9640410").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("1fd7b3cdd8c1db71715284d5ab6ac9a3eb0bc17cc399adc6c8f19dff").ToPositiveBigInteger(),
                    new BitString("66d9a41a897400a2f77dfb66fe2e4c258e52eed025f2abd57d47956b").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("e965f34393edd4332ae22cd9510ebfc3580abf2c78341a4174188353").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("367a421e16c8f5cba37a6673b09c1bb4288a3e870e9e5dcf7e88155e").ToPositiveBigInteger(),
                    new BitString("5639292f29eaf5a7517cbd4a1cd668edecce5d8b275438f1b7f5967b").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("da5a20861c762f327158016851b8063df8e14f8ed356007a1ce6c5ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964aa833b760f1a685d51ade29bd798e193487cdafe4aed5c3a906b38e51394e4ab9f97215c"),
                // expectedDkm
                new BitString("ded5915843575c033439d37fb2877885"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167654fd06d29728e05302c2ff61c67c3a513"),
                // expectedTag
                new BitString("b224511d6bc007c39f32371cf5c5"),
            },
            #endregion ccm

            #region hmac
            new object[]
            {
                // label
                "fullMqv P-224 hmac",
                // scheme
                EccScheme.FullMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("2012eacb9faa235b7caaa39fa497e7f0"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("b5603c4fab02eb19c1c13ea5d0d3fbfabe69c96f83aa3b9210d74534").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("0a271fc0954b5ef341e023a7abfb42d3aa80e0817b89c847e0047d46").ToPositiveBigInteger(),
                    new BitString("19deb4118a9bd8aeb9c04642f772ef2ce5e4b0b5228cdd3920732cb7").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("b338f700280af1bf5a85495b2233a7702e312e6787e38e7efa9dc0bb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("28b752d989ab278a4c74fa6df707ef4098ac42b5ae9b9c816a52a116").ToPositiveBigInteger(),
                    new BitString("cfb222530016fd0fab2871ccb7c4cbebcdd8f9d7fe0cec9d8eb1cfa7").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("6ccb96e03fd30cb97ace846e5338b918f1e34e5d49a11369f1cdd6c7").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("cfc6717d0886fbe6194995c252c7a340198bacb729fe5e873cdb75bf").ToPositiveBigInteger(),
                    new BitString("cf033741de78d734886cc5099cebec8e489ac02523f37c28f5daed35").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("e11937f25625b1df3286786739b98f44d10853bcf3abfe9c166f368a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("636f6ca55bca968b01ee44f9d20668e0fd8e8ee2b7b0076460d9f5e9").ToPositiveBigInteger(),
                    new BitString("64bac14c4ffdaeaa6d5d43b7d323af51edf8e150a1ddfa02aad86301").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("64de4091bfad7f20ecd6f394af511e53d35bd5942ef262b6d4838be8"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696402f37462fafad03be321f9282678cd5bbd662d05a3480c1b895ac55acbdbb289a26cfafe"),
                // expectedDkm
                new BitString("0c6f2bd69f2be89fee7fb53357fd"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167652012eacb9faa235b7caaa39fa497e7f0"),
                // expectedTag
                new BitString("2e8769611dd9f9fd"),
            },
            new object[]
            {
                // label
                "fullMqv P-224 hmac inverse",
                // scheme
                EccScheme.FullMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("2012eacb9faa235b7caaa39fa497e7f0"),
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("6ccb96e03fd30cb97ace846e5338b918f1e34e5d49a11369f1cdd6c7").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("cfc6717d0886fbe6194995c252c7a340198bacb729fe5e873cdb75bf").ToPositiveBigInteger(),
                    new BitString("cf033741de78d734886cc5099cebec8e489ac02523f37c28f5daed35").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("e11937f25625b1df3286786739b98f44d10853bcf3abfe9c166f368a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("636f6ca55bca968b01ee44f9d20668e0fd8e8ee2b7b0076460d9f5e9").ToPositiveBigInteger(),
                    new BitString("64bac14c4ffdaeaa6d5d43b7d323af51edf8e150a1ddfa02aad86301").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("b5603c4fab02eb19c1c13ea5d0d3fbfabe69c96f83aa3b9210d74534").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("0a271fc0954b5ef341e023a7abfb42d3aa80e0817b89c847e0047d46").ToPositiveBigInteger(),
                    new BitString("19deb4118a9bd8aeb9c04642f772ef2ce5e4b0b5228cdd3920732cb7").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("b338f700280af1bf5a85495b2233a7702e312e6787e38e7efa9dc0bb").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("28b752d989ab278a4c74fa6df707ef4098ac42b5ae9b9c816a52a116").ToPositiveBigInteger(),
                    new BitString("cfb222530016fd0fab2871ccb7c4cbebcdd8f9d7fe0cec9d8eb1cfa7").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("64de4091bfad7f20ecd6f394af511e53d35bd5942ef262b6d4838be8"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696402f37462fafad03be321f9282678cd5bbd662d05a3480c1b895ac55acbdbb289a26cfafe"),
                // expectedDkm
                new BitString("0c6f2bd69f2be89fee7fb53357fd"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167652012eacb9faa235b7caaa39fa497e7f0"),
                // expectedTag
                new BitString("2e8769611dd9f9fd"),
            },
            #endregion hmac
            #endregion fullMqv

            #region ephemeralunified
            #region hmac
            new object[]
            {
                // label
                "ephem unified P-224 hmac",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("123817b69860aa195a9cb405b9554750"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("e935434303d842605d07112b3b7789ccbe4c6d987db5fa15ea1cdadb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("784028a2246950401ec81f8e4e03fd3765c4da4d45eba652ed5ba2b5").ToPositiveBigInteger(),
                    new BitString("d071c32634bcf058f072493b6943453a0bd117f5ee5c5866f037f6ab").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("d58751997b3a551ccdc6f507bf6ab87e2be7f8267067a455a5815a54").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("c7893ced15c3009b93cb0abeaea2ede308b67fbbd902c6c5d94c24a4").ToPositiveBigInteger(),
                    new BitString("858afc2edd61fcefef3469dd5a004e74a3c727d16498a9408a8e3224").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("dfd0570d4ae5c9f690c757aead04f7e14758fdc4ee05d8f0d089b91a"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964a6a68a65ab05652a9cb3152d9bdf5d3526995afae189667f96e13da17a905ff0b2b9b5bf"),
                // expectedDkm
                new BitString("24b6b6e6d10b2e5482c20ba3b06c"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765123817b69860aa195a9cb405b9554750"),
                // expectedTag
                new BitString("5f5f30f60bb868ca"),
            },
            new object[]
            {
                // label
                "ephem unified P-224 hmac inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("123817b69860aa195a9cb405b9554750"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("d58751997b3a551ccdc6f507bf6ab87e2be7f8267067a455a5815a54").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("c7893ced15c3009b93cb0abeaea2ede308b67fbbd902c6c5d94c24a4").ToPositiveBigInteger(),
                    new BitString("858afc2edd61fcefef3469dd5a004e74a3c727d16498a9408a8e3224").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("e935434303d842605d07112b3b7789ccbe4c6d987db5fa15ea1cdadb").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("784028a2246950401ec81f8e4e03fd3765c4da4d45eba652ed5ba2b5").ToPositiveBigInteger(),
                    new BitString("d071c32634bcf058f072493b6943453a0bd117f5ee5c5866f037f6ab").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("dfd0570d4ae5c9f690c757aead04f7e14758fdc4ee05d8f0d089b91a"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964a6a68a65ab05652a9cb3152d9bdf5d3526995afae189667f96e13da17a905ff0b2b9b5bf"),
                // expectedDkm
                new BitString("24b6b6e6d10b2e5482c20ba3b06c"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765123817b69860aa195a9cb405b9554750"),
                // expectedTag
                new BitString("5f5f30f60bb868ca"),
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "ephem unified P-224 ccm",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                192,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("3d851423f00ce4c41c9d2b41ef878dfd"),
                // aesCcmNonce
                new BitString("dec3f7a2013552"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("4ee19a1e8dafef8d84ad06f9a3176c7105d645fc61f66d5bb74175b2").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("2240e3d5ef61fc9e516f726bd4fd2d18caea337451ad42c07b97dcb6").ToPositiveBigInteger(),
                    new BitString("56700b229e6655d635d9af60923fff9bc21709c3308ec956d7ff35de").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("d8d47bd33e59d933a11f21b3ece887c6dab61bf398df4933b64ce48c").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("9ce739dfcf76b8f1d8472eb8bf7bdf3e60a72913578edfd2e5891a29").ToPositiveBigInteger(),
                    new BitString("4359df09d4435c3c20611e97d22e4a058cf249fa342c1993181bc569").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("ea83ece5641d41ad2486145f76d2f38145955efc6e67443050b7df07"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369648ad6b7b9f10253bf904d3bf3765d08d7247c69b0e49f199f5da00e0e65cff44d16a9d97d"),
                // expectedDkm
                new BitString("3e4f4074018299964ae03f473b87e009aa8a7bbf0eccc6e5"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167653d851423f00ce4c41c9d2b41ef878dfd"),
                // expectedTag
                new BitString("a927c08177ca67141ec1f867e38c"),
            },
            new object[]
            {
                // label
                "ephem unified P-224 ccm inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                192,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("3d851423f00ce4c41c9d2b41ef878dfd"),
                // aesCcmNonce
                new BitString("dec3f7a2013552"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("d8d47bd33e59d933a11f21b3ece887c6dab61bf398df4933b64ce48c").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("9ce739dfcf76b8f1d8472eb8bf7bdf3e60a72913578edfd2e5891a29").ToPositiveBigInteger(),
                    new BitString("4359df09d4435c3c20611e97d22e4a058cf249fa342c1993181bc569").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("4ee19a1e8dafef8d84ad06f9a3176c7105d645fc61f66d5bb74175b2").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("2240e3d5ef61fc9e516f726bd4fd2d18caea337451ad42c07b97dcb6").ToPositiveBigInteger(),
                    new BitString("56700b229e6655d635d9af60923fff9bc21709c3308ec956d7ff35de").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("ea83ece5641d41ad2486145f76d2f38145955efc6e67443050b7df07"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369648ad6b7b9f10253bf904d3bf3765d08d7247c69b0e49f199f5da00e0e65cff44d16a9d97d"),
                // expectedDkm
                new BitString("3e4f4074018299964ae03f473b87e009aa8a7bbf0eccc6e5"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167653d851423f00ce4c41c9d2b41ef878dfd"),
                // expectedTag
                new BitString("a927c08177ca67141ec1f867e38c"),
            },
            #endregion ccm
            #endregion ephemeralunified

            #region onePassUnified
            #region ccm
            new object[]
            {
                // label
                "onePassUnified P-224 ccm",
                // scheme
                EccScheme.OnePassUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("804dee63a8dc67be84c18f04b5682db5"),
                // aesCcmNonce
                new BitString("e86adec6b3b157"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("dd2b49e11e064d50dd309f96cfcb29c373ce6b10641c5c4a035d454e").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("393b10b2cd9398808011467a94d8fef665a79b4a4e99639c21351079").ToPositiveBigInteger(),
                    new BitString("741713cb21fa0ede9c1ecdc6b2f1533a616ebfd5cc0d486f4da7b56d").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("b64d8d876c5404a0a34cfcd9a12373456e055fe8bd309be9248ca72c").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("a6decaf522a909cd4b824e1430b80a46927715c55d8fec445003d93e").ToPositiveBigInteger(),
                    new BitString("b3e9a1aa4572461ff5b3835c15a99c8b4b21550f1fa83f1f356c21fd").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("7c0a6740ff622923ea46e25dd90aa5dbfadeab7ba7733d09b31af07f").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("c6c1bf7ca86b4f304469db504c2612e4f35fc4411620eba9ac649a31").ToPositiveBigInteger(),
                    new BitString("aaed91b55ec9953cabba4bb6a04ac0c9b83dab3119a8634f0cf3f256").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("81101f720aa1993c34ce7c6bd5a984099da4ce0d661313a9adf6bfd7672cc1f3a6ac08981d120c2e1d11e88c373507dfea5990464192db30"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696476430e483c2af502ff4b8c88c823b25003f6e6eccb09abdf681231af16ed313b0890194f"),
                // expectedDkm
                new BitString("8db8e85b6dc01cf1f932b1c7e448bcc7"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765804dee63a8dc67be84c18f04b5682db5"),
                // expectedTag
                new BitString("957cc97172eafb39eb9306843a86"),
            },
            new object[]
            {
                // label
                "onePassUnified P-224 ccm inverse",
                // scheme
                EccScheme.OnePassUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("804dee63a8dc67be84c18f04b5682db5"),
                // aesCcmNonce
                new BitString("e86adec6b3b157"),
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("7c0a6740ff622923ea46e25dd90aa5dbfadeab7ba7733d09b31af07f").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("c6c1bf7ca86b4f304469db504c2612e4f35fc4411620eba9ac649a31").ToPositiveBigInteger(),
                    new BitString("aaed91b55ec9953cabba4bb6a04ac0c9b83dab3119a8634f0cf3f256").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("dd2b49e11e064d50dd309f96cfcb29c373ce6b10641c5c4a035d454e").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("393b10b2cd9398808011467a94d8fef665a79b4a4e99639c21351079").ToPositiveBigInteger(),
                    new BitString("741713cb21fa0ede9c1ecdc6b2f1533a616ebfd5cc0d486f4da7b56d").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("b64d8d876c5404a0a34cfcd9a12373456e055fe8bd309be9248ca72c").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("a6decaf522a909cd4b824e1430b80a46927715c55d8fec445003d93e").ToPositiveBigInteger(),
                    new BitString("b3e9a1aa4572461ff5b3835c15a99c8b4b21550f1fa83f1f356c21fd").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("81101f720aa1993c34ce7c6bd5a984099da4ce0d661313a9adf6bfd7672cc1f3a6ac08981d120c2e1d11e88c373507dfea5990464192db30"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696476430e483c2af502ff4b8c88c823b25003f6e6eccb09abdf681231af16ed313b0890194f"),
                // expectedDkm
                new BitString("8db8e85b6dc01cf1f932b1c7e448bcc7"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765804dee63a8dc67be84c18f04b5682db5"),
                // expectedTag
                new BitString("957cc97172eafb39eb9306843a86"),
            },
            #endregion ccm

            #region hmac
            new object[]
            {
                // label
                "onePassUnified P-224 hmac",
                // scheme
                EccScheme.OnePassUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("8717ded8ebdb064245488b8b21ccf6a6"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("206c5b9a7c482ca92d8a553fc4776832f9c3c72b421f1111030de800").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("9c41bb89986c833559dbba3edfcac5da68933342375d1cc75f9e7c4b").ToPositiveBigInteger(),
                    new BitString("88a6e5eaac2ec1ef5389e7a4f35b8da34e26ea6f87e8b8d045d80e93").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("a9b8015b579483e407f2d91142a81ee738d29b9c81b3ef5db00e511a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("2f1f5b753799175bd49facaeb890654de35999140111378a009bb22c").ToPositiveBigInteger(),
                    new BitString("903444521545e6dc3a190675f3eeb396cbf95872148b03527eb4891d").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("6063bf909899dd91e051b4e207fb897f1ba226eea9e11b2696dfece7").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("6d1d90c87af8d42f3706c765cc6f48bf847ed1ca1580d321568ba4de").ToPositiveBigInteger(),
                    new BitString("9df134d19dcd69263c82f2f7b0a7459c32e106edd7cb017a9e104cfb").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("823c48dc319c46780a1c14b77d18dbf901c40338d8628b50a0c88913f42e25b57aee7211f2ee6fdea24c5da666d9474371fea1d2184b4cd0"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696488ffe310eb6932b8738dcf0b3b30342b2f9c5bbab6ab819195212ab5103e65865194513a"),
                // expectedDkm
                new BitString("ddb3a08a153c2e20ad9bfd93e862"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167658717ded8ebdb064245488b8b21ccf6a6"),
                // expectedTag
                new BitString("f27458ced21439d1"),
            },
            new object[]
            {
                // label
                "onePassUnified P-224 hmac inverse",
                // scheme
                EccScheme.OnePassUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("8717ded8ebdb064245488b8b21ccf6a6"),
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("6063bf909899dd91e051b4e207fb897f1ba226eea9e11b2696dfece7").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("6d1d90c87af8d42f3706c765cc6f48bf847ed1ca1580d321568ba4de").ToPositiveBigInteger(),
                    new BitString("9df134d19dcd69263c82f2f7b0a7459c32e106edd7cb017a9e104cfb").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("206c5b9a7c482ca92d8a553fc4776832f9c3c72b421f1111030de800").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("9c41bb89986c833559dbba3edfcac5da68933342375d1cc75f9e7c4b").ToPositiveBigInteger(),
                    new BitString("88a6e5eaac2ec1ef5389e7a4f35b8da34e26ea6f87e8b8d045d80e93").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("a9b8015b579483e407f2d91142a81ee738d29b9c81b3ef5db00e511a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("2f1f5b753799175bd49facaeb890654de35999140111378a009bb22c").ToPositiveBigInteger(),
                    new BitString("903444521545e6dc3a190675f3eeb396cbf95872148b03527eb4891d").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("823c48dc319c46780a1c14b77d18dbf901c40338d8628b50a0c88913f42e25b57aee7211f2ee6fdea24c5da666d9474371fea1d2184b4cd0"),
                // expectedOi
                new BitString("a1b2c3d4e543415653696488ffe310eb6932b8738dcf0b3b30342b2f9c5bbab6ab819195212ab5103e65865194513a"),
                // expectedDkm
                new BitString("ddb3a08a153c2e20ad9bfd93e862"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167658717ded8ebdb064245488b8b21ccf6a6"),
                // expectedTag
                new BitString("f27458ced21439d1"),
            },
            #endregion hmac
            #endregion onePassUnified
            
            #region onePassMqv
            #region hmac
            new object[]
            {
                // label
                "onePassMqv P-224 hmac",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("62f3fe59c92289ac2a2e163157b59b93"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("fdf1feb01cda935cca067242e357dc0a5a5bf4b914dbb8670f664914").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a954f07bcf4c4303d4ac8816fcfbc24905fcc5e39cead1a3e5ab7d3f").ToPositiveBigInteger(),
                    new BitString("b13966f224cc7259406862621e162875454d98ed819984c1e3d32931").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("f58ee8be4bef57de9902df7604e644eef304c3768b973da2c6a07628").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a419bafb8d27df03f8a025acb8f3f9d2106ae3f513915a6e5c5c8653").ToPositiveBigInteger(),
                    new BitString("30afdf737f87f5c9721c1ccb729d84b7e5a32fbf45e7cbd21dbd74a3").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("da58b80aa06dbe87e766501d4f5818c153930103a6e3ac86b7e9b14a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("dcd2a6c0b73310b90912bfb613c9217dd91078b2789f2412ba43121e").ToPositiveBigInteger(),
                    new BitString("a971971c9d3666e22ce30914ccbaeafae8ca8b6d09625412cbfd7aa0").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("0d3e8be1ac2b5e270828f3c8522f5108fa309c9872da4a0d2ba0b253"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e521336022af7ddeca89b71c8e0628529345c5b1161d54f6af57d13ed71b111ca9a9a5e9e5"),
                // expectedDkm
                new BitString("e86d0bc928ee8328317d25c30404"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676562f3fe59c92289ac2a2e163157b59b93"),
                // expectedTag
                new BitString("579a8c9df7eb7df7"),
            },
            new object[]
            {
                // label
                "onePassMqv P-224 hmac inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("62f3fe59c92289ac2a2e163157b59b93"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("f58ee8be4bef57de9902df7604e644eef304c3768b973da2c6a07628").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a419bafb8d27df03f8a025acb8f3f9d2106ae3f513915a6e5c5c8653").ToPositiveBigInteger(),
                    new BitString("30afdf737f87f5c9721c1ccb729d84b7e5a32fbf45e7cbd21dbd74a3").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("da58b80aa06dbe87e766501d4f5818c153930103a6e3ac86b7e9b14a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("dcd2a6c0b73310b90912bfb613c9217dd91078b2789f2412ba43121e").ToPositiveBigInteger(),
                    new BitString("a971971c9d3666e22ce30914ccbaeafae8ca8b6d09625412cbfd7aa0").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("fdf1feb01cda935cca067242e357dc0a5a5bf4b914dbb8670f664914").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a954f07bcf4c4303d4ac8816fcfbc24905fcc5e39cead1a3e5ab7d3f").ToPositiveBigInteger(),
                    new BitString("b13966f224cc7259406862621e162875454d98ed819984c1e3d32931").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("0d3e8be1ac2b5e270828f3c8522f5108fa309c9872da4a0d2ba0b253"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e521336022af7ddeca89b71c8e0628529345c5b1161d54f6af57d13ed71b111ca9a9a5e9e5"),
                // expectedDkm
                new BitString("e86d0bc928ee8328317d25c30404"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676562f3fe59c92289ac2a2e163157b59b93"),
                // expectedTag
                new BitString("579a8c9df7eb7df7"),
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "onePassMqv P-224 ccm",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("ee8953da84f6c1a909d22ea0a7afb00e"),
                // aesCcmNonce
                new BitString("186ce451fae07e"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("7ab8b524fde9a252e2ec9a3c8636dfc8a88ae4da9e9217da06bd9828").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("037b7e9bd1a1f3fea4fd7bc61b4a9de59c0800f6dd2853021519eb42").ToPositiveBigInteger(),
                    new BitString("de8ac642772ef8f85cccc5bfe9056bc2b1cc0dd3f9859716633340fd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("7f90aea5ad562af710b22be47f72e08f69360e285de9798eaf5d51cb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("96efadc75bf3091b8d74de7a415fd3b3e4ef0a6d59485ba6d35b2b6c").ToPositiveBigInteger(),
                    new BitString("90ab198b4e8d4861fbf7d06f30890f1d754645aa5fce67e5798a4b13").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("4c2fd3e8c11e41c27d9641f5715b6d12e4af3f27208b40e0fcda7b93").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("053f9177098d02f86fa40b2426906a5246cb49cfcc39fe05ed4b9744").ToPositiveBigInteger(),
                    new BitString("cba4acf518d15743eeec4fcd32e1e421d5137dcd361ca38f19eb4e29").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("37258088597e2d33628acd795d6d80358c87ae30ed0b8076f31ca2bd"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646cf0ab95b430de5f6a12723a5cf6d525988696c83a3fd513822df9232af5a165f619dcfd"),
                // expectedDkm
                new BitString("08349e46bf880dab70cc14901186248d"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765ee8953da84f6c1a909d22ea0a7afb00e"),
                // expectedTag
                new BitString("a62579ad4b73caf3bd6ce3ec2e12"),
            },
            new object[]
            {
                // label
                "onePassMqv P-224 ccm inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("ee8953da84f6c1a909d22ea0a7afb00e"),
                // aesCcmNonce
                new BitString("186ce451fae07e"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("4c2fd3e8c11e41c27d9641f5715b6d12e4af3f27208b40e0fcda7b93").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("053f9177098d02f86fa40b2426906a5246cb49cfcc39fe05ed4b9744").ToPositiveBigInteger(),
                    new BitString("cba4acf518d15743eeec4fcd32e1e421d5137dcd361ca38f19eb4e29").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("7ab8b524fde9a252e2ec9a3c8636dfc8a88ae4da9e9217da06bd9828").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("037b7e9bd1a1f3fea4fd7bc61b4a9de59c0800f6dd2853021519eb42").ToPositiveBigInteger(),
                    new BitString("de8ac642772ef8f85cccc5bfe9056bc2b1cc0dd3f9859716633340fd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("7f90aea5ad562af710b22be47f72e08f69360e285de9798eaf5d51cb").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("96efadc75bf3091b8d74de7a415fd3b3e4ef0a6d59485ba6d35b2b6c").ToPositiveBigInteger(),
                    new BitString("90ab198b4e8d4861fbf7d06f30890f1d754645aa5fce67e5798a4b13").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("37258088597e2d33628acd795d6d80358c87ae30ed0b8076f31ca2bd"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646cf0ab95b430de5f6a12723a5cf6d525988696c83a3fd513822df9232af5a165f619dcfd"),
                // expectedDkm
                new BitString("08349e46bf880dab70cc14901186248d"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765ee8953da84f6c1a909d22ea0a7afb00e"),
                // expectedTag
                new BitString("a62579ad4b73caf3bd6ce3ec2e12"),
            },
            #endregion ccm
            #endregion onePassMqv
            
            #region onePassDh
            #region ccm
            new object[]
            {
                // label
                "onePassDh P-224 ccm",
                // scheme
                EccScheme.OnePassDh,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("7cf47857e8588e8e13a5779bd6e179b9"),
                // aesCcmNonce
                new BitString("825e1117db7d7c"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("714dbf63b95608e62e0afd6d8385a5f7496c4dd85565b38ec2ea74c4").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("6c269346c7fff2c1515ea366fd00886ba8e101b0d64d34ed69eb8ab0").ToPositiveBigInteger(),
                    new BitString("801b590957772c8d95259efe449eecfcdf870828b43c9593dde2f5d8").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("684122be269980ce9e6fa7f0fe8df9d07d10459ae9b1924fffcb427a").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("fa14225a7c66e8a2d2abe63420053f86d2e49bec645e981625f280bd").ToPositiveBigInteger(),
                    new BitString("ed4b0f6965a5056c02cb4da3e306839cda7f4abe42e85f3f8a03acb4").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("6514d302c91792c3626c99c76e38f37ce1eea7b7cee70866c0ba1b54"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964408b6b338d9d882526c767b1417a83fdbb2d4757feb0162528bd8e1a6ad10eb0ad3975d2"),
                // expectedDkm
                new BitString("48f0f09c04b08acac33bfbbe4d4f16cc"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167657cf47857e8588e8e13a5779bd6e179b9"),
                // expectedTag
                new BitString("6cbca4f20c6bfe819294d5731816"),
            },
            new object[]
            {
                // label
                "onePassDh P-224 ccm",
                // scheme
                EccScheme.OnePassDh,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("7cf47857e8588e8e13a5779bd6e179b9"),
                // aesCcmNonce
                new BitString("825e1117db7d7c"),
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("684122be269980ce9e6fa7f0fe8df9d07d10459ae9b1924fffcb427a").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("fa14225a7c66e8a2d2abe63420053f86d2e49bec645e981625f280bd").ToPositiveBigInteger(),
                    new BitString("ed4b0f6965a5056c02cb4da3e306839cda7f4abe42e85f3f8a03acb4").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("714dbf63b95608e62e0afd6d8385a5f7496c4dd85565b38ec2ea74c4").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("6c269346c7fff2c1515ea366fd00886ba8e101b0d64d34ed69eb8ab0").ToPositiveBigInteger(),
                    new BitString("801b590957772c8d95259efe449eecfcdf870828b43c9593dde2f5d8").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("6514d302c91792c3626c99c76e38f37ce1eea7b7cee70866c0ba1b54"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964408b6b338d9d882526c767b1417a83fdbb2d4757feb0162528bd8e1a6ad10eb0ad3975d2"),
                // expectedDkm
                new BitString("48f0f09c04b08acac33bfbbe4d4f16cc"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167657cf47857e8588e8e13a5779bd6e179b9"),
                // expectedTag
                new BitString("6cbca4f20c6bfe819294d5731816"),
            },
            #endregion ccm

            #region hmac
            new object[]
            {
                // label
                "onePassDh P-224 hmac",
                // scheme
                EccScheme.OnePassDh,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("78970a6f7287dd4f0a54f7b4ff4dc373"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("cce095bf561b8e7b8da1d36232fbdc5172f5c62405b450cb3626db56").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("4f563693e0d6cf099cb0e947d757a9a73f798f78a7b449d483fb4fcd").ToPositiveBigInteger(),
                    new BitString("27f0fa2e2275ee4e00caebdb629d33bdab4dea63c97fe09aaab110c5").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("3ce85fda6d591bc2beb4be40b781966b1f9c9e8480f660af8dbda415").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("19fc9e47d02260b232284ddc0c06b77ba64f254ada993b25416ea55e").ToPositiveBigInteger(),
                    new BitString("3a66d17eb6af82f45ac104f5af97921541b64fb69f5dd76696476d00").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("1f6ac82f52e8691233f2000bc786402f4f94c1eb2fdc0b126eb63af7"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964c000ead9f12c63b4d17befeae36ed63642ca9c983fa6ca054cc2f25e191aed9ffcbb6759"),
                // expectedDkm
                new BitString("c23e336a9fd8f53855267ab88129"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676578970a6f7287dd4f0a54f7b4ff4dc373"),
                // expectedTag
                new BitString("201eb77c103b72bc440d1af01249"),
            },
            new object[]
            {
                // label
                "onePassDh P-224 hmac inverse",
                // scheme
                EccScheme.OnePassDh,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("78970a6f7287dd4f0a54f7b4ff4dc373"),
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("3ce85fda6d591bc2beb4be40b781966b1f9c9e8480f660af8dbda415").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("19fc9e47d02260b232284ddc0c06b77ba64f254ada993b25416ea55e").ToPositiveBigInteger(),
                    new BitString("3a66d17eb6af82f45ac104f5af97921541b64fb69f5dd76696476d00").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("cce095bf561b8e7b8da1d36232fbdc5172f5c62405b450cb3626db56").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("4f563693e0d6cf099cb0e947d757a9a73f798f78a7b449d483fb4fcd").ToPositiveBigInteger(),
                    new BitString("27f0fa2e2275ee4e00caebdb629d33bdab4dea63c97fe09aaab110c5").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,

                // expectedZ
                new BitString("1f6ac82f52e8691233f2000bc786402f4f94c1eb2fdc0b126eb63af7"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964c000ead9f12c63b4d17befeae36ed63642ca9c983fa6ca054cc2f25e191aed9ffcbb6759"),
                // expectedDkm
                new BitString("c23e336a9fd8f53855267ab88129"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676578970a6f7287dd4f0a54f7b4ff4dc373"),
                // expectedTag
                new BitString("201eb77c103b72bc440d1af01249"),
            },
            #endregion hmac
            #endregion onePassDh

            #region StaticUnified
            #region hmac
            new object[]
            {
                // label
                "StaticUnified P-224 hmac",
                // scheme
                EccScheme.StaticUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("5ac86fdbe298025324111cb4367d2fb5"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("0aaf95da34a07b4e0e4b146be3234770cfeb6e82e3eac764dea01ff1").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9da9d9b868bc66dc42b5c1a574036575efc4ff50136d655a79dd9f9").ToPositiveBigInteger(),
                    new BitString("3e2b5c4b3bd22f56fa7dde4c955456868bd659164e1d5a36ea6419ab").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                new BitString("dd51b8eff60bbf79b9e11b0b5363"),
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("38bd268409377242ccccf0da07d78c926fa90f19668f3a254d49c484").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("e9ae73e20729b94468686115c87e550a5b80bea570de3380eaca24f7").ToPositiveBigInteger(),
                    new BitString("6ef16e93abbb28fc0f1ebc720642fabc904d1059825dea220b2fbe8e").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // expectedZ
                new BitString("0b29d163b4c37de794698d6b19d0643ee261e364c5dbc1119329d462"),
                // expectedOi
                new BitString("a1b2c3d4e5dd51b8eff60bbf79b9e11b0b5363434156536964875086e2c370f08c52627a0bc191b637daaf5f71c94a"),
                // expectedDkm
                new BitString("3d9415aabd46d8b805e043da461b"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167655ac86fdbe298025324111cb4367d2fb5"),
                // expectedTag
                new BitString("d64a14da54bdabd8"),
            },
            new object[]
            {
                // label
                "StaticUnified P-224 hmac inverse",
                // scheme
                EccScheme.StaticUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("5ac86fdbe298025324111cb4367d2fb5"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("38bd268409377242ccccf0da07d78c926fa90f19668f3a254d49c484").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("e9ae73e20729b94468686115c87e550a5b80bea570de3380eaca24f7").ToPositiveBigInteger(),
                    new BitString("6ef16e93abbb28fc0f1ebc720642fabc904d1059825dea220b2fbe8e").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("0aaf95da34a07b4e0e4b146be3234770cfeb6e82e3eac764dea01ff1").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9da9d9b868bc66dc42b5c1a574036575efc4ff50136d655a79dd9f9").ToPositiveBigInteger(),
                    new BitString("3e2b5c4b3bd22f56fa7dde4c955456868bd659164e1d5a36ea6419ab").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                new BitString("dd51b8eff60bbf79b9e11b0b5363"),
                // expectedZ
                new BitString("0b29d163b4c37de794698d6b19d0643ee261e364c5dbc1119329d462"),
                // expectedOi
                new BitString("a1b2c3d4e5dd51b8eff60bbf79b9e11b0b5363434156536964875086e2c370f08c52627a0bc191b637daaf5f71c94a"),
                // expectedDkm
                new BitString("3d9415aabd46d8b805e043da461b"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167655ac86fdbe298025324111cb4367d2fb5"),
                // expectedTag
                new BitString("d64a14da54bdabd8"),
            },
            #endregion hmac
            #endregion StaticUnified
        };

        [Test]
        [TestCaseSource(nameof(_test_NoKeyConfirmation))]
        public void ShouldNoKeyConfirmationCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            int keyLength,
            int tagLength,
            BitString noKeyConfirmationNonce,
            BitString aesCcmNonce,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString thisPartyDkmNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherpartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString otherPartyDkmNonce,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var otherPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new EccKeyPair(otherpartyPublicStaticKey),
                    new EccKeyPair(otherPartyPublicEphemKey),
                    otherPartyDkmNonce,
                    null,
                    // when "party v" noKeyConfirmationNonce is provided as a part of party U's shared information
                    keyAgreementRole == KeyAgreementRole.ResponderPartyV ? noKeyConfirmationNonce : null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    (thisPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    (otherPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // add dkm nonce to entropy provided when needed
            if (thisPartyDkmNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyDkmNonce);
            }

            // MAC no key confirmation data makes use of a nonce
            _entropyProviderScheme.AddEntropy(noKeyConfirmationNonce);

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new NativeShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithPartyId(thisPartyId)
                .BuildKdfNoKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(otherPartySharedInformation);

            Assert.That(result.Success, nameof(result.Success));
            Assert.That(result.Z, Is.EqualTo(expectedZ), nameof(result.Z));
            Assert.That(result.Oi, Is.EqualTo(expectedOi), nameof(result.Oi));
            Assert.That(result.Dkm, Is.EqualTo(expectedDkm), nameof(result.Dkm));
            Assert.That(result.MacData, Is.EqualTo(expectedMacData), nameof(result.MacData));
            Assert.That(result.Tag, Is.EqualTo(expectedTag), nameof(result.Tag));
        }

        private static object[] _test_keyConfirmation = new object[]
        {
            #region full unified
            new object[]
            {
                // label
                "fullUnified P224 hmac",
                // scheme
                EccScheme.FullUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("bf64447965afda01a63bdf4e5896a35bb2f2959f1d6affcaacbcec84").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("efc7a590b38e5eddb6d261c4e980d2789d1f50559d011d79344a71e9").ToPositiveBigInteger(),
                    new BitString("8b28ee9d31e28ef6ee4fa4c9ea0d68e1f37891275ea6a55931ce2522").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("413b6d21051c941f6e2356dd0ddf7b531610ebbf24714e9e5057bb74").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("79ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c6").ToPositiveBigInteger(),
                    new BitString("87925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("40131f44557303c9d1a6ac42514d829499e6b863e3a723efd22c2528").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("e868b8ede95e998f4f1b890d3ee9b546aea6b7f3da88caf20fb43d42").ToPositiveBigInteger(),
                    new BitString("2c3151ab1a057d4ac5631a876208376c924b76e76b57f88c0eb827ca").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("4f2f5ffa67781fa04cdfcd308b773cb13d12c3369fa68d6f648bca2a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c").ToPositiveBigInteger(),
                    new BitString("25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("046c09fd63d2f2a3910190fef505af63deb819e6db948416fcbf89304956e2a9f395878f963c18bc197fbef349392f5fd5bf4731864741b4"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369645f1ec9fdf3d0b09a719ac253b5bcd00932ae058b86611aff51c8ca8448978615854b69b0"),
                // expectedDkm
                new BitString("9ce9dfc61f69d864b6b1133cb01b"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e543415653696479ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c687925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e"),
                // expectedTag
                new BitString("b8e68a044f36f699")
            },
            new object[]
            {
                // label
                "fullUnified P224 hmac inverse",
                // scheme
                EccScheme.FullUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("40131f44557303c9d1a6ac42514d829499e6b863e3a723efd22c2528").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("e868b8ede95e998f4f1b890d3ee9b546aea6b7f3da88caf20fb43d42").ToPositiveBigInteger(),
                    new BitString("2c3151ab1a057d4ac5631a876208376c924b76e76b57f88c0eb827ca").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("4f2f5ffa67781fa04cdfcd308b773cb13d12c3369fa68d6f648bca2a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c").ToPositiveBigInteger(),
                    new BitString("25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("bf64447965afda01a63bdf4e5896a35bb2f2959f1d6affcaacbcec84").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("efc7a590b38e5eddb6d261c4e980d2789d1f50559d011d79344a71e9").ToPositiveBigInteger(),
                    new BitString("8b28ee9d31e28ef6ee4fa4c9ea0d68e1f37891275ea6a55931ce2522").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("413b6d21051c941f6e2356dd0ddf7b531610ebbf24714e9e5057bb74").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("79ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c6").ToPositiveBigInteger(),
                    new BitString("87925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("046c09fd63d2f2a3910190fef505af63deb819e6db948416fcbf89304956e2a9f395878f963c18bc197fbef349392f5fd5bf4731864741b4"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369645f1ec9fdf3d0b09a719ac253b5bcd00932ae058b86611aff51c8ca8448978615854b69b0"),
                // expectedDkm
                new BitString("9ce9dfc61f69d864b6b1133cb01b"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e543415653696479ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c687925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e"),
                // expectedTag
                new BitString("b8e68a044f36f699")
            },
            #endregion full unified

            #region fullMqv
            new object[]
            {
                // label
                "fullMqv P224 hmac",
                // scheme
                EccScheme.FullMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a13226f2560825d9ccc656e27d6091d2c036f26abc3b57dc47478c9a").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59caf428dea1b059f5b580c7adc246b056b99dab5c9fe012b6ec9257").ToPositiveBigInteger(),
                    new BitString("20d77996dc489ce0e557eeccb9ee3d55630c4538f8da0b6ead7deb8c").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("302c1c8a6a6b0df87445dadcbc99be525d970d351a189d99414416d5").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0").ToPositiveBigInteger(),
                    new BitString("cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("a105eae1bb706d6bfee644f64e064d532740bc3f681056ca4c029296").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("c5d03ee658dbfb5f8787c00819aa44855cb3d47a6ddda74be7586704").ToPositiveBigInteger(),
                    new BitString("cdf7ffd06d781f68e3290ef13ff0fdea07a30a327af2419c2875bef1").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("c6ef9d41f90e1bffe71124720b782d85aa438fe423c4603a59130260").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("55dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879").ToPositiveBigInteger(),
                    new BitString("f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("fe533ed3ca082bf4abe6ca3775c21192381743ff7c94f27088f526ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964d5385cb451bf18d14d128981704c2ddebf71c82bbe02aa09d8cf30ee010d8dd49aa8ca2e"),
                // expectedDkm
                new BitString("adca617377ecad9ea33e244f9110"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e555dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42"),
                // expectedTag
                new BitString("d62c008cc35958da")
            },
            new object[]
            {
                // label
                "fullMqv P224 hmac inverse",
                // scheme
                EccScheme.FullMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("a105eae1bb706d6bfee644f64e064d532740bc3f681056ca4c029296").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("c5d03ee658dbfb5f8787c00819aa44855cb3d47a6ddda74be7586704").ToPositiveBigInteger(),
                    new BitString("cdf7ffd06d781f68e3290ef13ff0fdea07a30a327af2419c2875bef1").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("c6ef9d41f90e1bffe71124720b782d85aa438fe423c4603a59130260").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("55dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879").ToPositiveBigInteger(),
                    new BitString("f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a13226f2560825d9ccc656e27d6091d2c036f26abc3b57dc47478c9a").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59caf428dea1b059f5b580c7adc246b056b99dab5c9fe012b6ec9257").ToPositiveBigInteger(),
                    new BitString("20d77996dc489ce0e557eeccb9ee3d55630c4538f8da0b6ead7deb8c").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("302c1c8a6a6b0df87445dadcbc99be525d970d351a189d99414416d5").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0").ToPositiveBigInteger(),
                    new BitString("cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("fe533ed3ca082bf4abe6ca3775c21192381743ff7c94f27088f526ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964d5385cb451bf18d14d128981704c2ddebf71c82bbe02aa09d8cf30ee010d8dd49aa8ca2e"),
                // expectedDkm
                new BitString("adca617377ecad9ea33e244f9110"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e555dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42"),
                // expectedTag
                new BitString("d62c008cc35958da")
            },
            #endregion fullMqv

            #region onePassUnified
            new object[]
            {
                // label
                "onePassUnified P224 hmac",
                // scheme
                EccScheme.OnePassUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("d516aa59124b22a426a1564dcc9120a7b2983e6cd509b726ae0dca9f").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("28007e7a035fffcdd9793a48f99f7ebb201726a51add6967eb0a0380").ToPositiveBigInteger(),
                    new BitString("bbbe9e522cd8a6163b4a03b546838a81ddb8ce7d0847017f07e8f35d").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("f59096f7cec93b1442cffb9a27ef849fd43f7019efeaec71a6520df4").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("4ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33").ToPositiveBigInteger(),
                    new BitString("c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("b445eef72bc8c3e7d09af4ff6dde76a02317dae149d32b252dc714eb").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d3bb8302551df53d82e78af4eef154df5b8363ae633fa4795c66dc86").ToPositiveBigInteger(),
                    new BitString("dac05c91159e657cbf3c9119f9189876e00a44e5a67217a023ab00fe").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("").ToPositiveBigInteger(),
                    new BitString("").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedZ
                new BitString("5a2b672094f591810be90ebf0c061faa0e630faabc900b118744bb7694f345eb97b2593b6eafef4c29102c48928073a843370b1d0c6b309d"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964b6bf2797bf8d45e51be53669d048c7c7bea47d8525cb341d6824065011a06ebe14f0adb4"),
                // expectedDkm
                new BitString("033da1261b5def0b27b1964d8bc3"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e54341565369644ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedTag
                new BitString("b6f41de5d2cd1b58")
            },
            new object[]
            {
                // label
                "onePassUnified P224 hmac inverse",
                // scheme
                EccScheme.OnePassUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("b445eef72bc8c3e7d09af4ff6dde76a02317dae149d32b252dc714eb").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d3bb8302551df53d82e78af4eef154df5b8363ae633fa4795c66dc86").ToPositiveBigInteger(),
                    new BitString("dac05c91159e657cbf3c9119f9189876e00a44e5a67217a023ab00fe").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("").ToPositiveBigInteger(),
                    new BitString("").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("d516aa59124b22a426a1564dcc9120a7b2983e6cd509b726ae0dca9f").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("28007e7a035fffcdd9793a48f99f7ebb201726a51add6967eb0a0380").ToPositiveBigInteger(),
                    new BitString("bbbe9e522cd8a6163b4a03b546838a81ddb8ce7d0847017f07e8f35d").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("f59096f7cec93b1442cffb9a27ef849fd43f7019efeaec71a6520df4").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("4ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33").ToPositiveBigInteger(),
                    new BitString("c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("5a2b672094f591810be90ebf0c061faa0e630faabc900b118744bb7694f345eb97b2593b6eafef4c29102c48928073a843370b1d0c6b309d"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964b6bf2797bf8d45e51be53669d048c7c7bea47d8525cb341d6824065011a06ebe14f0adb4"),
                // expectedDkm
                new BitString("033da1261b5def0b27b1964d8bc3"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e54341565369644ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedTag
                new BitString("b6f41de5d2cd1b58")
            },
            #endregion onePassUnified

            #region onePassMqv
            #region hmac
            new object[]
            {
                // label
                "onePassMqv P224 hmac",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("71b94b0b139630a558fec67d8525e1e71fda06d8b5171ead65dd9079").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9049254bea3b30830bf808ef63290049d1e1bf2627e11add742c6e7").ToPositiveBigInteger(),
                    new BitString("71ab72bcb8de04453b4e1ddc5984d7b79759d193ed7e54c13eaf9eed").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("a94036779eca96197f9c9bc601fd9614631d284ed4f32dc6f5f46de9").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("00e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b2087").ToPositiveBigInteger(),
                    new BitString("71f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecf").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("8b6606df3fae46702c192a54d37839be34b78dee0f00c6b5ea192cbb").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("5ffc623fa5d9c0f2839a0b68ae1d2cf737d53fb9b3e015269f1c59f2").ToPositiveBigInteger(),
                    new BitString("543e22fc09775e36d4d9d0865680a4ecfc843ff1f545a4b63ad252e4").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("f7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedZ
                new BitString("5cb7d19d782b3bc5ebf88374507afbfa52d61f6ccd3d25d048af3d0f"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964670d390c7d9a654abb3dac2f24f9ce052a8b6591d0fe26460c7a64b96f5dd555078c9950"),
                // expectedDkm
                new BitString("526ee2c03cbb91c42ec40441c182"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696400e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b208771f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecff7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedTag
                new BitString("89fdcffc87f5fc57")
            },
            new object[]
            {
                // label
                "onePassMqv P224 hmac inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("8b6606df3fae46702c192a54d37839be34b78dee0f00c6b5ea192cbb").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("5ffc623fa5d9c0f2839a0b68ae1d2cf737d53fb9b3e015269f1c59f2").ToPositiveBigInteger(),
                    new BitString("543e22fc09775e36d4d9d0865680a4ecfc843ff1f545a4b63ad252e4").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                new BitString("f7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("71b94b0b139630a558fec67d8525e1e71fda06d8b5171ead65dd9079").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9049254bea3b30830bf808ef63290049d1e1bf2627e11add742c6e7").ToPositiveBigInteger(),
                    new BitString("71ab72bcb8de04453b4e1ddc5984d7b79759d193ed7e54c13eaf9eed").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("a94036779eca96197f9c9bc601fd9614631d284ed4f32dc6f5f46de9").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("00e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b2087").ToPositiveBigInteger(),
                    new BitString("71f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecf").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("5cb7d19d782b3bc5ebf88374507afbfa52d61f6ccd3d25d048af3d0f"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964670d390c7d9a654abb3dac2f24f9ce052a8b6591d0fe26460c7a64b96f5dd555078c9950"),
                // expectedDkm
                new BitString("526ee2c03cbb91c42ec40441c182"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696400e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b208771f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecff7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedTag
                new BitString("89fdcffc87f5fc57")
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "onePassMqv P224 ccm",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm the weird scenario",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm the weird scenario inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            #endregion ccm
            #endregion onePassMqv

            #region onePassDh
            new object[]
            {
                // label
                "onePassDh P224 hmac",
                // scheme
                EccScheme.OnePassDh,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("0fcc94775a976e3f39e45e733ccfe5d6ca27a86f51688ceeb4cae803").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("99c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f").ToPositiveBigInteger(),
                    new BitString("8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("1d73f478d76322bf30410392f561e49b8dd4420aad0076a0e2c23d4f").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("405a354518867d4441719ec73fc8456bf9d7a5e1c18a5f16b02f84e2").ToPositiveBigInteger(),
                    new BitString("1d609ef8513ec4bdeddc281ab7aff0969397bc05e9e7e27952bce6bf").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("1fd40be3414f0981766848fef8a4fff57864b378b1ed7704b97be463"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646f5791408f3d4c049f3a15aed63488746236d2e66568cd7e3d3f6c7808f15ae3672cdef9"),
                // expectedDkm
                new BitString("6817769ab5fe5875250446959a33"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e599c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8"),
                // expectedTag
                new BitString("87ea941c03cc5e8e")
            },
            new object[]
            {
                // label
                "onePassDh P224 hmac inverse",
                // scheme
                EccScheme.OnePassDh,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("1d73f478d76322bf30410392f561e49b8dd4420aad0076a0e2c23d4f").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("405a354518867d4441719ec73fc8456bf9d7a5e1c18a5f16b02f84e2").ToPositiveBigInteger(),
                    new BitString("1d609ef8513ec4bdeddc281ab7aff0969397bc05e9e7e27952bce6bf").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("0fcc94775a976e3f39e45e733ccfe5d6ca27a86f51688ceeb4cae803").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("99c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f").ToPositiveBigInteger(),
                    new BitString("8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("1fd40be3414f0981766848fef8a4fff57864b378b1ed7704b97be463"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646f5791408f3d4c049f3a15aed63488746236d2e66568cd7e3d3f6c7808f15ae3672cdef9"),
                // expectedDkm
                new BitString("6817769ab5fe5875250446959a33"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e599c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8"),
                // expectedTag
                new BitString("87ea941c03cc5e8e")
            },
            #endregion onePassDh

            #region StaticUnified
            #region hmac
            new object[]
            {
                // label
                "StaticUnified P224 hmac",
                // scheme
                EccScheme.StaticUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("b8706f099ef246aa5fcdaa2062d8820b8f2d0749fe6fd076939e4ca2").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("8eb82fd84b257cbe16bebb643d590e65198e82cdab32a5f2196edc45").ToPositiveBigInteger(),
                    new BitString("df3c23bd8c61aaf6a983fc31ceaf38c3ddeb5614338c289c3865f66e").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                new BitString("34e74cd386476a01a91a80533919"),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("baa3488d9499c4409e57a7f9efdc7b23eee14c6719e04ffda48e3729").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("b8a97d2a2d8ce6c58537afc41027cf9863aac3cf225533e5eba90e37").ToPositiveBigInteger(),
                    new BitString("e5ff363350ba18ea7b4b3d178e7f9b967b8440e3b82b01b30e83dc3f").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedZ
                new BitString("a997f2f38776f1e59dc47a2f3865fad252bcf0954138c3e8ff59ba6f"),
                // expectedOi
                new BitString("a1b2c3d4e534e74cd386476a01a91a80533919434156536964e204a7177157f793f51806ad1a9beee2eefe63ebde51"),
                // expectedDkm
                new BitString("7d740e1d93e4fc2b53df5038c930"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696434e74cd386476a01a91a80533919836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedTag
                new BitString("0d49be39bb9ee470")
            },
            new object[]
            {
                // label
                "StaticUnified P224 hmac inverse",
                // scheme
                EccScheme.StaticUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("baa3488d9499c4409e57a7f9efdc7b23eee14c6719e04ffda48e3729").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("b8a97d2a2d8ce6c58537afc41027cf9863aac3cf225533e5eba90e37").ToPositiveBigInteger(),
                    new BitString("e5ff363350ba18ea7b4b3d178e7f9b967b8440e3b82b01b30e83dc3f").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("b8706f099ef246aa5fcdaa2062d8820b8f2d0749fe6fd076939e4ca2").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("8eb82fd84b257cbe16bebb643d590e65198e82cdab32a5f2196edc45").ToPositiveBigInteger(),
                    new BitString("df3c23bd8c61aaf6a983fc31ceaf38c3ddeb5614338c289c3865f66e").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                new BitString("34e74cd386476a01a91a80533919"),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("a997f2f38776f1e59dc47a2f3865fad252bcf0954138c3e8ff59ba6f"),
                // expectedOi
                new BitString("a1b2c3d4e534e74cd386476a01a91a80533919434156536964e204a7177157f793f51806ad1a9beee2eefe63ebde51"),
                // expectedDkm
                new BitString("7d740e1d93e4fc2b53df5038c930"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696434e74cd386476a01a91a80533919836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedTag
                new BitString("0d49be39bb9ee470")
            },
            #endregion hmac

            #region ccm
            new object[]
            {
                // label
                "StaticUnified P224 ccm",
                // scheme
                EccScheme.StaticUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("74c5b2ffc98670024d99a36a51"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("0747edc603288e86cc03ba184138af5a13fbdfc400013a42936a0321").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("1e1c2b7952184c197684bba87f4e5128e5b869b17c155016dcaa31e3").ToPositiveBigInteger(),
                    new BitString("4e12b6f0fe7842fc19d0f19707edac9339b48721718053ec3a5c6edc").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                new BitString("b256233d5ee114f6cd00b073a5d2"),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("d38c193ac27d316a6787d6b34fbeaaecb41d6f5169b8e46fb3a4679e").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("e6ca16d9a54202b89071df52b325b883ccff4c520a9988bca86c9dcd").ToPositiveBigInteger(),
                    new BitString("d248676560c2120cbd9165f2504eb76629a8c31b5c8de2c590502f77").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("40bc7e4d90e30406c36d3c1c80779bc479f90ccc935735d14732fbb24884484466ec0b48b246819ae37fb4abf8a741301f485d1cd6f71acc"),
                // expectedZ
                new BitString("b0ba5f60deb5fc881482cf909a1f9b41d680324afc11d59e327fe35f"),
                // expectedOi
                new BitString("a1b2c3d4e5b256233d5ee114f6cd00b073a5d243415653696482f93f1b6cfb3973d5ae963541969e1ec877d52dbf68"),
                // expectedDkm
                new BitString("fb7d879de16afa55885b6ae35e0f1733"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e5434156536964b256233d5ee114f6cd00b073a5d240bc7e4d90e30406c36d3c1c80779bc479f90ccc935735d14732fbb24884484466ec0b48b246819ae37fb4abf8a741301f485d1cd6f71acc"),
                // expectedTag
                new BitString("4adae4db55fd6cd342705c97586f9988")
            },
            new object[]
            {
                // label
                "StaticUnified P224 ccm inverse",
                // scheme
                EccScheme.StaticUnified,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("74c5b2ffc98670024d99a36a51"),
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("d38c193ac27d316a6787d6b34fbeaaecb41d6f5169b8e46fb3a4679e").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("e6ca16d9a54202b89071df52b325b883ccff4c520a9988bca86c9dcd").ToPositiveBigInteger(),
                    new BitString("d248676560c2120cbd9165f2504eb76629a8c31b5c8de2c590502f77").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("40bc7e4d90e30406c36d3c1c80779bc479f90ccc935735d14732fbb24884484466ec0b48b246819ae37fb4abf8a741301f485d1cd6f71acc"),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("0747edc603288e86cc03ba184138af5a13fbdfc400013a42936a0321").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("1e1c2b7952184c197684bba87f4e5128e5b869b17c155016dcaa31e3").ToPositiveBigInteger(),
                    new BitString("4e12b6f0fe7842fc19d0f19707edac9339b48721718053ec3a5c6edc").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                new BitString("b256233d5ee114f6cd00b073a5d2"),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("b0ba5f60deb5fc881482cf909a1f9b41d680324afc11d59e327fe35f"),
                // expectedOi
                new BitString("a1b2c3d4e5b256233d5ee114f6cd00b073a5d243415653696482f93f1b6cfb3973d5ae963541969e1ec877d52dbf68"),
                // expectedDkm
                new BitString("fb7d879de16afa55885b6ae35e0f1733"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e5434156536964b256233d5ee114f6cd00b073a5d240bc7e4d90e30406c36d3c1c80779bc479f90ccc935735d14732fbb24884484466ec0b48b246819ae37fb4abf8a741301f485d1cd6f71acc"),
                // expectedTag
                new BitString("4adae4db55fd6cd342705c97586f9988")
            },
            #endregion ccm
            #endregion StaticUnified
        };

        [Test]
        [TestCaseSource(nameof(_test_keyConfirmation))]
        public void ShouldKeyConfirmationCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            int keyLength,
            int tagLength,
            BitString aesCcmNonce,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString thisPartyDkmNonce,
            BitString thisPartyEphemeralNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString otherPartyDkmNonce,
            BitString otherPartyEphemeralNonce,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var otherPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new EccKeyPair(otherPartyPublicStaticKey),
                    new EccKeyPair(otherPartyPublicEphemKey),
                    otherPartyDkmNonce,
                    otherPartyEphemeralNonce,
                    null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    (thisPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    (otherPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // add dkm nonce to entropy provided when needed
            if (thisPartyDkmNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyDkmNonce);
            }

            // MAC no key confirmation data makes use of a nonce
            if (thisPartyEphemeralNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyEphemeralNonce);
            }

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new NativeShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithPartyId(thisPartyId)
                .BuildKdfKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .WithKeyConfirmationRole(keyConfirmationRole)
                .WithKeyConfirmationDirection(keyConfirmationDirection)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(otherPartySharedInformation);

            Assert.That(result.Success, nameof(result.Success));
            Assert.That(result.Z.ToHex(), Is.EqualTo(expectedZ.ToHex()), nameof(result.Z));
            Assert.That(result.Oi.ToHex(), Is.EqualTo(expectedOi.ToHex()), nameof(result.Oi));
            Assert.That(result.Dkm.ToHex(), Is.EqualTo(expectedDkm.ToHex()), nameof(result.Dkm));
            Assert.That(result.MacData.ToHex(), Is.EqualTo(expectedMacData.ToHex()), nameof(result.MacData));
            Assert.That(result.Tag.ToHex(), Is.EqualTo(expectedTag.ToHex()), nameof(result.Tag));
        }

        private static IEnumerable<object> TestData = new[]
        {
            new object[]
            {
				// label
				"working test from vs",
				// scheme
				EccScheme.OnePassMqv,
				// parameter set
				EccParameterSet.Ed,
                // curve
                Curve.B409,
				// iut role
				KeyAgreementRole.ResponderPartyV,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d384,
				// KC mac
				KeyAgreementMacType.HmacSha2D384,
				// key len
				192,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("cfd42c6561"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Unilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("000E103B086AC76E472A7B45842B89AA572B34B87599DF749AE0591EEE1168EC9120C6B366C6F7E6559397A86B78AA47175C75EB").ToPositiveBigInteger(),
                        new BitString("009480400218DFEBEAE0B15FA4064EC02BF251F35DE3ABFF91E2D9BCD759DD1D839016D2F3443F827DF37001170E101610FF8013").ToPositiveBigInteger()),
                    new BitString("00E7233A212FB43EE637202BAFBAF7A091ED10813DC60DC3CBD56A0155CCA1CA0A25F775FE6232E737A23655B9763D59597C85BD").ToPositiveBigInteger()),
				// ephemeral key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("0172FF5F701104355F304C39135643E141601F054BA7165BAC750744FE5691CA2BB4583FF2D29C5DE705958F75F1B25EA09C0C97").ToPositiveBigInteger(),
                        new BitString("00F25385054D3E19C9D8E219651C4B721540695DF0487FDDD0425AC90DCEDF173F9CB805EDF44E37FACD073E74242372023DE621").ToPositiveBigInteger()),
                    new BitString("00C6DF4A8BBD99C85A57CDED34039FB233E93D53F22CCC61198512D295624348473B304B5675C7905F5CFBB5B8BDC8EA56E3B64C").ToPositiveBigInteger()),
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("01dccc77900e5b9d30af936b60a9d75c2d8b2481535f5309a31778e732ddbadf9b2e634703d61543c270c5bac83077f5293bdd69").ToPositiveBigInteger(),
                        new BitString("01dfb27a0712092b13c1c42616e14d67cc3ae96753fd679001b0cfa4f99693d0145b5dc7ff545306ae351e8a5851e91712b16c2f").ToPositiveBigInteger())),
                // iut ephemeral key
                null,
                // nonce ephemeral server
                null,
                // nonce ephemeral iut
                new BitString("22a77e474945fc29b0b763cc8248ba4419ade9b2925f1f0a376cc7dbf17a0fa5bcbcbaf0235b75f3c96d558ca41b6333a43e165e"),
				// oi
				new BitString("434156536964cfd42c65618b5be1fe60c8c7b6b296c7f9dd95723665c217c41901113f370b474f448fd4c0d41515d4"),
				// dkm
				new BitString("7fbec7e6e329e263f5a111f3085deeb96810dffdaeae7201"),
                // mac data
                null,
				// tag iut
				new BitString("5211226d40ad22466f00d7bac5f1c7fa"),
            },
            new object[]
            {
				// label
				"broken test from vs",
				// scheme
				EccScheme.OnePassMqv,
				// parameter set
				EccParameterSet.Ed,
                // curve
                Curve.B409,
				// iut role
				KeyAgreementRole.InitiatorPartyU,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d384,
				// KC mac
				KeyAgreementMacType.HmacSha2D384,
				// key len
				192,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("6b606d9a64"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Bilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("012FE188907418A2A3908E387C5651E4A8DADE4BC87A229A03E2FF6F4EE31BABF1BC09A228F8E3758BB9E0CBF4F8B79C36840FDB").ToPositiveBigInteger(),
                        new BitString("011190DFE859474A8216264949B6AA42E22F50900CFD46D96A1558ABED86237E64F190D5BF82B542E698E53FE39F222EFF98F600").ToPositiveBigInteger()),
                    new BitString("00BAB618FC65189F8D34E1639F986111B1114F68E2A5186B4A477920404ADE5DA9E17AC685D89EA16F0088F29C1123AA1D5E35AB").ToPositiveBigInteger()),
				// ephemeral key server
				null,
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("017a34ad2769e1a68bca80c0d3b5a3d6d980cc9b81af02f4c7a4dbef72e6c3817bcd61660e71f883de3eb891d24b03d7c4a355d3").ToPositiveBigInteger(),
                        new BitString("008a111fcd9b24dc28c318e9832e598579a5757d81beeab2cc9abbecb8f22194bea467970de0e99b2e9fea7b53dfde457a09ac3d").ToPositiveBigInteger())),
                // iut ephemeral key
                new EccKeyPair(
                    new EccPoint(
                        new BitString("013c06618a28208b1acdc7a233c53d1e0e9ea77c2dc71c45e703f53bbdc9bacb75104a8be3eb8524cfc4676ceda8dd0faf92f141").ToPositiveBigInteger(),
                        new BitString("0146c14a61ee963602f40289769e2418b02bfc3c778fe8e4a70c5615482bba781e337169126e70b0ad9af05acf81164b033f2bd6").ToPositiveBigInteger())),
                // nonce ephemeral server
                new BitString("057A81200E0E7DD00E9AAB63F089BF6B23E9DCBED36E7377972C365A1D37D842E78C1FEE50C606B3D12B33EBB419758C45D8605F8FA4CE652911E654C24EFB02861F7D265605038449AC23E1939C497D40F8E9187A5EA224B2B633948A901FF4CC8787FD56B680"), 
                // nonce ephemeral iut
                null,
				// oi
				new BitString("6b606d9a64434156536964ac58b58d8a93dd86c698f0d97734fec1c7af8541e92229ce48d1796a1b4ef8666181b9ab"),
				// dkm
				new BitString("792febdd0e0f9fa4b2b2b5d902ec9cb9238646b249fafb8e"),
                // mac data
                new BitString("4b435f325f564341565369646b606d9a64057a81200e0e7dd00e9aab63f089bf6b23e9dcbed36e7377972c365a1d37d842e78c1fee50c606b3d12b33ebb419758c45d8605f8fa4ce652911e654c24efb02861f7d265605038449ac23e1939c497d40f8e9187a5ea224b2b633948a901ff4cc8787fd56b680013c06618a28208b1acdc7a233c53d1e0e9ea77c2dc71c45e703f53bbdc9bacb75104a8be3eb8524cfc4676ceda8dd0faf92f1410146c14a61ee963602f40289769e2418b02bfc3c778fe8e4a70c5615482bba781e337169126e70b0ad9af05acf81164b033f2bd6"),
				// tag iut
				new BitString("7071549402f33c56537b4a2e86af2942"),
            },
            new object[]
            {
				// label
				"broken test from vs 2",
				// scheme
				EccScheme.OnePassMqv,
				// parameter set
				EccParameterSet.Ed,
                // curve
                Curve.B409,
				// iut role
				KeyAgreementRole.InitiatorPartyU,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d384,
				// KC mac
				KeyAgreementMacType.HmacSha2D384,
				// key len
				192,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("71f5451ec8"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Bilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("00120D018CD0D97C4D357812F3C1CBCA195629666E25D36005FD8A542DBF42C83B56952C2999F0D1F7ADE7C879D0067EADA3729D").ToPositiveBigInteger(),
                        new BitString("018AE7B24209B970600AAF3B386E17E02165C862DD330AC46033B70BF68137BA2F93B88D23A04316F4FB1B2D94C128C83549DB0E").ToPositiveBigInteger()),
                    new BitString("0094E99F7413BC9FAB5EFFDA8F8C267BB4C2D0D9FF565D2CE9DCFE73B46DBF9F94403EB46028B99DBD7D9AF7918DC040C436A226").ToPositiveBigInteger()),
				// ephemeral key server
				null,
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("006bbad541403bcfe900deecd71f790e287b3ca3777fc010bf34f0b42ae848e98379a81f1a0aad8c7308bf1c6fd6d9b32578be23").ToPositiveBigInteger(),
                        new BitString("00626812fc54d53de968b5ec92f8943dc5a75abb58156f90cf42ec2ac9ee464b30747ec8d466c208386d773130ccd4f28abf5c63").ToPositiveBigInteger())),
                // iut ephemeral key
                new EccKeyPair(
                    new EccPoint(
                        new BitString("01b6cdbe1dbf16c3bac71a42597444511ec3d5e1f76e19194d058088b264dd3a8cb8202fddc22d4d7609fa1c6a723fe943e03b95").ToPositiveBigInteger(),
                        new BitString("000ae1fa9ee2696510964180237f926ce8a1a66da3a9b121ae60a3fe7fa71d9de2ecad7d493377caba0a265574b88152dc1b6ed6").ToPositiveBigInteger())),
                // nonce ephemeral server
                new BitString("5F024066397ADEEFA49776BF05613816773E8590D2E05423BF54DFA3C7678618BD3702848544D3826F6D19972769E912DC2A618EEE0E6B61C6C5EB0CFD4E5CFE2E9DE92EB3D440D8CD73DF3E025AE96A61DD326425F4F6A625B698C65DA0F64EAB5BF210940B00"), 
                // nonce ephemeral iut
                null,
				// oi
				new BitString("71f5451ec84341565369643f835d2c1b2644d451fe4c65bab63398eee5c44e3654d83cb71ef6316134c6749c3c5791"),
				// dkm
				new BitString("99301c57ec3754807df5fddfd2e8559d50e639b2f35515a5"),
                // mac data
                null,
				// tag iut
				new BitString("34a89d93cad928180c0e3e9faa7a1bcc"),
            },
            new object[]
            {
				// label
				"passing test with iut provided ephemeral nonce",
				// scheme
				EccScheme.OnePassMqv,
				// parameter set
				EccParameterSet.Ed,
                // curve
                Curve.B409,
				// iut role
				KeyAgreementRole.ResponderPartyV,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d384,
				// KC mac
				KeyAgreementMacType.HmacSha2D384,
				// key len
				192,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("cfd42c6561"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Unilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("000E103B086AC76E472A7B45842B89AA572B34B87599DF749AE0591EEE1168EC9120C6B366C6F7E6559397A86B78AA47175C75EB").ToPositiveBigInteger(),
                        new BitString("009480400218DFEBEAE0B15FA4064EC02BF251F35DE3ABFF91E2D9BCD759DD1D839016D2F3443F827DF37001170E101610FF8013").ToPositiveBigInteger()),
                    new BitString("00E7233A212FB43EE637202BAFBAF7A091ED10813DC60DC3CBD56A0155CCA1CA0A25F775FE6232E737A23655B9763D59597C85BD").ToPositiveBigInteger()),
				// ephemeral key server
                new EccKeyPair(
                    new EccPoint(
                        new BitString("0172FF5F701104355F304C39135643E141601F054BA7165BAC750744FE5691CA2BB4583FF2D29C5DE705958F75F1B25EA09C0C97").ToPositiveBigInteger(),
                        new BitString("00F25385054D3E19C9D8E219651C4B721540695DF0487FDDD0425AC90DCEDF173F9CB805EDF44E37FACD073E74242372023DE621").ToPositiveBigInteger()),
                    new BitString("00C6DF4A8BBD99C85A57CDED34039FB233E93D53F22CCC61198512D295624348473B304B5675C7905F5CFBB5B8BDC8EA56E3B64C").ToPositiveBigInteger()),
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("01dccc77900e5b9d30af936b60a9d75c2d8b2481535f5309a31778e732ddbadf9b2e634703d61543c270c5bac83077f5293bdd69").ToPositiveBigInteger(),
                        new BitString("01dfb27a0712092b13c1c42616e14d67cc3ae96753fd679001b0cfa4f99693d0145b5dc7ff545306ae351e8a5851e91712b16c2f").ToPositiveBigInteger())),
                // iut ephemeral key
                null,
                // nonce ephemeral server
                null, 
                // nonce ephemeral iut
                new BitString("22a77e474945fc29b0b763cc8248ba4419ade9b2925f1f0a376cc7dbf17a0fa5bcbcbaf0235b75f3c96d558ca41b6333a43e165e"), 
				// oi
				new BitString("434156536964cfd42c65618b5be1fe60c8c7b6b296c7f9dd95723665c217c41901113f370b474f448fd4c0d41515d4"),
				// dkm
				new BitString("7fbec7e6e329e263f5a111f3085deeb96810dffdaeae7201"),
                // mac data
                null,
				// tag iut
				new BitString("5211226d40ad22466f00d7bac5f1c7fa"),
            },
            new object[]
            {
				// label
				"val test",
				// scheme
				EccScheme.OnePassMqv,
				// parameter set
				EccParameterSet.Ed,
                // curve
                Curve.B409,
				// iut role
				KeyAgreementRole.ResponderPartyV,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d384,
				// KC mac
				KeyAgreementMacType.HmacSha2D384,
				// key len
				192,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("a1b2c3d4e5"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Unilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("019D9AC69D63F472733FCEA108F5B4620F08E9E675082D9620197B8480CEEF14EFE6DE6FD7FB07217063A67513F82A4DF8F43EF3").ToPositiveBigInteger(),
                        new BitString("018E5BF863639989BD712C8278543C595A72FD66D86BBD9A64DEA4A4A1C1A28B3CB00332F4D880EED3BD7E00A605D226A3070077").ToPositiveBigInteger()),
                    new BitString("00A315FC644BEE86E61253323302889E0E6E7DF0553CF367A8217D77D62B53DFC4CF7CABFD41F55D497D637016448D2C7A58323C").ToPositiveBigInteger()),
				// ephemeral key server
                new EccKeyPair(
                    new EccPoint(
                        new BitString("01308E09994AB2B7F0849CFCB755187158A0774AA8926CB1DA50C96A552A043ED27D59EE47598926991DEFCE489BD32DF2EC8A33").ToPositiveBigInteger(),
                        new BitString("007B3D2A57DC787957674299617218ABC9AB378ED5C3A4BEC8DEF9E11F95498568DEC887D093D3B74309AB8E36B85FF128351D4F").ToPositiveBigInteger()),
                    new BitString("00831C0B591426A443FD21A3E9080B17EB701FFA95012209841A49C8932738A0603FAD1960F4BCA1BD961A2FFA96E47FEF376CE6").ToPositiveBigInteger()),
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("00F284F1BED2B959DAC0FE46FFEDE99EDECC8A4A90983B6501BB3A2E3AF75E8DC394ACF4CA694E3B1A293C5DE1428F5ACAFEFD49").ToPositiveBigInteger(),
                        new BitString("012B3EEE38EEE409F2298B7D697C97129ED34F79C8C3BCBE7E3F9D47C5D21924B6DEACC425871902C9CAE0FAEA179D3676ADF6C3").ToPositiveBigInteger())),
                // iut ephemeral key
                null,
                // nonce ephemeral server
                null, 
                // nonce ephemeral iut
                new BitString("824AF45EC5B4FAAD1127B9A14DB710CC9B074DB1F0D9F9012475424B4F89C8622515B68A838A946C83BDC7C8817B05FC5F7EA4188D25A938D577E959E17E2B18F03ADC93B3704B7F5FDAD75A128EB90DCF63897AE2F89D01A92A3ACF09F4475AD81A573A491500"), 
				// oi
				new BitString("434156536964A1B2C3D4E541378881DF6738743957CAC1A2AC70F19F1F6444AC0A4AA21E758DCB8EDFED7C2D9E0F18"),
				// dkm
				new BitString("648AE6180D8B2767E83D01929C3DCE424E51506F9B6197A4"),
                // mac data
                null,
				// tag iut
				new BitString("6da6d68cc7351814a6ab709bc4d78aae"),
            },
            new object[]
            {
				// label
				"https://github.com/usnistgov/ACVP/issues/951 299758 demo tcId 1",
				// scheme
				EccScheme.FullMqv,
				// parameter set
				EccParameterSet.Eb,
                // curve
                Curve.K233,
				// iut role
				KeyAgreementRole.ResponderPartyV,
				// kas mode
				KasMode.KdfKc,
				// hash
				HashFunctions.Sha2_d256,
				// KC mac
				KeyAgreementMacType.HmacSha2D256,
				// key len
				128,
				// mac len
				128,
				// kdf fixed info encoding
				FixedInfoEncoding.Concatenation,
				// server ID
				new BitString("434156536964"),
                // iut id
                new BitString("91cf25ba82"),
				// iut KC role
				KeyConfirmationRole.Recipient,
				// KC type
				KeyConfirmationDirection.Unilateral,
				// static key server
				new EccKeyPair(
                    new EccPoint(
                        new BitString("00DBCB786A61400FF76D2BA3B49BA368D4A76882C359814CB1B294D0336C").ToPositiveBigInteger(),
                        new BitString("00CBC37CF64FA5AC003EB51363002C97F997AED484A3C57C5F8FC7B85F61").ToPositiveBigInteger()),
                    new BitString("007A32A2D40C8AF6AC332C45D7ACCE3370F5A27F99B3D0294C8212A20976").ToPositiveBigInteger()),
				// ephemeral key server
                new EccKeyPair(
                    new EccPoint(
                        new BitString("006BAF03845410BC12EF9D2420839BEF3A51D45C5E6E3AEBC3ECF34E79E7").ToPositiveBigInteger(),
                        new BitString("01C079A0F672D5A8D0C04A408CC8E376DD1D22B9DD272DA5D9EC86C721B3").ToPositiveBigInteger()),
                    new BitString("000005631E2975B9A5BED1FC9F5334DEB831E14F19E2356693FE4C99143D").ToPositiveBigInteger()),
				// iut static key
				new EccKeyPair(
                    new EccPoint(
                        new BitString("016a857f1a4eed68b60689be8ded91fc7f3c01e1137d9db2d65a93fee7d7").ToPositiveBigInteger(),
                        new BitString("018f5693a108229a161ab92fa0fd12aa3fb43f6890e0c6b2a0e019f234da").ToPositiveBigInteger())),
                // iut ephemeral key
                new EccKeyPair(
                    new EccPoint(
                        new BitString("01b41756b267c5b1edbf85072e428b776377094246959c4da5d221cc5e4b").ToPositiveBigInteger(),
                        new BitString("008a01de26cd9f85940ca0a879b6c11ed5fe80b73871caa830eed4a3eacf").ToPositiveBigInteger())),
                // nonce ephemeral server
                null, 
                // nonce ephemeral iut
                null, 
				// oi
				new BitString("43415653696491cf25ba828c2251401d0f6d5948796292a36825f07301f73c1c2a384be052b09b009e6fad16e5f016"),
				// dkm
				new BitString("eb44b3d55fd44cf4469a6554c49cb9ab"),
                // mac data
                null,
				// tag iut
				new BitString("3fe6e9cdcf724a64aa330573de36045c"),
            },
        };

        [Test]
        [TestCaseSource(nameof(TestData))]
        public void DebugTestsWithKc(
            string label,
            EccScheme scheme,
            EccParameterSet parameterSet,
            Curve curveName,
            KeyAgreementRole iutRole,
            KasMode kasMode,
            HashFunctions hash,
            KeyAgreementMacType kcMac,
            int keyLen,
            int macLen,
            FixedInfoEncoding fixedInfoEncoding,
            BitString serverId,
            BitString iutId,
            KeyConfirmationRole iutKcRole,
            KeyConfirmationDirection kcDirection,
            EccKeyPair staticKeyServer,
            EccKeyPair ephemeralKeyServer,
            EccKeyPair staticKeyIut,
            EccKeyPair ephemeralKeyIut,
            BitString nonceEphemeralServer,
            BitString nonceEphemeralIut,
            BitString iutOi,
            BitString iutDkm,
            BitString iutMacData,
            BitString iutTag
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var otherPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    iutId,
                    staticKeyIut,
                    ephemeralKeyIut,
                    null,
                    nonceEphemeralIut,
                    null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = serverId.BitLength +
                                    iutId.BitLength;

            var entropyBits = iutOi.GetLeastSignificantBits(iutOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // // add dkm nonce to entropy provided when needed
            // if (thisPartyDkmNonce != null)
            // {
            //     _entropyProviderScheme.AddEntropy(thisPartyDkmNonce);
            // }
            //
            // // MAC no key confirmation data makes use of a nonce
            // if (thisPartyEphemeralNonce != null)
            // {
            //     _entropyProviderScheme.AddEntropy(thisPartyEphemeralNonce);
            // }

            if (nonceEphemeralServer != null)
            {
                _entropyProviderScheme.AddEntropy(nonceEphemeralServer);
            }

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new NativeShaFactory().GetShaInstance(ShaAttributes.GetHashFunctionFromEnum(hash)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(kcMac)
                .WithMacLength(macLen)
                //.WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(iutRole))
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithPartyId(serverId)
                .BuildKdfKc()
                .WithKeyLength(keyLen)
                .WithMacParameters(macParams)
                .WithKeyConfirmationRole(KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(iutKcRole))
                .WithKeyConfirmationDirection(kcDirection)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = staticKeyServer.PrivateD;
                kas.Scheme.StaticKeyPair.PublicQ = staticKeyServer.PublicQ;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = ephemeralKeyServer.PrivateD;
                kas.Scheme.EphemeralKeyPair.PublicQ = ephemeralKeyServer.PublicQ;
            }

            var result = kas.ComputeResult(otherPartySharedInformation);

            Assert.That(iutDkm.ToHex(), Is.EqualTo(result.Dkm.ToHex()), nameof(result.Dkm));

            if (iutMacData != null)
            {
                Assert.That(result.MacData.ToHex(), Is.EqualTo(iutMacData.ToHex()), nameof(result.MacData));
            }
            Assert.That(iutTag.ToHex(), Is.EqualTo(result.Tag.ToHex()), nameof(result.Tag));
        }

        [Test]
        public void TestKeyConfirmation()
        {
            var keyConfirmationFactory = new KeyConfirmationFactory(new KeyConfirmationMacDataCreator());
            var keyConfirmation = keyConfirmationFactory.GetInstance(new KeyConfirmationParameters(
                KeyAgreementRole.InitiatorPartyU,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Unilateral,
                KeyAgreementMacType.HmacSha2D256,
                128,
                128,
                new BitString("434156536964"),
                new BitString("91cf25ba82"),
                new BitString("006BAF03845410BC12EF9D2420839BEF3A51D45C5E6E3AEBC3ECF34E79E701C079A0F672D5A8D0C04A408CC8E376DD1D22B9DD272DA5D9EC86C721B3"),
                new BitString("01b41756b267c5b1edbf85072e428b776377094246959c4da5d221cc5e4b008a01de26cd9f85940ca0a879b6c11ed5fe80b73871caa830eed4a3eacf"),
                new BitString("eb44b3d55fd44cf4469a6554c49cb9ab")
            ));

            var result = keyConfirmation.ComputeMac();

            Assert.That(result.Mac.ToHex(), Is.EqualTo(new BitString("3fe6e9cdcf724a64aa330573de36045c").ToHex()));
        }
    }
}
