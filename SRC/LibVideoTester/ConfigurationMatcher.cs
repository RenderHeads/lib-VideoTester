using System;
using System.Collections.Generic;

namespace LibVideoTester
{
    public static class ConfigurationMatcher
    {
        public static bool TryGetMatches(VideoMetaData v, out List<Configuration> matches, List<Configuration> configurations)
        {
            matches =  new List<Configuration>();
            foreach (Configuration c in configurations)
            {
                    if (v.TestConfiguration(c))
                    {
                        matches.Add(c);
                    }
            }
            return matches.Count > 0;
        }
    }
}

