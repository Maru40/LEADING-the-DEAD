using UnityEngine;

public class CustomLabelAttribute : PropertyAttribute
{
    public string customLabel;

    public CustomLabelAttribute(string customLabel)
    {
        this.customLabel = customLabel;
    }
}
