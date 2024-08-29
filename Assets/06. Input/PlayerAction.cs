//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/06. Input/PlayerAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""4f5a0777-a20f-485b-903a-268e6c78850b"",
            ""actions"": [
                {
                    ""name"": ""GrapInput"",
                    ""type"": ""Button"",
                    ""id"": ""d16828d3-fe38-4bb1-87be-2749b526d9fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""StopInput"",
                    ""type"": ""Button"",
                    ""id"": ""690b7a5e-a4c3-4ce0-b354-b97342a84aa8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""28745b83-29e0-434b-a251-c4a36c8dfad7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""GrapInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23ad3455-813d-4080-8f95-dd986c7b9aff"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""StopInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""MOBILE"",
            ""bindingGroup"": ""MOBILE"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_GrapInput = m_Player.FindAction("GrapInput", throwIfNotFound: true);
        m_Player_StopInput = m_Player.FindAction("StopInput", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_GrapInput;
    private readonly InputAction m_Player_StopInput;
    public struct PlayerActions
    {
        private @PlayerAction m_Wrapper;
        public PlayerActions(@PlayerAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @GrapInput => m_Wrapper.m_Player_GrapInput;
        public InputAction @StopInput => m_Wrapper.m_Player_StopInput;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @GrapInput.started += instance.OnGrapInput;
            @GrapInput.performed += instance.OnGrapInput;
            @GrapInput.canceled += instance.OnGrapInput;
            @StopInput.started += instance.OnStopInput;
            @StopInput.performed += instance.OnStopInput;
            @StopInput.canceled += instance.OnStopInput;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @GrapInput.started -= instance.OnGrapInput;
            @GrapInput.performed -= instance.OnGrapInput;
            @GrapInput.canceled -= instance.OnGrapInput;
            @StopInput.started -= instance.OnStopInput;
            @StopInput.performed -= instance.OnStopInput;
            @StopInput.canceled -= instance.OnStopInput;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    private int m_MOBILESchemeIndex = -1;
    public InputControlScheme MOBILEScheme
    {
        get
        {
            if (m_MOBILESchemeIndex == -1) m_MOBILESchemeIndex = asset.FindControlSchemeIndex("MOBILE");
            return asset.controlSchemes[m_MOBILESchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnGrapInput(InputAction.CallbackContext context);
        void OnStopInput(InputAction.CallbackContext context);
    }
}
