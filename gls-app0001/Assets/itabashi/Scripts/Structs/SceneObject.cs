using UnityEngine;

[System.Serializable]
public class SceneObject
{
	[SerializeField]
	private string m_SceneName;

	public static implicit operator string(SceneObject sceneObject)
	{
		return sceneObject.m_SceneName;
	}

	public static implicit operator SceneObject(string sceneName)
	{
		return new SceneObject() { m_SceneName = sceneName };
	}

    public override bool Equals(object obj)
    {
		if(obj == null || this.GetType() != obj.GetType())
        {
			return false;
        }

		SceneObject other = (SceneObject)obj;

		return m_SceneName == other.m_SceneName;
    }
}