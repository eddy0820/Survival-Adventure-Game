using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIController : MonoBehaviour
{
    CharacterStat playerStats;
    VisualElement rootElement;
    VisualElement healthBarFill;
    VisualElement staminaBarFill;
    Label healthText;
    Label staminaText;

    private void Awake()
    { 
        playerStats = GetComponentInParent<CharacterStat>();

        rootElement = GetComponent<UIDocument>().rootVisualElement;
    
        healthBarFill = rootElement.Q<VisualElement>("health-bar-fill");
        staminaBarFill = rootElement.Q<VisualElement>("stamina-bar-fill");  

        healthText = rootElement.Q<Label>("health-text");
        staminaText = rootElement.Q<Label>("stamina-text");
    }

    private void Update()
    {
        UpdateHealthAndStaminaBars();
    }

    private void UpdateHealthAndStaminaBars()
    {
        if(rootElement.visible)
        {
            healthBarFill.style.width = (playerStats.CurrentHealth * 122) / playerStats.CharacterStats["MaxHealth"].value;
            staminaBarFill.style.width = (playerStats.CurrentStamina * 122) / playerStats.CharacterStats["MaxStamina"].value;

            healthText.text = "Health: " + playerStats.CurrentHealth + "/" + playerStats.CharacterStats["MaxHealth"].value;
            staminaText.text = "Stamina: " + playerStats.CurrentStamina + "/" + playerStats.CharacterStats["MaxStamina"].value;
        } 
    }

    //When pressing the ToggleHUD key.
    public void OnToggleHUDPressed()
    {
        rootElement.visible = !rootElement.visible;
    }
}
