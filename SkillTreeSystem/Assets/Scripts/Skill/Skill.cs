[System.Serializable]
public class Skill
{
    public int skillID;
    public int[] skillDependencies;
    public bool unlocked;
    public int cost;
    public string skillTittle;
    public string skillDescription;
    public SkillType skillType;
}

public enum SkillType
{
    BoostAttributes,
    UnlockMap,
    UnlockMission,
    UnlockItem
}