using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Demo_ScreenshotToClipboard : UI_Demo_LoadClipboardToRawImage
{
    public float m_timeBeforePushScreenshot = 1;
    public bool m_useImageCheck;
    public float m_timeBeforeImageCheck = 3;

    string m_imagePathUsed;
    public void SaveScreenShotToClipboard()
    {
        m_clipboardJar.TakeScreenshot(out m_imagePathUsed);

        Invoke("LoadPushed", m_timeBeforePushScreenshot);
        if (m_useImageCheck)
            Invoke("LoadPushedImage", m_timeBeforeImageCheck);
    }
    public void LoadPushed()
    {
        m_clipboardJar.PushImageFromPathToClipboard(m_imagePathUsed);
    }
    public void LoadPushedImage()
    {
        LoadImageFromClipboard();
    }


}
