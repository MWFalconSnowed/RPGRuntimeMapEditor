using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public string[] dialogueOptions = {
        "Les ruines chantent une mélodie oubliée...",
        "Ce portail semble murmurer ton nom.",
        "Fais attention... certaines fleurs veillent sur ce lieu."
    };

    private int currentOption = 0;

    void OnGUI()
    {
        GUI.skin.button.fontSize = 20;
        GUI.skin.button.wordWrap = true;

        Rect rect = new Rect(20, Screen.height - 100, Screen.width - 40, 80);
        if (GUI.Button(rect, dialogueOptions[currentOption]))
        {
            currentOption = (currentOption + 1) % dialogueOptions.Length;
        }
    }
}
