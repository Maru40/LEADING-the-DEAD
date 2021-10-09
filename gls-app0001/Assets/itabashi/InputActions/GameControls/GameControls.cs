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
                    ""name"": ""ThrowAim"",
                    ""type"": ""Value"",
                    ""id"": ""abc3db55-edbb-4c4c-abce-cb9b2db9b274"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""3aba2e08-be4d-46eb-9af2-013569e3976f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemSelectUp"",
                    ""type"": ""Button"",
                    ""id"": ""da51d974-36d2-4630-a043-ed0da0c32994"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemSelectDown"",
                    ""type"": ""Button"",
                    ""id"": ""300f730f-0350-4a04-8eca-411b48277dca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemSelectLeft"",
                    ""type"": ""Button"",
                    ""id"": ""b008c3e3-447e-41ba-b5de-be341ab5959a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemSelectRight"",
                    ""type"": ""Button"",
                    ""id"": ""0ba1ba3a-45df-41f0-a8f4-0b7bef732087"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""078a078a-3d08-4b20-87ff-cbbe05cc71b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BatSwing"",
                    ""type"": ""Button"",
                    ""id"": ""7a5bd760-fcea-49d4-886d-f883f86e3511"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""1314145f-a84b-4d81-a9a6-8767ace4f73d"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67c790b1-d0ce-4ba5-b080-1f29121abaa5"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemSelectUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79e34ec4-c9fc-43cb-a727-d66c5fda444e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemSelectDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d80ca1dd-a805-4612-b0f6-765398647dee"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemSelectLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7474b8a9-8211-4133-b065-2fdcfd97addb"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemSelectRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd7c2273-38aa-4c05-8736-6309b6ad2758"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68d2b87b-5998-4965-aecc-fe1b83af7bd4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BatSwing"",
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
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        m_Player_RotateViewPoint = m_Player.FindAction("RotateViewPoint", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_ThrowingStance = m_Player.FindAction("ThrowingStance", throwIfNotFound: true);
        m_Player_ThrowAim = m_Player.FindAction("ThrowAim", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_ItemSelectUp = m_Player.FindAction("ItemSelectUp", throwIfNotFound: true);
        m_Player_ItemSelectDown = m_Player.FindAction("ItemSelectDown", throwIfNotFound: true);
        m_Player_ItemSelectLeft = m_Player.FindAction("ItemSelectLeft", throwIfNotFound: true);
        m_Player_ItemSelectRight = m_Player.FindAction("ItemSelectRight", throwIfNotFound: true);
        m_Player_UseItem = m_Player.FindAction("UseItem", throwIfNotFound: true);
        m_Player_BatSwing = m_Player.FindAction("BatSwing", throwIfNotFound: true);
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
    private readonly InputAction m_Player_ThrowAim;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_ItemSelectUp;
    private readonly InputAction m_Player_ItemSelectDown;
    private readonly InputAction m_Player_ItemSelectLeft;
    private readonly InputAction m_Player_ItemSelectRight;
    private readonly InputAction m_Player_UseItem;
    private readonly InputAction m_Player_BatSwing;
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
        public InputAction @ThrowAim => m_Wrapper.m_Player_ThrowAim;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @ItemSelectUp => m_Wrapper.m_Player_ItemSelectUp;
        public InputAction @ItemSelectDown => m_Wrapper.m_Player_ItemSelectDown;
        public InputAction @ItemSelectLeft => m_Wrapper.m_Player_ItemSelectLeft;
        public InputAction @ItemSelectRight => m_Wrapper.m_Player_ItemSelectRight;
        public InputAction @UseItem => m_Wrapper.m_Player_UseItem;
        public InputAction @BatSwing => m_Wrapper.m_Player_BatSwing;
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
                @ThrowAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
                @ThrowAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
                @ThrowAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrowAim;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @ItemSelectUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectUp;
                @ItemSelectUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectUp;
                @ItemSelectUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectUp;
                @ItemSelectDown.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectDown;
                @ItemSelectDown.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectDown;
                @ItemSelectDown.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectDown;
                @ItemSelectLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectLeft;
                @ItemSelectLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectLeft;
                @ItemSelectLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectLeft;
                @ItemSelectRight.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectRight;
                @ItemSelectRight.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectRight;
                @ItemSelectRight.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemSelectRight;
                @UseItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUseItem;
                @BatSwing.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBatSwing;
                @BatSwing.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBatSwing;
                @BatSwing.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBatSwing;
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
                @ThrowAim.started += instance.OnThrowAim;
                @ThrowAim.performed += instance.OnThrowAim;
                @ThrowAim.canceled += instance.OnThrowAim;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @ItemSelectUp.started += instance.OnItemSelectUp;
                @ItemSelectUp.performed += instance.OnItemSelectUp;
                @ItemSelectUp.canceled += instance.OnItemSelectUp;
                @ItemSelectDown.started += instance.OnItemSelectDown;
                @ItemSelectDown.performed += instance.OnItemSelectDown;
                @ItemSelectDown.canceled += instance.OnItemSelectDown;
                @ItemSelectLeft.started += instance.OnItemSelectLeft;
                @ItemSelectLeft.performed += instance.OnItemSelectLeft;
                @ItemSelectLeft.canceled += instance.OnItemSelectLeft;
                @ItemSelectRight.started += instance.OnItemSelectRight;
                @ItemSelectRight.performed += instance.OnItemSelectRight;
                @ItemSelectRight.canceled += instance.OnItemSelectRight;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
                @BatSwing.started += instance.OnBatSwing;
                @BatSwing.performed += instance.OnBatSwing;
                @BatSwing.canceled += instance.OnBatSwing;
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
        void OnThrowAim(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnItemSelectUp(InputAction.CallbackContext context);
        void OnItemSelectDown(InputAction.CallbackContext context);
        void OnItemSelectLeft(InputAction.CallbackContext context);
        void OnItemSelectRight(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnBatSwing(InputAction.CallbackContext context);
    }
}
