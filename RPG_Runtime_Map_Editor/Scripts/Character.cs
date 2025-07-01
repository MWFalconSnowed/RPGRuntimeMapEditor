using UnityEngine;

public class Character
{
    public string name;
    public int health;
    public int strength;
}

public class CharacterEditor : MonoBehaviour
{
    public Character character;

    public void SetCharacter(string name, int health, int strength)
    {
        character = new Character
        {
            name = name,
            health = health,
            strength = strength
        };
    }

    public void DisplayCharacterInfo()
    {
        Debug.Log($"Character Name: {character.name}, Health: {character.health}, Strength: {character.strength}");
    }
}
