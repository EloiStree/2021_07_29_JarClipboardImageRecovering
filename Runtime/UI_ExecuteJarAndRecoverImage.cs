using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class UI_ExecuteJarAndRecoverImage : MonoBehaviour
{

    public TextAsset m_jarFile;
    public string m_pathFolderDefaultToUse = "";
    public string m_pathFolderSpecific = "";

   

    public string m_jarName = "Clipboard2File.jar";
    public string m_defaultImageFileName = "default.png";


    public void SetSpecificJarPath(string path) {

        if (File.Exists(path))
        {
            m_pathFolderSpecific = Path.GetDirectoryName(path);
            m_jarName = Path.GetFileName(path);
        }
        else m_pathFolderSpecific = path;



    }


    [ContextMenu("Open Asset Steam Folder")]
    public void OpenSteamAssetFolder() {
        UnityEngine.Debug.Log(Application.streamingAssetsPath);
        Application.OpenURL(Application.streamingAssetsPath);
    
    }

    [ContextMenu("Execute Jar")]
    public void FetchCurrentClipboardAsFileWithNowTitle( out RecoverClipboardImageCallback callback)
    {
        callback = new RecoverClipboardImageCallback( DateTime.Now.ToString("yyyy_mm_dd_HH_mm_ss") + ".png", GetJarPath());
        callback.ExecuteTheJarToFetchClipboardAsFile();
    }
    public void FetchCurrentClipboardAsDefaultTitle(out RecoverClipboardImageCallback defaultCallback)
    {
        defaultCallback = new RecoverClipboardImageCallback(m_defaultImageFileName, GetJarPath());
        defaultCallback.ExecuteTheJarToFetchClipboardAsFile();
    }





   
    public  string GetJarPath() {

        return GetDirectoryPath() +"\\"+ m_jarName;
    }

    private  string GetDirectoryPath()
    {
        if (m_pathFolderSpecific.Length > 0)
            return m_pathFolderSpecific;
        return Directory.GetCurrentDirectory() + "/" + m_pathFolderDefaultToUse;
    }
   
    public void GetScreenshotPathOfUnityFor(string nameWithExtension, out string path) {
        path= Application.persistentDataPath + "\\" + nameWithExtension;
    }
    public void TakeScreenshot(out string imagePath, string fileNameUse="screenshot.png")
    {
        GetScreenshotPathOfUnityFor(fileNameUse, out imagePath);
        ScreenCapture.CaptureScreenshot(imagePath);
    }
    public void PushImageFromPathToClipboard(string path) {

        RecoverClipboardImageCallback callback = new RecoverClipboardImageCallback(path , GetJarPath());
        callback.ExecuteTheJarToPushImageToClipboard();
    }


    public void OpenJavaDirectory() {
        Application.OpenURL(Path.GetDirectoryName(GetJarPath()));
    }
}

public class RecoverClipboardImageCallback
{


    [SerializeField] string m_givenImageFilePath;
    [SerializeField] string m_jarFilePath;
    public RecoverClipboardImageCallback()
    {
        m_givenImageFilePath = "default.png";
        m_jarFilePath = "Clipboard2File.jar";
    }

    public RecoverClipboardImageCallback(string givenImageFilePath, string jarFilePath)
    {
        m_givenImageFilePath = givenImageFilePath;
        m_jarFilePath = jarFilePath;
    }

    //java -jar Clipboard2File.jar push "rick.jpg"

    public void ExecuteTheJarToPushImageToClipboard()
    {
        List<string> cmds = new List<string>();
        GetImageAsAbsolutePath(out string imagePath);
        cmds.Add(string.Format("cd \"{0}\"", Path.GetDirectoryName(m_jarFilePath)));
        cmds.Add(string.Format("java -jar {1} push \"{0}\" ", imagePath, Path.GetFileName(m_jarFilePath)));
        RunCommands(cmds, Path.GetDirectoryName(m_jarFilePath));
    }

    public void ExecuteTheJarToFetchClipboardAsFile()
    {
        DeleteExistingImage();
        GetImageAsAbsolutePath(out string imagePath);
        List<string> cmds = new List<string>();
        cmds.Add(string.Format("cd \"{0}\"", Path.GetDirectoryName(m_jarFilePath)));
        cmds.Add(string.Format("java -jar {1} pull \"{0}\" ", imagePath, Path.GetFileName(m_jarFilePath)));
        RunCommands(cmds, Path.GetDirectoryName(m_jarFilePath));
    }
    public void DeleteExistingImage()
    {
        GetImageAsAbsolutePath(out string path);
        if (File.Exists(path))
            File.Delete(path);
    }
    public void GetImageAsAbsolutePath(out string path) {
        if (Path.IsPathRooted(m_givenImageFilePath))
            path = m_givenImageFilePath;
        else path =  Path.GetDirectoryName(m_jarFilePath) + "\\" + m_givenImageFilePath;
    }
    public bool WasImageCreated() {
        GetImageAsAbsolutePath(out string path);
        return File.Exists(path); 
    }
    public bool DoesJarFileExist() { return File.Exists(m_jarFilePath); }

    static void RunCommands(List<string> cmds, string workingDirectory = "")
    {
        var process = new Process();
        var psi = new ProcessStartInfo();
        psi.FileName = "cmd.exe";
        psi.RedirectStandardInput = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.WorkingDirectory = workingDirectory;
        process.StartInfo = psi;
        process.Start();
        process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
        process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        using (StreamWriter sw = process.StandardInput)
        {
            foreach (var cmd in cmds)
            {
                sw.WriteLine(cmd);
            }
        }
        process.WaitForExit();
    }

    internal void GetAsTexture(out Texture2D texture)
    {
        GetImageAsAbsolutePath(out string imgPath);
        texture = LoadPNG(imgPath);
    }
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); 
        }
        return tex;
    }
}
