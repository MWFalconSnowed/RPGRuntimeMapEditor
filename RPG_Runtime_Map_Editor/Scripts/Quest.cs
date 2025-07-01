using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName = "Réactiver le Cercle des Anciens";
    public string description = "Explore les ruines et trouve les 3 cristaux d’activation.";
    public bool isCompleted = false;

    public void CompleteQuest()
    {
        isCompleted = true;
        Debug.Log($"✨ Quête terminée : {questName}");
    }
}
