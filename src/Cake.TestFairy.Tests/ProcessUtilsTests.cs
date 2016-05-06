using Cake.Core;
using Cake.TestFairy.Internal;
using Cake.TestFairy.Internal.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Cake.TestFairy.Tests
{
    public class ProcessUtilsTests
    {
        [Test]
        public void CanExecute_ReturnsTrue_ForBuiltInCommand()
        {
            IProcessUtils processUtils = new ProcessUtils();

            bool canExecute = processUtils.CanExecute("xcopy");

            canExecute.Should().BeTrue();
        }

        [Test]
        public void CanExecute_ReturnsFalse_ForUnknownCommand()
        {
            IProcessUtils processUtils = new ProcessUtils();

            bool canExecute = processUtils.CanExecute("kdajfjkd.kks");

            canExecute.Should().BeFalse();
        }

        [Test]
        public void RunCommand_ThrowsException_ForUnknownCommand()
        {
            IProcessUtils processUtils = new ProcessUtils();

            Assert.Throws<CakeException>(() => processUtils.RunCommand("kdajfjkd.kks", ""));

        }

        [Test]
        [Ignore("Causes screen flashes")]
        public void RunCommand_ThrowsException_WhenCommandReturnsNonZeroResult()
        {
            IProcessUtils processUtils = new ProcessUtils();

            Assert.Throws<CakeException>(() => processUtils.RunCommand("stub.exe", ""));
        }

        [Test]
        [Ignore("Causes screen flashes")]
        public void RunCommand_ExecutesCommand()
        {
            IProcessUtils processUtils = new ProcessUtils();

            processUtils.RunCommand("stub.exe", "parameter");
        }
    }
}