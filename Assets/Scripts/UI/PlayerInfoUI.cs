using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text playerNameText;
    public Text playerLevelText;

    [Header("Player Stats Reference")]
    public PlayerStats playerStats;

    private void Update()
    {
        if (playerStats != null)
        {
            if (playerNameText != null)
            {
                playerNameText.text = playerStats.playerName;
            }
            if (playerLevelText != null)
            {
                playerLevelText.text = "Level " + playerStats.level.ToString();
            }
        }
        else
        {
            Debug.LogWarning("PlayerInfoUI: PlayerStats reference is not set.");
        }
    }
}
