using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkillTreeReader : MonoBehaviour
{
    private static SkillTreeReader _instance;
    public static SkillTreeReader Instance { get { return _instance; } set {} }

    // Array with all the skills in our skilltree
    private Skill[] _skillTree;

    // Dictionary with the skills in our skilltree
    private Dictionary<int, Skill> _skills;

    // Variable for caching the currently being inspected skill
    private Skill _skillInspected;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            SetUpSkillTree();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization of the skill tree
    void SetUpSkillTree()
    {
        _skills = new Dictionary<int, Skill>();

        if (PlayerPrefs.GetString("SkillTree", "").CompareTo("") == 0)
        {
            LoadSkillTree();
        }
        else
        {
            LoadPlayerSkillTree();
        }


    }

    public void LoadSkillTree()
    {
        string path = "Assets/Scripts/Data/skilltree.json";
        string dataAsJson;
        if (File.Exists(path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

            // Store the SkillTree as an array of Skill
            _skillTree = new Skill[loadedData.skilltree.Length];
            _skillTree = loadedData.skilltree;

            // Populate a dictionary with the skill id and the skill data itself
            for (int i = 0; i < _skillTree.Length; ++i)
            {
                _skills.Add(_skillTree[i].skillID, _skillTree[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    public void LoadPlayerSkillTree()
    {
        string dataAsJson = PlayerPrefs.GetString("SkillTree");

        // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
        SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

        // Store the SkillTree as an array of Skill
        _skillTree = new Skill[loadedData.skilltree.Length];
        _skillTree = loadedData.skilltree;

        // Populate a dictionary with the skill id and the skill data itself
        for (int i = 0; i < _skillTree.Length; ++i)
        {
            _skills.Add(_skillTree[i].skillID, _skillTree[i]);
        }
    }

    public void SaveSkillTree()
    {
        // We fill with as many skills as nodes we have
        SkillTree skillTree = new SkillTree();
        skillTree.skilltree = new Skill[_skillTree.Length];
        for (int i = 0; i < _skillTree.Length; ++i)
        {
            _skills.TryGetValue(_skillTree[i].skillID, out _skillInspected);
            if (_skillInspected != null)
            {
                skillTree.skilltree[i] = _skillInspected;
            }
        }

        string json = JsonUtility.ToJson(skillTree);

        PlayerPrefs.SetString("SkillTree", json);
    }

    public bool IsSkillUnlocked(int id_skill)
    {
        if (_skills.TryGetValue(id_skill, out _skillInspected))
        {
            return _skillInspected.unlocked;
        }
        else
        {
            return false;
        }
    }

    public bool CanSkillBeUnlocked(int id_skill)
    {
        bool canUnlock = true;
        if (_skills.TryGetValue(id_skill, out _skillInspected)) // The skill exists
        {
            if (_skillInspected.cost <= PlayerAttributes._points) // Enough points available
            {
                int[] dependencies = _skillInspected.skillDependencies;
                for (int i = 0; i < dependencies.Length; ++i)
                {
                    if (_skills.TryGetValue(dependencies[i], out _skillInspected))
                    {
                        if (!_skillInspected.unlocked)
                        {
                            canUnlock = false;
                            break;
                        }
                    }
                    else // If one of the dependencies doesn't exist, the skill can't be unlocked.
                    {
                        return false;
                    }
                }
            }
            else // If the player doesn't have enough skill points, can't unlock the new skill
            {
                return false;
            }

        }
        else // If the skill id doesn't exist, the skill can't be unlocked
        {
            return false;
        }
        return canUnlock;
    }

    public bool UnlockSkill(int id_Skill)
    {
        if (_skills.TryGetValue(id_Skill, out _skillInspected))
        {
            if (_skillInspected.cost <= PlayerAttributes._points)
            {
                PlayerAttributes._points -= _skillInspected.cost;
                _skillInspected.unlocked = true;

                // We replace the entry on the dictionary with the new one (already unlocked)
                _skills.Remove(id_Skill);
                _skills.Add(id_Skill, _skillInspected);
                return true;
            }
            else
            {
                return false;   // The skill can't be unlocked. Not enough points
            }
        }
        else
        {
            return false;   // The skill doesn't exist
        }
    }

    public void SetText(int id_Skill, ref string tittle, ref string description, ref int cost)
    {
        if (_skills.TryGetValue(id_Skill, out _skillInspected))
        {
            tittle = _skillInspected.skillTittle;
            description = _skillInspected.skillDescription;
            cost = _skillInspected.cost;
        }
    }
}