using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;

public static class ComponentExtension
{
    /// <summary>
    /// InputActionとComponentを連動させる
    /// </summary>
    /// <param name="component">コンポーネント</param>
    /// <param name="inputActions">連動させるInputAction</param>
    public static void RegisterController(this Component component, IInputActionCollection inputActions)
    {
        inputActions.Enable();

        component.OnEnableAsObservable()
            .Subscribe(_ => inputActions.Enable());

        component.OnDisableAsObservable()
            .Subscribe(_ => inputActions.Disable());

        component.OnDestroyAsObservable()
            .Subscribe(_ => inputActions.Disable());
    }
}
