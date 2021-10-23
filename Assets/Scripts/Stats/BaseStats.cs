using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalAdventureGame.Stats
{ 

[CreateAssetMenu(fileName = "New Base Stats", menuName = "Stats/Base Stats")]
public class BaseStats : ScriptableObject
{
    [SerializeField] List<BaseStat> stats = new List<BaseStat>();

    public List <BaseStat> Stats => stats;

    [Serializable]
    public class BaseStat
    {
        [SerializeField] StatType statType = null;
        [SerializeField] float value;

        public StatType StatType => statType;

        public float Value => value;
    }
}

}
