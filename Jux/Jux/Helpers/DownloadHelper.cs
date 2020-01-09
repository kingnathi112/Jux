using Jux.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Jux.Data.Search_Category.Category;

namespace Jux.Helpers
{
   public class DownloadHelper
    {
        private bool downloadCancelled = false;
        public bool downloadCompleted = false;
        public bool IsExisting = false;
        public int downloadProgress = 0;
        private string downloadFileLocation = "";

        public string Song(Download AlbumOrSingle, string Url, string Artist, string Album, string Title, string Location, string AudioType, int QualitySIze, int TrackNo = 1)
        {
            string url = Url;
            var audioFileName = "";
            if (AlbumOrSingle.ToString() == "Single")
            {
                audioFileName = $"{TrackNo}. {Artist} - {Title}.{AudioType}";
            }
            else if(AlbumOrSingle.ToString() == "Album")
            {
                audioFileName = $"{TrackNo}. {Title}.{AudioType}";
            }

            var fileLocation = Path.Combine(Location, audioFileName);

            if (File.Exists(fileLocation))
            {
                var fileSize = Math.Round(Convert.ToDouble(new FileInfo(fileLocation).Length) / Convert.ToDouble(1000000), 2);
                var songSize = Math.Round(Convert.ToDouble(QualitySIze) / Convert.ToDouble(1000000), 2);

                if(fileSize < songSize)
                {
                    IsExisting = false;
                    using (var wbClient = new WebClient())
                    {
                        wbClient.DownloadProgressChanged += WbClient_DownloadProgressChanged;
                        wbClient.DownloadFileCompleted += WbClient_DownloadFileCompleted;
                        wbClient.DownloadFileAsync(new Uri(url), fileLocation);
                        return fileLocation;
                    }
                }
                else
                {
                    IsExisting = true;
                    return fileLocation;
                }
            }
            else
            {
                IsExisting = false;
                using (var wbClient = new WebClient())
                {
                    wbClient.DownloadProgressChanged += WbClient_DownloadProgressChanged;
                    wbClient.DownloadFileCompleted += WbClient_DownloadFileCompleted;
                    wbClient.DownloadFileAsync(new Uri(url), fileLocation);
                    return fileLocation;
                }
            }
        }

        public string AlbumImage(string Url, string AlbumTitle, string Location)
        {
            string url = Url;
            var fileLocation = Path.Combine(Location, AlbumTitle + ".png");

            using (var wbClient = new WebClient())
            {
                wbClient.DownloadProgressChanged += WbClient_DownloadProgressChanged;
                wbClient.DownloadFileCompleted += WbClient_DownloadFileCompleted;
                wbClient.DownloadFileAsync(new Uri(url), fileLocation);
                return fileLocation;
            }
        }
        private void WbClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress = e.ProgressPercentage;
        }

        private void WbClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            downloadCompleted = true;
        }
    }
}
