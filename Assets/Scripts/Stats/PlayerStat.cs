using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalAdventureGame.Stats;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] BaseStats baseStats;
    [SerializeField] bool testStats;
    Dictionary<string, Stat> playerStats = new Dictionary<string, Stat>();
    public Dictionary<string, Stat> PlayerStats => playerStats;

    private void Awake()
    {
        InitializePlayerStats();
        
        if(testStats)
        {
            TestStats();
        }  
    }
    
    private void InitializePlayerStats()
    {
        foreach(BaseStats.BaseStat baseStat in baseStats.Stats)
        {
            playerStats.Add(baseStat.StatType.Name, new Stat(baseStat.StatType, baseStat.Value));
        }
    }

    private void TestStats()
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
        foreach(KeyValuePair<string, Stat> stat in playerStats)
        {
            Debug.Log(stat.Key + " value: " + stat.Value.value);
        }
    }

    private void DebugTestAddModifiers(StatModifier modifier)
    {
        foreach(KeyValuePair<string, Stat> stat in playerStats)
        {
            stat.Value.AddModifier(modifier);
        }
    }

    private void DebugTestRemoveModifiers(StatModifier modifier)
    {    
        foreach(KeyValuePair<string, Stat> stat in playerStats)
        {
            stat.Value.RemoveModifier(modifier);
        }
    }
}
