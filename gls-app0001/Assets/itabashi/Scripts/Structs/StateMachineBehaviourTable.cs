using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviourTable<T> where T : StateMachineBehaviour
{
    private Animator m_animator;

    Dictionary<string, T> m_stateMachineBehaviourTable = new Dictionary<string, T>();

    static System.Text.StringBuilder m_stringBuilder = new System.Text.StringBuilder();

    private Stack<string> m_pathStack = new Stack<string>();

    private int m_nowLayerIndex;

    public StateMachineBehaviourTable(Animator animator)
    {
        m_animator = animator;

        var controller = m_animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        
        if(!controller)
        {
            return;
        }

        foreach(var layer in controller.layers)
        {
            m_nowLayerIndex = m_animator.GetLayerIndex(layer.name);

            StateMachineCheck(layer.stateMachine);
        }
    }

    private void StateMachineCheck(UnityEditor.Animations.AnimatorStateMachine stateMachine)
    {
        m_pathStack.Push(stateMachine.name);

        foreach(var childStateMachine in stateMachine.stateMachines)
        {
            StateMachineCheck(childStateMachine.stateMachine);
        }

        var stackArray = m_pathStack.ToArray();
        System.Array.Reverse(stackArray);

        string stateMachinePath = GetAppendPath(stackArray);

        PushBehabiourTable(stateMachinePath, m_nowLayerIndex);

        foreach (var state in stateMachine.states)
        {
            var stateFullPath = GetAppendPath(stateMachinePath, state.state.name);

            PushBehabiourTable(stateFullPath, m_nowLayerIndex);
        }

        m_pathStack.Pop();
    }

    private void PushBehabiourTable(string path,int layerIndex)
    {
        var ary = m_animator.GetBehaviours(Animator.StringToHash(path), layerIndex);

        foreach (var behaviour in ary)
        {
            T t = behaviour as T;
            if (t)
            {
                m_stateMachineBehaviourTable.Add(path, t);
                break;
            }
        }
    }

    public T this[string fullPath]
    {
        get
        {
            return m_stateMachineBehaviourTable.ContainsKey(fullPath) ? m_stateMachineBehaviourTable[fullPath] : null;
        }
    }

    private static string GetAppendPath(params string[] paths)
    {
        m_stringBuilder.Clear();

        foreach(var path in paths)
        {
            m_stringBuilder.Append(path);

            if (path == paths[paths.Length - 1])
            {
                break;
            }

            m_stringBuilder.Append(".");
        }

        var appendPath = m_stringBuilder.ToString();

        m_stringBuilder.Clear();

        return appendPath;
    }
}
