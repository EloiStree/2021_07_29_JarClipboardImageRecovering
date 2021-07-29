using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Demo_LoadClipboardToRawImage : MonoBehaviour
{

    public UI_ExecuteJarAndRecoverImage m_clipboardJar;
    public RawImage m_debugDisplay;
    public AspectRatioFitter m_ratioFilter;
    public bool m_useDateAsName;
    public float m_timeBeforeDownloadCheck=2f;
    RecoverClipboardImageCallback callback;
    public void LoadImageFromClipboard()
    {
        if(m_useDateAsName)
            m_clipboardJar.FetchCurrentClipboardAsFileWithNowTitle(out callback);
        else 
            m_clipboardJar.FetchCurrentClipboardAsDefaultTitle(out callback);
        Invoke("CheckIfDownloaded", m_timeBeforeDownloadCheck);
    }
    public void CheckIfDownloaded()
    {
        callback.GetImageAsAbsolutePath(out string path);
        if (callback.WasImageCreated())
        {
            callback.GetAsTexture(out Texture2D texture);
            if (texture != null) { 
                m_debugDisplay.texture = texture;   
                m_ratioFilter.aspectRatio = (float)texture.width / (float)texture.height;
            }
        }
        else m_debugDisplay.texture = null;
    }
}
