using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Text;

enum Accessibility
{
    Public,
    Protected,
    Private
}

class ClassCodeEditor
{
    private StringBuilder m_codeBuffer = new StringBuilder();

    private const string m_lnBlank = "\n    ";

    public void AddUsingNamespace(string namespaceName)
    {
        m_codeBuffer.Append("using ");
        m_codeBuffer.Append(namespaceName);
        m_codeBuffer.Append(";\n");
    }

    public void AddClass(Accessibility accessibility, string className)
    {
        var accessibilityName = accessibility switch
        {
            Accessibility.Private => "private",
            Accessibility.Protected => "protected",
            Accessibility.Public => "public",
            _ => ""
        };

        m_codeBuffer.Append(accessibilityName);
        m_codeBuffer.Append(" class ");
        m_codeBuffer.Append(className);
        m_codeBuffer.Append("\n{");
        m_codeBuffer.Append(m_lnBlank);
    }

    public void Append(string code)
    {
        code = code.Replace("\n", m_lnBlank);
        m_codeBuffer.Append(code);

        m_codeBuffer.Append(m_lnBlank);
    }

    public static implicit operator string(in ClassCodeEditor classCodeEditor)
    {
        return classCodeEditor.m_codeBuffer.ToString();
    }

    public void AddLine()
    {
        m_codeBuffer.Append("\n");
    }

    public void Clear()
    {
        m_codeBuffer.Clear();
    }

    public void Finish()
    {
        m_codeBuffer.Append("\n}");
    }

    public override string ToString()
    {
        return this;
    }
}

public class AnimatorControllerTableEditor : EditorWindow
{
    private AnimatorController m_animatorController;

    private string m_folderPath;

    private Stack<string> m_statePathStack = new Stack<string>();

    private string m_layerName = "";

    [MenuItem("Window/AnimatorControllerTable")]
    static void Open()
    {
        var window = GetWindowWithRect<AnimatorControllerTableEditor>(new Rect(0, 0, 300, 100));
        window.Show();
    }

    private void OnGUI()
    {
        m_animatorController = (AnimatorController)EditorGUILayout.ObjectField(m_animatorController, typeof(AnimatorController), true);

        if (GUILayout.Button("フォルダを選択"))
        {
            m_folderPath = EditorUtility.OpenFolderPanel("セーブフォルダを選択", Application.dataPath, string.Empty);
        }

        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.TextField("フォルダのパス", m_folderPath);

        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(!m_animatorController || m_folderPath == null || m_folderPath == "");

        bool isCreateTable = GUILayout.Button("テーブルを作成");

        EditorGUI.EndDisabledGroup();

        if (isCreateTable)
        {
            CreateAnimatorControllerTable();

            AssetDatabase.Refresh();
        }
    }

    private void CreateAnimatorControllerTable()
    {
        var fileName = m_animatorController.name + "Table.cs";
        var fullPath = Path.Combine(m_folderPath, fileName);

        Debug.Log(fullPath);

        ClassCodeEditor classCodeEditor = new ClassCodeEditor();

        classCodeEditor.AddUsingNamespace("System");
        classCodeEditor.AddUsingNamespace("UnityEngine");
        classCodeEditor.AddLine();

        string className = m_animatorController.name + "Table";

        classCodeEditor.AddClass(Accessibility.Public, className);

        foreach (var layer in m_animatorController.layers)
        {
            m_layerName = layer.name;

            string layerName = layer.name.Replace(" ", "");

            string layerClassName = layerName + "Table";

            classCodeEditor.Append($"public static readonly {layerClassName} {layerName} = new {layerClassName}();");


            classCodeEditor.Append(CreateStateMachine(layer.stateMachine));
        }

        classCodeEditor.Finish();

        File.WriteAllText(m_folderPath + "/" + m_animatorController.name + "Table.cs", classCodeEditor);
    }

    ClassCodeEditor CreateStateMachine(AnimatorStateMachine stateMachine)
    {
        m_statePathStack.Push(stateMachine.name);

        string stateMachineClassname = stateMachine.name + "Table";

        stateMachineClassname = stateMachineClassname.Replace(" ", "");

        ClassCodeEditor classCodeEditor = new ClassCodeEditor();

        classCodeEditor.AddClass(Accessibility.Public, stateMachineClassname);

        foreach(var subMachine in stateMachine.stateMachines)
        {
            string subMachineClassName = subMachine.stateMachine.name + "Table";

            classCodeEditor.Append($"public readonly {subMachineClassName} {subMachine.stateMachine.name} = new {subMachineClassName}();");

            classCodeEditor.Append(CreateStateMachine(subMachine.stateMachine));
        }

        foreach(var state in stateMachine.states)
        {
            var stackArray = m_statePathStack.ToArray();
            System.Array.Reverse(stackArray);

            string fullPath = GetAppendPath(stackArray);
            
            fullPath = GetAppendPath(fullPath, state.state.name);

            classCodeEditor.Append($"public readonly AnimationState {state.state.name} = new AnimationState(\"{fullPath}\",\"{m_layerName}\");");
        }
        
        classCodeEditor.Finish();

        m_statePathStack.Pop();

        return classCodeEditor;
    }

    private static string GetAppendPath(params string[] paths)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var path in paths)
        {
            stringBuilder.Append(path);

            if (path == paths[paths.Length - 1])
            {
                break;
            }

            stringBuilder.Append(".");
        }

        var appendPath = stringBuilder.ToString();

        stringBuilder.Clear();

        return appendPath;
    }

}
