// GENERATED AUTOMATICALLY FROM 'Assets/itabashi/InputActions/GameControls/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""d6252b34-d6da-4d43-ad91-ec2ace46c8a9"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""ccb116db-f242-4614-b759-6b1740fb2109"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c09ac2ad-22ac-41b4-8cf3-4df385c12fd2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""2b84931d-e82d-4922-a112-ff9c12820171"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RotateViewPoint"",
                    ""type"": ""Value"",
                    ""id"": ""f42eb2bb-fa51-42f8-bc9c-f1d2ef36ae2a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""ed3c762f-f222-4959-9238-939b82ed67ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowingStance"",
                    ""type"": ""Button"",
                    ""id"": ""e48ab3eb-3771-4943-b01c-010eb3ffff0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""c6dc93e5-54d3-43c8-b554-c4d8092af467"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowAim"",
                    ""type"": ""Value"",
                    ""id"": ""abc3db55-edbb-4c4c-abce-cb9b2db9b274"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""67c78102-2091-4733-8702-3ddccc5db853"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""864d7c9c-001a-4c62-b737-1b3d1a162ba1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4cceafa-6a3a-4cb9-b8ad-8950dd85109b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b12731f1-b6a0-4ed3-b251-13be47ffbb21"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateViewPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""718662aa-22c7-4c45-bc76-602b581835f7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d54afae-c8c3-4bb7-8a9a-1554b9a55061"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowingStance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b107b64-6520-4815-b2c8-a22ca58d41d9"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Right Stick Vertical"",
                    ""id"": ""561c4ba9-6208-4f57-8c99-64d782e9520c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowAim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ce72aff0-c874-48ee-b7c8-10e0a3cd98af"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7b1fef51-bfad-4788-b564-2f3342acbd3a"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        m_Player_RotateViewPoint = m_Player.FindAction("RotateViewPoint", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_ThrowingStance = m_Player.FindAction("ThrowingStance", throwIfNotFound: true);
        m_Player_Throw = m_Player.FindAction("Throw", throwIfNotFound: true);
        m_Player_ThrowAim = m_Player.FindAction("ThrowAim", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Cancel;
    private readonly InputAction m_Player_RotateViewPoint;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_ThrowingStance;
    private readonly InputAction m_Player_Throw;
    private readonly InputAction m_Player_ThrowAim;
    public struct PlayerActions
    {
        private @GameControls m_Wrapper;
        public PlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
        public InputAction @RotateViewPoint => m_Wrapper.m_Player_RotateViewPoint;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @ThrowingStance => m_Wrapper.m_Player_ThrowingStance;
        public InputAction @Throw => m_Wrapper.m_Player_Throw;
        public InputAction @ThrowAim => m_Wrapper.m_Player_ThrowAim;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @RotateViewPoint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @RotateViewPoint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @RotateViewPoint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @ThrowingStance.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowingStance;
                @ThrowingStance.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowingStance;
                @ThrowingStance.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowingStance;
                @Throw.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @ThrowAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
                @ThrowAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
                @ThrowAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @RotateViewPoint.started += instance.OnRotateViewPoint;
                @RotateViewPoint.performed += instance.OnRotateViewPoint;
                @RotateViewPoint.canceled += instance.OnRotateViewPoint;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @ThrowingStance.started += instance.OnThrowingStance;
                @ThrowingStance.performed += instance.OnThrowingStance;
                @ThrowingStance.canceled += instance.OnThrowingStance;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @ThrowAim.started += instance.OnThrowAim;
                @ThrowAim.performed += instance.OnThrowAim;
                @ThrowAim.canceled += instance.OnThrowAim;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnRotateViewPoint(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnThrowingStance(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnThrowAim(InputAction.CallbackContext context);
    }
}
