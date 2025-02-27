﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CFB1.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private readonly MonteCarloAesCfb _subject = new MonteCarloAesCfb(
            new BlockCipherEngineFactory(),
            new ModeBlockCipherFactory(),
            new AesMonteCarloKeyMaker(),
            1,
            BlockCipherModesOfOperation.CfbBit
        );

        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            string keyString = "19c27cdff233706c4c6176eb63a4568e";
            string ivString = "30c6e45842a35240331669e7bd3ee1f3";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var firstExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var secondExpectedKey = new BitString("d00433b558d5e5b504620846ea9335d9");
            var secondExpectedIv = new BitString("c9c64f6aaae695d948037ead89376357");
            var secondExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var secondExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var lastExpectedKey = new BitString("0b677b274bde90ae72844bff9eb0d0b3");
            var lastExpectedIv = new BitString("337584c1b9d72ddb72b0a544b0f41dbf");
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, plainText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText), nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondPlainText = result.Response[1].PlainText;
            var secondCipherText = result.Response[1].CipherText;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));
            Assert.That(secondPlainText, Is.EqualTo(secondExpectedPlainText), nameof(secondExpectedPlainText));
            Assert.That(secondCipherText, Is.EqualTo(secondExpectedCipherText), nameof(secondExpectedCipherText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            string keyString = "5f21cf99ae456ed8d65cbc72c0b89759e20cb1091bc69376";
            string ivString = "6e26057d460762ac302a9a64272a2944";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var firstExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var secondExpectedKey = new BitString("579fd63a9d329fa44285e3f834a94747d56c1278a9fc3c3d");
            var secondExpectedIv = new BitString("94d95f8af411d01e3760a371b23aaf4b");

            var lastExpectedKey = new BitString("ffa875bfb283d86a75801112b048bdd4d2f6e5caafbd9994");
            var lastExpectedIv = new BitString("3f597d5af4d9b1a3980cea2b4f60118e");
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, plainText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText), nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            string keyString = "299182240c5ff67364c6b1aac2e3f06858f290650c12c359cb47071920c55903";
            string ivString = "5323e8ab1eba7a3212846dabf1493549";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var firstExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));

            var secondExpectedKey = new BitString("ebd07fe190d02504be720ebaf56c963e6375cb3ea01105c9fdfe112f826c9b75");
            var secondExpectedIv = new BitString("3b875b5bac03c69036b91636a2a9c276");

            var lastExpectedKey = new BitString("101dd780875c359a7a4327fe2951e70939db234e72b09c579c12abb01c0f8c1b");
            var lastExpectedIv = new BitString("23028f02b4adc4a5d35ed0e120b142bc");
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, plainText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText), nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            string keyString = "d3057c24774e4420c5dd6d66eeb04912";
            string ivString = "cdb7d2e7a5e85f2b5e6a08d89b2c9bdd";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var firstExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));

            var secondExpectedKey = new BitString("0679aba4f75ae4e820fd2d2caa297854");
            var secondExpectedIv = new BitString("d57cd7808014a0c8e520404a44993146");
            var secondExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var secondExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var lastExpectedKey = new BitString("d5f29f0400854d31f2ddcdd5c32ce3ba");
            var lastExpectedIv = new BitString("89b57c371d635d67a83aa3ebb4f4a03d");
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt, iv, key, cipherText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.That(firstPlainText, Is.EqualTo(firstExpectedPlainText), nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));
            Assert.That(secondCipherText, Is.EqualTo(secondExpectedCipherText), nameof(secondExpectedCipherText));
            Assert.That(secondPlainText, Is.EqualTo(secondExpectedPlainText), nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            string keyString = "81e26656c1be3dc9b7275ba3d224bac3fd42c4ba1637688d";
            string ivString = "ebe322635745494100c2b0bbcafc7a33";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var firstExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));

            var secondExpectedKey = new BitString("e98067eb3a70d4e54fbe0fbd36aa282e41ab0af9a96d2db9");
            var secondExpectedIv = new BitString("f899541ee48e92edbce9ce43bf5a4534");
            var secondExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var secondExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var lastExpectedKey = new BitString("481dac4354c7a602f31d9fd33119cd834f69277fabba5c5f");
            var lastExpectedIv = new BitString("40648ac18d340bf6f83887de83cb28db");
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt, iv, key, cipherText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.That(firstPlainText, Is.EqualTo(firstExpectedPlainText), nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));
            Assert.That(secondCipherText, Is.EqualTo(secondExpectedCipherText), nameof(secondExpectedCipherText));
            Assert.That(secondPlainText, Is.EqualTo(secondExpectedPlainText), nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            string keyString = "63a8a289c2f89f0246e2c42f264ea27013b51e24bf9416c0a01a481ef2246f17";
            string ivString = "6ec27ed42082fb5fb84c27829b03226d";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var firstExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));

            var secondExpectedKey = new BitString("4c1189c6218791ceba937ff30c4c4bee86b03c7569edf1604cd4a816aa8dbaf7");
            var secondExpectedIv = new BitString("95052251d679e7a0eccee00858a9d5e0");
            var secondExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var secondExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1"));

            var lastExpectedKey = new BitString("9ccc8ef62058690c1265f314c752bdcb6c90e7ceaa9ca313a55ff48862005491");
            var lastExpectedIv = new BitString("f2037905c418ed80780ffccdb650bf3c");
            var lastExpectedCipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));
            var lastExpectedPlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"));


            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt, iv, key, cipherText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.That(firstPlainText, Is.EqualTo(firstExpectedPlainText), nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondCipherText = result.Response[1].CipherText;
            var secondPlainText = result.Response[1].PlainText;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));
            Assert.That(secondCipherText, Is.EqualTo(secondExpectedCipherText), nameof(secondExpectedCipherText));
            Assert.That(secondPlainText, Is.EqualTo(secondExpectedPlainText), nameof(secondExpectedPlainText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void Cfb8()
        {
            string keyString = "be2742df6d0139a45eb0e9a1822eef4a";
            string ivString = "3d3d13c3096535cfd1889e592308f9f2";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("8d");
            var firstExpectedCipherText = new BitString("8a");

            var secondExpectedKey = new BitString("0078ef5381ec8f98b21ff995e1f058c0");
            var secondExpectedIv = new BitString("be5fad8cecedb63cecaf103463deb78a");
            var secondExpectedPlainText = new BitString("26");
            var secondExpectedCipherText = new BitString("ff");

            var lastExpectedKey = new BitString("2ba452ed17c06b9602fd6a80f3112c38");
            var lastExpectedIv = new BitString("b0b335f8827ecd6e2afe15b5b87a8185");
            var lastExpectedPlainText = new BitString("6f");
            var lastExpectedCipherText = new BitString("77");


            var subject = new MonteCarloAesCfb(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                new AesMonteCarloKeyMaker(),
                8,
                BlockCipherModesOfOperation.CfbByte
            );
            var result = subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, plainText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText), nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;
            var secondPlainText = result.Response[1].PlainText;
            var secondCipherText = result.Response[1].CipherText;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));
            Assert.That(secondPlainText, Is.EqualTo(secondExpectedPlainText), nameof(secondExpectedPlainText));
            Assert.That(secondCipherText, Is.EqualTo(secondExpectedCipherText), nameof(secondExpectedCipherText));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void Cfb128()
        {
            string keyString = "ef22e67279d033c22053886c67ebb15a";
            string ivString = "0dfb0fe904ea07f356e58470423e5f69";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("f9f662a43a988a3f7116ead7c60886ef");
            var firstExpectedCipherText = new BitString("909e9a16af24281a1f4771ba360d8733");

            var secondExpectedKey = new BitString("7fbc7c64d6f41bd83f14f9d651e63669");
            var secondExpectedIv = new BitString("909e9a16af24281a1f4771ba360d8733");

            var lastExpectedKey = new BitString("01c78e54c67009f34e3a9cb740744111");
            var lastExpectedIv = new BitString("4e5ffce071752c3cb513e3a6f7347da5");
            var lastExpectedPlainText = new BitString("3bff30f019dbf7457c6c188d16e7b0e3");
            var lastExpectedCipherText = new BitString("c51817da5cb5a2ffb9f71b9cbbb0b087");


            var subject = new MonteCarloAesCfb(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                new AesMonteCarloKeyMaker(),
                128,
                BlockCipherModesOfOperation.CfbBlock
            );
            var result = subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, plainText
            ));


            Assert.That(sanityCheckFirstKey, Is.EqualTo(key), nameof(sanityCheckFirstKey));
            Assert.That(sanityCheckFirstIv, Is.EqualTo(iv), nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText), nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.That(secondKey, Is.EqualTo(secondExpectedKey), nameof(secondExpectedKey));
            Assert.That(secondIv, Is.EqualTo(secondExpectedIv), nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.That(lastKey, Is.EqualTo(lastExpectedKey), nameof(lastExpectedKey));
            Assert.That(lastIv, Is.EqualTo(lastExpectedIv), nameof(lastExpectedIv));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText), nameof(lastPlainText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }
    }
}
