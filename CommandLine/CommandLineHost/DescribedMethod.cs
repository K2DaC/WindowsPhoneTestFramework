using System;

namespace WindowsPhoneTestFramework.CommandLineHost
{
    public class DescribedMethod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Action<string> Action { get; set; }
    }
}