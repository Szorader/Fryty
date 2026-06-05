using UnityEngine;

public class ScreenModeController : MonoBehaviour
{
    // maximized window
    public void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    // fullscreen alt-tab freindly
    public void SetBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    // fullscreen
    public void SetFullscreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }
}