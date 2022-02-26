﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace TweakUIX.Helpers
{
    internal class Utils
    {

        public void CheckForUpdates()
        {
            if (IsInet() == true)
            {
                string assemblyInfo = new WebClient().DownloadString(Strings.Uri.URL_ASSEMBLY);

                try
                {
                    var readVersion = assemblyInfo.Split('\n');
                    var infoVersion = readVersion.Where(t => t.Contains("[assembly: AssemblyFileVersion"));
                    var latestVersion = "";
                    foreach (var item in infoVersion)
                    {
                        latestVersion = item.Substring(item.IndexOf('(') + 2, item.LastIndexOf(')') - item.IndexOf('(') - 3);
                    }

                    if (latestVersion ==
                        Program.GetCurrentVersionTostring())                                                     // Up-to-date

                        MessageBox.Show("No new release found.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (latestVersion !=                                                                        // Update available
                       Program.GetCurrentVersionTostring())

                    {
                        if (MessageBox.Show($"Tweak UIX {latestVersion} is available.\nDo you want to goto the Github page?", @"App update available",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Process.Start(Helpers.Strings.Uri.URL_GITLATEST);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Checking for App updates failed.\n{ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (IsInet() == false)
            {
                MessageBox.Show("Checking for App updates failed.\n\nCheck your Internet connection and try again.");
            }
        }

        // Check Inet
        public static bool IsInet()
        {
            try
            {
                using (var CheckInternet = new WebClient())
                using (CheckInternet.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Launch Urls in richText control
        public static void LaunchUri(string url)
        {
            if (IsHttpURL(url)) Process.Start(url);
        }

        // Check Urls in in richText control
        public static bool IsHttpURL(string url)
        {
            return
                ((!string.IsNullOrWhiteSpace(url)) &&
                (url.ToLower().StartsWith("http")));
        }
    }
}