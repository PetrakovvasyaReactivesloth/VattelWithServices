using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundlesEditor
{
    private const string ASSET_BUNDLES_FOLDER_NAME = "AssetBundles";
    private const string CRC_CODES_FILE_NAME = "CRCCodes.txt";
    private const string VERSIONS_CODES_FILE_NAME = "BundleVersions.txt";
    private const string FILE_ELEMENTS_IN_LINE_DIVIDER = ",";
    private const string NEW_LINE_CODE = "\n";


    [MenuItem("AssetBundles/Clean all cache")]
    public static void CleanAllCache()
    {
        Debug.Log(Caching.ClearCache() ? "Cache cleaned successfully" : "Can't clean cache");
    }

    [MenuItem("AssetBundles/Generate CRC values file")]
    public static void GenerateCRCFile()
    {
        var dataPath = Application.dataPath;
        var allPlatformsBundlesFolderPath = dataPath.Replace(dataPath.Split('/').Last(), ASSET_BUNDLES_FOLDER_NAME);
        string[] bundlesDirectoryFolders = Directory.GetDirectories(allPlatformsBundlesFolderPath);
       
        foreach (var bundlesDirectoryFolder in bundlesDirectoryFolders)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(bundlesDirectoryFolder);
            FileInfo[] filesInfo = directoryInfo.GetFiles("*.manifest");
            List<string> bundleNamesInDirectory = new List<string>();
            
            foreach (FileInfo file in filesInfo)
            {
                string name = file.Name.Replace(".manifest", string.Empty);
                
                if (name != bundlesDirectoryFolder.Split('\\').Last())
                {
                    bundleNamesInDirectory.Add(name);
                }
            }

            string crcCodesFilePath = bundlesDirectoryFolder + "/" + CRC_CODES_FILE_NAME;
            File.WriteAllText(crcCodesFilePath, string.Empty);

            for (int i = 0; i < bundleNamesInDirectory.Count; i++)
            {
                var bundleName = bundleNamesInDirectory[i];
                uint crc;
                string fileToBuild = (bundlesDirectoryFolder + "/" + bundleName);

                BuildPipeline.GetCRCForAssetBundle(fileToBuild, out crc);

                using (StreamWriter sw = new StreamWriter(crcCodesFilePath, true))
                {
                    string additiveString = (i == bundleNamesInDirectory.Count - 1) ? string.Empty : NEW_LINE_CODE;

                    sw.Write(bundleName + FILE_ELEMENTS_IN_LINE_DIVIDER + crc + additiveString);
                }
            }
        }

        Debug.Log("CRC codes generated successfully");
    }

    //[MenuItem("AssetBundles/Generate versions file")]
    //public void GenerateVersionsFile()
    //{
    //    //var dataPath = Application.dataPath;
    //    //var allPlatformsBundlesFolderPath = dataPath.Replace(dataPath.Split('/').Last(), ASSET_BUNDLES_FOLDER_NAME);

    //    //using (StreamWriter sw = new StreamWriter(allPlatformsBundlesFolderPath + "/" + VERSIONS_CODES_FILE_NAME, true))
    //    //{
    //    //    string additiveString = (i == bundleNamesInDirectory.Count - 1) ? string.Empty : NEW_LINE_CODE;

    //    //    sw.Write(bundleName + FILE_ELEMENTS_IN_LINE_DIVIDER + crc + additiveString);
    //    //}
    //}

    [MenuItem("AssetBundles/Clean player prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player prefs cleaned successfully");
    }

    private string GetServerLink()
    {
        return "https://reactivesloth.com/dlc/mel/";
    }
}