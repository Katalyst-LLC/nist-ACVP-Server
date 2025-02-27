﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        [JsonIgnore] public int IvLength { get; set; }
        [JsonIgnore] public int KeyInLength { get; set; }
        public int KeyOutLength { get; set; }
        public KdfModes KdfMode { get; set; }
        public MacModes MacMode { get; set; }
        public int CounterLength { get; set; }
        public CounterLocations CounterLocation { get; set; }
        public bool ZeroLengthIv { get; set; }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keyoutlength":
                case "l":
                    KeyOutLength = int.Parse(value);
                    return true;

                case "prf":
                    value = value.Replace("_", "-");
                    switch (value.ToLower())
                    {
                        case "hmac-sha1":
                            MacMode = MacModes.HMAC_SHA1;
                            break;
                        case "hmac-sha224":
                            MacMode = MacModes.HMAC_SHA224;
                            break;
                        case "hmac-sha256":
                            MacMode = MacModes.HMAC_SHA256;
                            break;
                        case "hmac-sha384":
                            MacMode = MacModes.HMAC_SHA384;
                            break;
                        case "hmac-sha512":
                            MacMode = MacModes.HMAC_SHA512;
                            break;
                        default:
                            MacMode = EnumHelpers.GetEnumFromEnumDescription<MacModes>(value);
                            break;
                    }
                    return true;

                case "ctrlocation":
                    if (value.ToLower().Equals("before_iter"))
                    {
                        CounterLocation = CounterLocations.BeforeIterator;
                        return true;
                    }
                    else if (value.ToLower().Equals("after_iter"))
                    {
                        CounterLocation = CounterLocations.BeforeFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("after_fixed"))
                    {
                        CounterLocation = CounterLocations.AfterFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("before_fixed"))
                    {
                        CounterLocation = CounterLocations.BeforeFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("middle_fixed"))
                    {
                        CounterLocation = CounterLocations.MiddleFixedData;
                        return true;
                    }
                    return false;

                case "rlen":
                    value = value.Split("_")[0];
                    CounterLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
