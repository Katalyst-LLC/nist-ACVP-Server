﻿using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var parameterBuilder = new ParameterBuilder();
            var result = subject.Validate(parameterBuilder.Build());

            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase(2048, 160)]
        [TestCase(2049, 224)]
        [TestCase(3072, 257)]
        [TestCase(1024, 160)]
        [TestCase(1, 2)]
        public void ShouldReturnErrorWithInvalidLNPair(int L, int N)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(L, N)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(2048, 224)]
        [TestCase(2048, 256)]
        [TestCase(3072, 256)]
        public void ShouldReturnSuccessWithNewCapability(int L, int N)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(L, N)
                    })
                    .Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            _algorithm = "DSA";
            _mode = "keyGen";
            _capabilities = new[] { GetCapabilityWith(2048, 224), GetCapabilityWith(2048, 256), GetCapabilityWith(3072, 256) };
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithCapabilities(Capability[] value)
        {
            _capabilities = value;
            return this;
        }

        public static Capability GetCapabilityWith(int l, int n)
        {
            return new Capability
            {
                L = l,
                N = n
            };
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Capabilities = _capabilities
            };
        }
    }
}
