using SFB; // StandaloneFileBrowser
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public PolygonEditorRuntimeGUI editor;

    public void SelectAndLoadMap()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Select Map", "", extensions, false);
        if (paths.Length > 0)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(paths[0]);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            editor.mapTexture = tex;
        }
    }
}
