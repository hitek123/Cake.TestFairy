using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Cake.Core.IO;
using Cake.TestFairy.Internal.Interfaces;
using Newtonsoft.Json;

namespace Cake.TestFairy.Internal
{
    internal class TestFairyFileTransfer : ITestFairyFileTransfer
    {
        public T Upload<T>(string url, FilePath filePath, string fileParameterName, string fileContentType,
            NameValueCollection nameValueCollection)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpRequest.Method = "POST";
            httpRequest.KeepAlive = true;
            httpRequest.Credentials = CredentialCache.DefaultCredentials;

            using (Stream requestStream = httpRequest.GetRequestStream())
            {
                //Add the formdata
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nameValueCollection.Keys)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nameValueCollection[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                //Build the header
                string headerTemplate =
                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, fileParameterName, filePath, fileContentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                requestStream.Write(headerbytes, 0, headerbytes.Length);

                //Add the file
                FileStream fileStream = new FileStream(filePath.FullPath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                //Add the footer
                byte[] footer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(footer, 0, footer.Length);

                using (WebResponse uploadResponse = httpRequest.GetResponse())
                {
                    using (Stream stream = uploadResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var response = reader.ReadToEnd();
                            var responseDto = JsonConvert.DeserializeObject<T>(response);
                            return responseDto;
                        }
                    }
                }
            }
        }

        public void Download(string uri, FilePath filePath)
        {
            using (var webClient = new WebClient())
                webClient.DownloadFile(uri, filePath.FullPath);
        }
    }
}