// GENERATED AUTOMATICALLY FROM 'Assets/Input/BoxingInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BoxingInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BoxingInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BoxingInputActions"",
    ""maps"": [
        {
            ""name"": ""Boxer"",
            ""id"": ""1ccc1a05-445a-447a-aaf5-f59c6d9ea26a"",
            ""actions"": [
                {
                    ""name"": ""PunchLeft"",
                    ""type"": ""Button"",
                    ""id"": ""c3fe8aea-22ca-419a-9bcf-ea472b9381ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PunchRight"",
                    ""type"": ""Button"",
                    ""id"": ""5be8001d-09da-4fd5-a490-c7262efb88ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""258e45f8-2719-4ad0-ab45-09dc93f26ea7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""c8abf501-fb1f-44a1-8fbb-64b171b2b082"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""568b0bd1-aa08-4aed-8e7b-9efbd2483c24"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9458ece8-3bb1-4b4b-8df5-98f4bf93c285"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""093c0da7-654e-4821-be49-5126d2a3ba65"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86bec820-962a-4846-8d85-ab196408bda3"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""81f95a93-313f-423e-a35d-af0213dc97df"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""aeac76f8-d5e7-414a-a82c-f16c16549268"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b70b73e5-e488-45bc-94ba-8cab0058f87d"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a4202802-0371-47ed-a916-d6309b1eb245"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aa9d83c8-4c3b-48b5-ad5d-a24b902ebc2f"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""574fcacf-9ca4-4634-9b29-edd416c9b2da"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6af42e9-5e7f-4e4f-9edf-a143e6879f6e"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Boxer
        m_Boxer = asset.FindActionMap("Boxer", throwIfNotFound: true);
        m_Boxer_PunchLeft = m_Boxer.FindAction("PunchLeft", throwIfNotFound: true);
        m_Boxer_PunchRight = m_Boxer.FindAction("PunchRight", throwIfNotFound: true);
        m_Boxer_Move = m_Boxer.FindAction("Move", throwIfNotFound: true);
        m_Boxer_Block = m_Boxer.FindAction("Block", throwIfNotFound: true);
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

    // Boxer
    private readonly InputActionMap m_Boxer;
    private IBoxerActions m_BoxerActionsCallbackInterface;
    private readonly InputAction m_Boxer_PunchLeft;
    private readonly InputAction m_Boxer_PunchRight;
    private readonly InputAction m_Boxer_Move;
    private readonly InputAction m_Boxer_Block;
    public struct BoxerActions
    {
        private @BoxingInputActions m_Wrapper;
        public BoxerActions(@BoxingInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PunchLeft => m_Wrapper.m_Boxer_PunchLeft;
        public InputAction @PunchRight => m_Wrapper.m_Boxer_PunchRight;
        public InputAction @Move => m_Wrapper.m_Boxer_Move;
        public InputAction @Block => m_Wrapper.m_Boxer_Block;
        public InputActionMap Get() { return m_Wrapper.m_Boxer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BoxerActions set) { return set.Get(); }
        public void SetCallbacks(IBoxerActions instance)
        {
            if (m_Wrapper.m_BoxerActionsCallbackInterface != null)
            {
                @PunchLeft.started -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchLeft;
                @PunchLeft.performed -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchLeft;
                @PunchLeft.canceled -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchLeft;
                @PunchRight.started -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchRight;
                @PunchRight.performed -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchRight;
                @PunchRight.canceled -= m_Wrapper.m_BoxerActionsCallbackInterface.OnPunchRight;
                @Move.started -= m_Wrapper.m_BoxerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BoxerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BoxerActionsCallbackInterface.OnMove;
                @Block.started -= m_Wrapper.m_BoxerActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_BoxerActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_BoxerActionsCallbackInterface.OnBlock;
            }
            m_Wrapper.m_BoxerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PunchLeft.started += instance.OnPunchLeft;
                @PunchLeft.performed += instance.OnPunchLeft;
                @PunchLeft.canceled += instance.OnPunchLeft;
                @PunchRight.started += instance.OnPunchRight;
                @PunchRight.performed += instance.OnPunchRight;
                @PunchRight.canceled += instance.OnPunchRight;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
            }
        }
    }
    public BoxerActions @Boxer => new BoxerActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IBoxerActions
    {
        void OnPunchLeft(InputAction.CallbackContext context);
        void OnPunchRight(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
    }
}
