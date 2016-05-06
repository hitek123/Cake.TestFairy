using System;
using System.Diagnostics;
using Cake.Core;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    class ProcessUtils : IProcessUtils
    {
        public void RunCommand(string executable, string arguments)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = arguments;
            try
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new CakeException(output) {Source = "RunCommand"};
            }
            catch (Exception e)
            {
                throw new CakeException("RunCommand failed", e) { Source = "RunCommand" };

            }
        }

        public bool CanExecute(string executable)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo(executable);
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                Process.Start(processStartInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}