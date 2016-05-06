using System.Collections.Specialized;

namespace Cake.TestFairy.Internal.Interfaces
{
    public interface IDataMapper
    {
        NameValueCollection MapSettings(TestFairyUploadSettings settings);
    }
}