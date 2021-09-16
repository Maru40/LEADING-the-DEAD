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
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""76f28534-4c1a-4ca0-9b86-6a321b827db4"",
                    ""expectedControlType"": ""Button"",
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
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""c4cbd2e6-2362-4d76-a81c-a0d39029c958"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""e8b071a6-e352-4764-8175-7928738b6388"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Guard"",
                    ""type"": ""Button"",
                    ""id"": ""45ed9c2e-f754-4160-bf1d-7c0891c8fafe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""af125856-8bf2-4307-92d5-f221fc0dbbc8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""c53fdc7e-8f79-4595-b9c8-55c1b7b55d36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SubWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""d400e9e6-d395-4224-b589-a1b50b1f648c"",
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
                    ""name"": ""WeaponChangeLeft"",
                    ""type"": ""Button"",
                    ""id"": ""3e90601a-5270-46e5-b0fd-cca79b6cdc12"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponChangeRight"",
                    ""type"": ""Button"",
                    ""id"": ""23f221d9-ec84-4f65-a0d3-cf703c94ae64"",
                    ""expectedControlType"": ""Button"",
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
                    ""id"": ""5803d7a0-d519-4d8c-833f-eac875bdd2e7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
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
                    ""id"": ""bf3be0df-e421-400c-a7d5-caa28666feda"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12628588-6a8c-48f3-8582-19d289033170"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cb751fc-0b61-42cc-bdbe-6b33257d9c41"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Guard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8346696-7b33-4797-9879-5d9245f49235"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7ef4fcd-3393-428d-b8b0-1c6beb5b0a5b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""156723f3-5aad-483c-8165-2bc6479b404e"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SubWeapon"",
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
                    ""id"": ""c10db78b-92d7-41bd-bcd4-209f6c436e24"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponChangeLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aec815ca-542d-4161-90cb-3bf1646d004a"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponChangeRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Guard = m_Player.FindAction("Guard", throwIfNotFound: true);
        m_Player_Dodge = m_Player.FindAction("Dodge", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_SubWeapon = m_Player.FindAction("SubWeapon", throwIfNotFound: true);
        m_Player_RotateViewPoint = m_Player.FindAction("RotateViewPoint", throwIfNotFound: true);
        m_Player_WeaponChangeLeft = m_Player.FindAction("WeaponChangeLeft", throwIfNotFound: true);
        m_Player_WeaponChangeRight = m_Player.FindAction("WeaponChangeRight", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Cancel;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Guard;
    private readonly InputAction m_Player_Dodge;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_SubWeapon;
    private readonly InputAction m_Player_RotateViewPoint;
    private readonly InputAction m_Player_WeaponChangeLeft;
    private readonly InputAction m_Player_WeaponChangeRight;
    public struct PlayerActions
    {
        private @GameControls m_Wrapper;
        public PlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Guard => m_Wrapper.m_Player_Guard;
        public InputAction @Dodge => m_Wrapper.m_Player_Dodge;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @SubWeapon => m_Wrapper.m_Player_SubWeapon;
        public InputAction @RotateViewPoint => m_Wrapper.m_Player_RotateViewPoint;
        public InputAction @WeaponChangeLeft => m_Wrapper.m_Player_WeaponChangeLeft;
        public InputAction @WeaponChangeRight => m_Wrapper.m_Player_WeaponChangeRight;
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
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Reload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Guard.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuard;
                @Guard.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuard;
                @Guard.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGuard;
                @Dodge.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDodge;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @SubWeapon.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSubWeapon;
                @SubWeapon.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSubWeapon;
                @SubWeapon.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSubWeapon;
                @RotateViewPoint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @RotateViewPoint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @RotateViewPoint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateViewPoint;
                @WeaponChangeLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeLeft;
                @WeaponChangeLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeLeft;
                @WeaponChangeLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeLeft;
                @WeaponChangeRight.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeRight;
                @WeaponChangeRight.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeRight;
                @WeaponChangeRight.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponChangeRight;
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
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Guard.started += instance.OnGuard;
                @Guard.performed += instance.OnGuard;
                @Guard.canceled += instance.OnGuard;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @SubWeapon.started += instance.OnSubWeapon;
                @SubWeapon.performed += instance.OnSubWeapon;
                @SubWeapon.canceled += instance.OnSubWeapon;
                @RotateViewPoint.started += instance.OnRotateViewPoint;
                @RotateViewPoint.performed += instance.OnRotateViewPoint;
                @RotateViewPoint.canceled += instance.OnRotateViewPoint;
                @WeaponChangeLeft.started += instance.OnWeaponChangeLeft;
                @WeaponChangeLeft.performed += instance.OnWeaponChangeLeft;
                @WeaponChangeLeft.canceled += instance.OnWeaponChangeLeft;
                @WeaponChangeRight.started += instance.OnWeaponChangeRight;
                @WeaponChangeRight.performed += instance.OnWeaponChangeRight;
                @WeaponChangeRight.canceled += instance.OnWeaponChangeRight;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnGuard(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnSubWeapon(InputAction.CallbackContext context);
        void OnRotateViewPoint(InputAction.CallbackContext context);
        void OnWeaponChangeLeft(InputAction.CallbackContext context);
        void OnWeaponChangeRight(InputAction.CallbackContext context);
    }
}
