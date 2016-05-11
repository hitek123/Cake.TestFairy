namespace Cake.TestFairy
{
    public class TestFairyUploadResponse
    {
        public string Status { get; set; }
        public bool Success => Status == "ok";
        public string App_name { get; set; }
        public string App_version { get; set; }
        public int File_size { get; set; }
        public string Build_url { get; set; }
        public string Instrumented_url { get; set; }
        public string Invite_testers_url { get; set; }
        public string Icon_url { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}