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
    public static void RegisterController<T>(this Component component, T inputActions) where T : IInputActionCollection, System.IDisposable
    {
        inputActions.Enable();

        component.OnEnableAsObservable()
            .Subscribe(_ => inputActions.Enable()).AddTo(component);

        component.OnDisableAsObservable()
            .Subscribe(_ => inputActions.Disable()).AddTo(component);

        component.OnDestroyAsObservable()
            .Subscribe(_ => inputActions.Dispose()).AddTo(component);
    }
}
