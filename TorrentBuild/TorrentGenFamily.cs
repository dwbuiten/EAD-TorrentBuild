namespace TorrentBuild
{
    using EAD.Torrent;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.IO;

    [StandardModule]
    internal sealed class TorrentGenFamily
    {
        public static int GetWebSeedData(ArrayList returnarray)
        {
            int count = 0;
            if (File.Exists(TorrentBuild.LocalPath + "wsa.configure"))
            {
                int fileNumber = FileSystem.FreeFile();
                FileSystem.FileOpen(fileNumber, TorrentBuild.LocalPath + "wsa.configure", OpenMode.Binary, OpenAccess.Read, OpenShare.LockRead, -1);
                string str = Strings.Space((int) FileSystem.FileLen(TorrentBuild.LocalPath + "wsa.configure"));
                FileSystem.FileGet(fileNumber, ref str, -1L, false);
                FileSystem.FileClose(new int[] { fileNumber });
                TorrentDictionary dictionary = new TorrentDictionary();
                dictionary.Parse(str);
                if (dictionary.Contains("seedlist"))
                {
                    TorrentList list = new TorrentList();
                    list = (TorrentList) dictionary["seedlist"];
                    returnarray = list.Value;
                    count = returnarray.Count;
                }
                else
                {
                    TorrentString str2 = new TorrentString();
                    TorrentString str3 = new TorrentString();
                    TorrentString str4 = new TorrentString();
                    TorrentString str5 = new TorrentString();
                    TorrentString str6 = new TorrentString();
                    str2 = (TorrentString) dictionary["seed1"];
                    str3 = (TorrentString) dictionary["seed2"];
                    str4 = (TorrentString) dictionary["seed3"];
                    str5 = (TorrentString) dictionary["seed4"];
                    str6 = (TorrentString) dictionary["seed5"];
                    ArrayList list2 = new ArrayList();
                    TorrentList list3 = new TorrentList();
                    if (str2.Value != "")
                    {
                        returnarray.Add(str2);
                        count++;
                    }
                    if (str3.Value != "")
                    {
                        returnarray.Add(str3);
                        count++;
                    }
                    if (str4.Value != "")
                    {
                        returnarray.Add(str4);
                        count++;
                    }
                    if (str5.Value != "")
                    {
                        returnarray.Add(str5);
                        count++;
                    }
                    if (str6.Value != "")
                    {
                        returnarray.Add(str6);
                        count++;
                    }
                }
                if (TorrentBuild.GenerateVerbose)
                {
                    Interaction.MsgBox("Number of Webseeds loaded: " + Conversions.ToString(count), MsgBoxStyle.OkOnly, null);
                }
            }
            return count;
        }
    }
}

