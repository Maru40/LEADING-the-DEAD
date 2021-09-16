using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeBase<NodeType>
    where NodeType : class
{
    NodeType m_owner;

    public NodeBase(NodeType owner)
    {
        m_owner = owner;
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public NodeType GetOwner()
    {
        return m_owner;
    }
}
