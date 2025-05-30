// Assets/Scripts/PlayerStats.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public string playerName = "Hero";
    public int maxHP = 100;
    public int currentHP = 100;
    public int collectedGems = 0; // Tracks Emotional Gems
}