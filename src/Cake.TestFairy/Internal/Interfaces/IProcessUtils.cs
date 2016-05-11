namespace Cake.TestFairy.Internal.Interfaces
{
    public interface IProcessUtils
    {
        void RunCommand(string executable, string arguments);
        bool CanExecute(string executable);
    }
}