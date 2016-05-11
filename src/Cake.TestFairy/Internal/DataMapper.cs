using System.Collections.Specialized;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    class DataMapper : IDataMapper
    {
        public NameValueCollection MapSettings(TestFairyUploadSettings settings)
        {
            var formData = new NameValueCollection();
            formData.Add("api_key", settings.ApiKey);
            formData.Add("icon-watermark", settings.IconWaterMark ? "on" : "off");
            string videoSetting;
            switch (settings.VideoRecording)
            {
                case VideoRecording.Off:
                    videoSetting = "off";
                    break;
                case VideoRecording.Wifi:
                    videoSetting = "wifi";
                    break;
                default:
                    videoSetting = "on";
                    break;
            }
            formData.Add("video", videoSetting);
            formData.Add("max-duration", $"{settings.MaxDurationMinutes}m");
            formData.Add("comment", settings.Comment);
            formData.Add("auto-update", settings.AutoUpdate ? "on" : "off");
            formData.Add("notify", settings.EmailTesters ? "on" : "off");
            if (settings.TesterGroups != null)
                formData.Add("testers-groups", string.Join(",", settings.TesterGroups));
            return formData;
        }
    }
}