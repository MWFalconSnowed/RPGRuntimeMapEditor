using UnityEngine;
#if UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

public class StartupFix : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    // DLL import pour gérer la DPI Windows
    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    void Awake()
    {
        // Fix DPI Windows : rend le process DPI aware (évite le scaling automatique Windows)
        SetProcessDPIAware();

        // Forcer la résolution native 1920x1080 en fenêtre
        Screen.SetResolution(1920, 1080, false);

        // Optionnel : verrouiller le framerate pour stabilité (exemple 60 fps)
        Application.targetFrameRate = 60;

        Debug.Log("🚀 StartupFix appliqué : DPI aware & résolution 1920x1080 forcée");
    }
#else
    void Awake()
    {
        // Pour les autres plateformes, on force juste la résolution (windowed)
        Screen.SetResolution(1920, 1080, false);
        Application.targetFrameRate = 60;
        Debug.Log("🚀 StartupFix non-Windows : résolution 1920x1080 forcée");
    }
#endif
}
