using System;
using UnityEditor;
using UnityEngine;
using System.Text;

public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    // Rect for the title of the node 
    public Rect rectID;

    // Two Rect for the unlock field (1 for the label and other for the checkbox)
    public Rect rectUnlockLabel;
    public Rect rectUnlocked;

    // Two Rect for the cost field (1 for the label and other for the text field)
    public Rect rectCostLabel;
    public Rect rectCost;

    public Rect rectTittleLabel;
    public Rect rectTittle;

    public Rect rectDescriptionLabel;
    public Rect rectDescription;

    public Rect rectSkillTypeLabel;
    public Rect rectSkillType;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    // GUI Style for the title
    public GUIStyle styleID;

    // GUI Style for the fields
    public GUIStyle styleField;

    public Action<Node> OnRemoveNode;

    // Skill linked with the node
    public Skill skill;

    // Bool for checking if the node is whether unlocked or not
    private bool unlocked = false;

    // StringBuilder to create the node's title
    private StringBuilder nodeTitle;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle,
        GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
        Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,
        Action<Node> OnClickRemoveNode, int id, bool unlocked, int cost, int[] dependencies, string tittle, string description, SkillType skillType)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In,
            inPointStyle, OnClickInPoint);

        outPoint = new ConnectionPoint(this, ConnectionPointType.Out,
            outPointStyle, OnClickOutPoint);


        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        // Create new Rect and GUIStyle for our title and custom fields
        float rowHeight = height / 20;

        rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
        styleID = new GUIStyle();
        styleID.alignment = TextAnchor.UpperCenter;

        rectUnlocked = new Rect(position.x + width / 3,
            position.y + 3 * rowHeight, width / 3, rowHeight);

        rectUnlockLabel = new Rect(position.x,
            position.y + 3 * rowHeight, width / 3, rowHeight);

        styleField = new GUIStyle();
        styleField.alignment = TextAnchor.UpperRight;

        rectCostLabel = new Rect(position.x,
            position.y + 5 * rowHeight, width / 3, rowHeight);

        rectCost = new Rect(position.x + width / 3,
            position.y + 5 * rowHeight, 20, rowHeight);

        rectTittleLabel = new Rect(position.x,
            position.y + 7 * rowHeight, width / 3, rowHeight);

        rectTittle = new Rect(position.x + width / 3,
            position.y + 7 * rowHeight, 120, rowHeight);

        rectDescriptionLabel = new Rect(position.x,
            position.y + 9 * rowHeight, width / 3, rowHeight);

        rectDescription = new Rect(position.x + width / 3,
            position.y + 9 * rowHeight, 140, rowHeight + 100);

        rectSkillTypeLabel = new Rect(position.x,
            position.y + 16 * rowHeight, width / 3, rowHeight);

        rectSkillType = new Rect(position.x + width / 3,
            position.y + 16 * rowHeight, 120, rowHeight + 10);

        this.unlocked = unlocked;

        // We create the skill with current node info
        skill = new Skill();
        skill.skillID = id;
        skill.unlocked = unlocked;
        skill.cost = cost;
        skill.skillDependencies = dependencies;
        skill.skillTittle = tittle;
        skill.skillDescription = description;
        skill.skillType = skillType;

        // Create string with ID info
        nodeTitle = new StringBuilder();
        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        rectID.position += delta;
        rectUnlocked.position += delta;
        rectUnlockLabel.position += delta;
        rectCost.position += delta;
        rectCostLabel.position += delta;
        rectTittle.position += delta;
        rectTittleLabel.position += delta;
        rectDescription.position += delta;
        rectDescriptionLabel.position += delta;
        rectSkillType.position += delta;
        rectSkillTypeLabel.position += delta;
    }

    public void MoveTo(Vector2 pos)
    {
        rect.position = pos;
        rectID.position = pos;
        rectUnlocked.position = pos;
        rectUnlockLabel.position = pos;
        rectCost.position = pos;
        rectCostLabel.position = pos;
        rectTittle.position += pos;
        rectTittleLabel.position += pos;
        rectDescription.position += pos;
        rectDescriptionLabel.position += pos;
        rectSkillType.position += pos;
        rectSkillTypeLabel.position += pos;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);

        // Print the title
        GUI.Label(rectID, nodeTitle.ToString(), styleID);

        // Print the unlock field
        GUI.Label(rectUnlockLabel, "Unlocked: ", styleField);
        if (GUI.Toggle(rectUnlocked, unlocked, ""))
            unlocked = true;
        else
            unlocked = false;

        skill.unlocked = unlocked;

        // Print the cost field
        GUI.Label(rectCostLabel, "Cost: ", styleField);
        skill.cost = int.Parse(GUI.TextField(rectCost, skill.cost.ToString()));

        // Print the tittle field
        GUI.Label(rectTittleLabel, "Tiitle: ", styleField);
        skill.skillTittle = GUI.TextArea(rectTittle, skill.skillTittle.ToString());

        // Print the desciption field
        GUI.Label(rectDescriptionLabel, "Description: ", styleField);
        skill.skillDescription = GUI.TextArea(rectDescription, skill.skillDescription.ToString());

        // Print the type field
        GUI.Label(rectSkillTypeLabel, "Type: ", styleField);
        skill.skillType = (SkillType)EditorGUI.EnumPopup(rectSkillType, skill.skillType);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}