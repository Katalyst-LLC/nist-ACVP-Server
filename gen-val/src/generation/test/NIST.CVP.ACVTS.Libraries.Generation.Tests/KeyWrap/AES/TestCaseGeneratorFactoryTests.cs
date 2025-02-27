﻿using System;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.AES
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt<TestGroup, TestCase>))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt<TestGroup, TestCase>))]
        [TestCase("", typeof(TestCaseGeneratorNull<TestGroup, TestCase>))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Direction = direction
            };

            var subject = new TestCaseGeneratorFactory<TestGroup, TestCase>(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }
    }
}
