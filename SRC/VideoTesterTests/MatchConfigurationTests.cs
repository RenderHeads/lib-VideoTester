﻿using NUnit.Framework;
using LibVideoTester;
using System.Collections.Generic;

namespace VideoTesterTests
{
    public class MatchConfigurationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void shouldReturnAMatchIfAConfigurationPass()
        {
            string [] validCodecs = new string[] { "hap", "h264","hevc", "hapa"};
            Configuration config1 = new Configuration(validCodecs,1920,1080);
            Configuration config2 = new Configuration(validCodecs, 200, 200);
            VideoInfo v = new VideoInfo("hap",400,400);

            List<Configuration> matches = null;
            Assert.IsTrue(ConfigurationMatcher.TryGetMatches(v, out matches, new List<Configuration> { config1, config2 }));
            Assert.Contains(config1, matches);
            
            
        }

  


    }
}
