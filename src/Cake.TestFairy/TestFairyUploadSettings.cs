using System.Collections.Generic;

namespace Cake.TestFairy
{
    public class TestFairyUploadSettings
    {

        public TestFairyUploadSettings(string apiKey)
        {
            ApiKey = apiKey;
            ServerEndpoint = "http://app.testfairy.com";
            IconWaterMark = true;
            EmailTesters = true;
            TesterGroups = new List<string>();
            VideoRecording = VideoRecording.Wifi;
            MaxDurationMinutes = 10;
            Jarsigner = "jarsigner.exe";
            ZipUtility = "zip.exe";
            KeyTool = "keytool.exe";
            ZipAlign = "zipalign.exe";
            KeyStore = new KeyStore();
        }

        public string ServerEndpoint { get; set; }

        public string ApiKey { get; set; }
        public KeyStore KeyStore { get; set; }
        public bool IconWaterMark { get; set; }
        public bool EmailTesters { get; set; }
        public IEnumerable<string> TesterGroups { get; set; }
        public bool AutoUpdate { get; set; }
        public VideoRecording VideoRecording { get; set; }
        public int MaxDurationMinutes { get; set; }
        public string Comment { get; set; }

        public string Jarsigner { get; set; }
        public string ZipAlign { get; set; }
        public string KeyTool { get; set; }
        public string ZipUtility { get; set; }

    }
}