﻿using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorPQTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodPQ()
        {
            var subject = new TestCaseValidatorPQ(GetTestCase(), GetTestGroup(), GetResolverMock(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadG()
        {
            var subject = new TestCaseValidatorPQ(GetTestCase(), GetTestGroup(), GetResolverMock(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                TestGroupId = 1
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                P = 2,
                Q = 3
            };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, PQValidateResult>> GetResolverMock(bool shouldPass)
        {
            var goodResult = Task.FromResult(new PQValidateResult());
            var badResult = Task.FromResult(new PQValidateResult("fail"));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, PQValidateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
