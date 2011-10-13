using System;
using System.ComponentModel;
using WindowsPhoneTestFramework.EmuDriver;

namespace WindowsPhoneTestFramework.EmuHost.Commands
{
    public class DriverCommands
    {
        public IDriver Driver { get; set; }
        public AppLaunchingCommandLine CommandLine;

        [DisplayName("install")]
        [Description("installs the app - e.g. 'install'")]
        public void Install(string ignored)
        {
            var result = Driver.Install(CommandLine.ProductId, CommandLine.Name, CommandLine.IconPath, CommandLine.XapPath);
            Console.WriteLine("install:" + result);
        }

        [DisplayName("forceInstall")]
        [Description("installs the app - shutting it down first if required - e.g. 'forceInstall'")]
        public void ForceInstall(string ignored)
        {
            var result = Driver.ForceInstall(CommandLine.ProductId, CommandLine.Name, CommandLine.IconPath, CommandLine.XapPath);
            Console.WriteLine("forceInstall:" + result);
        }

        [DisplayName("uninstall")]
        [Description("uninstalls the app - e.g. 'uninstall'")]
        public void Uninstall(string ignored)
        {
            var result = Driver.Uninstall(CommandLine.ProductId);
            Console.WriteLine("uninstall:" + result);
        }

        [DisplayName("forceUninstall")]
        [Description("uninstalls the app - shutting it down first if required - e.g. 'forceUninstall'")]
        public void ForceUninstall(string ignored)
        {
            var result = Driver.ForceUninstall(CommandLine.ProductId);
            Console.WriteLine("forceUninstall:" + result);
        }

        [DisplayName("launch")]
        [Description("launches the app - e.g. 'launch'")]
        public void Launch(string ignored)
        {
            var result = Driver.Start(CommandLine.ProductId);
            Console.WriteLine("launch:" + result);
        }

        [DisplayName("stop")]
        [Description("stop the app - e.g. 'stop'")]
        public void Stop(string ignored)
        {
            var result = Driver.Stop(CommandLine.ProductId);
            Console.WriteLine("stop:" + result);
        }
    }
}