using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class PauseManager
{
    private static bool isPaused = false;

    public static bool IsPaused()
    {
        return isPaused;
    }

    public static void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
}