using UnityEngine;

public class ScreenModeController : MonoBehaviour
{
    public void SetFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetFullscreenBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}