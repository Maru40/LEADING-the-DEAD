using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class MissionData : ScriptableObject
{
    public const string MENU_PATH = "CreateScriptable/MissionData";

    public const string VALUE_REPLACE_TEXT = "[value]";

    [Header("値に置き換える場所は [value] と書く")]
    [SerializeField]
    private string m_regexText;

    protected string regexText => m_regexText;

    public abstract bool IsMissionClear(Player.PlayerStatusManager playerStatusManager, EnemyGenerator enemyGenerator);

    public abstract string GetExplanationText();
}
