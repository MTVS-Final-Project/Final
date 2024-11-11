using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerApp : MonoBehaviourPun
{
    public List<Sprite> playerSprites; // All sprites for different parts
    private SpriteRenderer spriteRenderer;

    // Dictionary to store selected sprite indices for each part
    public Dictionary<string, int> selectedSpriteIndices = new Dictionary<string, int>
    {
        {"Body", 0},
        {"Eyes", 0},
        {"Mouth", 0},
        {"LeftArm", 0},
        {"RightArm", 0},
        {"leftLeg", 0 },
        {"rightLeg", 0 },
        {"leftShoes", 0 },
        {"rightShoes", 0 },
        {"Hair", 0 },
        {"Pants", 0 },
    };

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (photonView.IsMine)
        {
            LoadSpriteSelections();
        }
    }

    public void ChangeSprite(string part, int newSpriteIndex)
    {
        if (photonView.IsMine)
        {
            // Update the selected index for the specified part
            selectedSpriteIndices[part] = newSpriteIndex;

            // Save the new selection and update across the network
            SaveSpriteSelection(part);
            photonView.RPC("UpdateSpriteForAllPlayers", RpcTarget.AllBuffered, part, newSpriteIndex);
        }
    }

    [PunRPC]
    void UpdateSpriteForAllPlayers(string part, int spriteIndex)
    {
        if (selectedSpriteIndices.ContainsKey(part) && spriteIndex >= 0 && spriteIndex < playerSprites.Count)
        {
            selectedSpriteIndices[part] = spriteIndex;
            spriteRenderer.sprite = playerSprites[spriteIndex];
            Debug.Log($"Updated {part} sprite across network: " + playerSprites[spriteIndex].name);
        }
        else
        {
            Debug.LogWarning($"Invalid sprite index received for {part}.");
        }
    }

    private void SaveSpriteSelection(string part)
    {
        // Save the selected sprite index for the specified part
        PlayerPrefs.SetInt("SelectedSpriteIndex_" + part, selectedSpriteIndices[part]);
    }

    private void LoadSpriteSelections()
    {
        // Load each part's saved selection
        foreach (var part in selectedSpriteIndices.Keys)
        {
            selectedSpriteIndices[part] = PlayerPrefs.GetInt("SelectedSpriteIndex_" + part, 0);
            // Apply the loaded sprite index
            ChangeSprite(part, selectedSpriteIndices[part]);
        }
    }
}
