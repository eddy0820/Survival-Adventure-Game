using UnityEngine;

namespace SurvivalAdventureGame.Stats
{ 

[CreateAssetMenu(fileName = "New Stat Type", menuName = "Stats/Stat Type")]
public class StatType : ScriptableObject
{
    [SerializeField] new string name = "New Stat Type Name";
    [SerializeField] float defaultValue = 0f;
    //Stat stat;
    public string Name => name;
    public float DefaultValue => defaultValue;
    //public Stat Stat => stat = new Stat(this);
}

}
