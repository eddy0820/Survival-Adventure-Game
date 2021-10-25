using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalAdventureGame.Stats;

public class CharacterStat : MonoBehaviour
{
    [SerializeField] BaseStats baseStats;
    [SerializeField] bool testStats;
    Dictionary<string, Stat> characterStats = new Dictionary<string, Stat>();
    public Dictionary<string, Stat> CharacterStats => characterStats;
    int currentHealth;
    int currentStamina;
    public int CurrentHealth => currentHealth;
    public int CurrentStamina => currentStamina;
    Movement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<Movement>();

        InitializeCharacterStats();
        
        if(testStats)
        {
            DebugTestStats();
        }  
    }

    private void InitializeCharacterStats()
    {
        foreach(BaseStats.BaseStat baseStat in baseStats.Stats)
        {
            characterStats.Add(baseStat.StatType.Name, new Stat(baseStat.StatType, baseStat.Value));
        }

        currentHealth = (int)characterStats["MaxHealth"].value;
        currentStamina = (int)characterStats["MaxStamina"].value;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }

    // Reduces stamina by x amount when the player jumps.
    public virtual void ReduceStaminaJumping()
    {
        currentStamina -= (int)characterStats["JumpStaminaCost"].value;
    }

    public virtual IEnumerator ReduceStaminaSprinting()
    {
        playerMovement.startedStaminaReductionCoroutine = true;

        // While the player is holding down the sprint key, and has any amount of mana, and is not still, reduce stamina by x amount.
        while(playerMovement.isSprinting && playerMovement.startedStaminaReductionCoroutine && currentStamina > 0 && playerMovement.GetHorizontalInput() != new Vector2(0, 0))
        {
            yield return new WaitForSeconds(1f);

            currentStamina -= (int)(characterStats["SprintStaminaReductionRate"].value);
        }

        playerMovement.startedStaminaReductionCoroutine = false;
    }

    public virtual IEnumerator RegenStamina()
    {
        playerMovement.startedStaminaRegenCoroutine = true;
        
        // While the player's stamina does not equal the max stamina, 
        // and the player is not holding down the sprint key or is holding down the sprint key but isn't moving, regen stamina by x amount.
        while((!playerMovement.isSprinting || (playerMovement.isSprinting && playerMovement.GetHorizontalInput() == new Vector2(0, 0))) && playerMovement.startedStaminaRegenCoroutine && currentStamina != characterStats["MaxStamina"].value)
        {
            yield return new WaitForSeconds(1f);

            currentStamina += (int)(characterStats["StaminaRegen"].value);
        }

        playerMovement.startedStaminaRegenCoroutine = false;
    }

    private void DebugTestStats()
    {
        DebugPrintPlayerStats();

        StatModifier modifier = new StatModifier(10, StatModifierTypes.Flat);
        
        DebugTestAddModifiers(modifier);
        DebugPrintPlayerStats();

        DebugTestRemoveModifiers(modifier);
        DebugPrintPlayerStats();
    }  

    private void DebugPrintPlayerStats()
    {
        foreach(KeyValuePair<string, Stat> stat in characterStats)
        {
            Debug.Log(stat.Key + " value: " + stat.Value.value);
        }
    }

    private void DebugTestAddModifiers(StatModifier modifier)
    {
        foreach(KeyValuePair<string, Stat> stat in characterStats)
        {
            stat.Value.AddModifier(modifier);
        }
    }

    private void DebugTestRemoveModifiers(StatModifier modifier)
    {    
        foreach(KeyValuePair<string, Stat> stat in characterStats)
        {
            stat.Value.RemoveModifier(modifier);
        }
    }
}
