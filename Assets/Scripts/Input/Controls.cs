// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""DefaultActionMap"",
            ""id"": ""a050cbff-2d56-4c18-b2bf-6562f6ece5b2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""70e2f657-81b2-40a9-842c-d0e1afe864a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TorsoMovementVertical"",
                    ""type"": ""Value"",
                    ""id"": ""4c7b7f8f-fa85-48d0-ae4c-fa2642f0daac"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0df3f278-f84f-4229-8765-ec9d8f84e3d4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5c54f919-c335-44bd-a16d-17d5409d737a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9ea9a42c-b15c-4748-b92d-a039a4142545"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0cd203f6-e19b-4e0d-8dcd-0e340f09787d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ea5870ef-b283-4968-9c40-c138dc784541"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""bd8652a0-673e-4d70-a93f-4447c97a0ac6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""df367993-8938-4213-9702-09844506fe8d"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ba470b8d-3e75-454f-af44-d74be598e0e9"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical"",
                    ""id"": ""ae4c38de-18df-4c9b-bbb9-792aafca4683"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f8e36438-e2f8-4da4-86b8-5af926152906"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8330c8ae-bfa9-4148-aae3-8adfdfc87693"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TorsoMovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // DefaultActionMap
        m_DefaultActionMap = asset.FindActionMap("DefaultActionMap", throwIfNotFound: true);
        m_DefaultActionMap_Movement = m_DefaultActionMap.FindAction("Movement", throwIfNotFound: true);
        m_DefaultActionMap_TorsoMovementVertical = m_DefaultActionMap.FindAction("TorsoMovementVertical", throwIfNotFound: true);
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

    // DefaultActionMap
    private readonly InputActionMap m_DefaultActionMap;
    private IDefaultActionMapActions m_DefaultActionMapActionsCallbackInterface;
    private readonly InputAction m_DefaultActionMap_Movement;
    private readonly InputAction m_DefaultActionMap_TorsoMovementVertical;
    public struct DefaultActionMapActions
    {
        private @Controls m_Wrapper;
        public DefaultActionMapActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_DefaultActionMap_Movement;
        public InputAction @TorsoMovementVertical => m_Wrapper.m_DefaultActionMap_TorsoMovementVertical;
        public InputActionMap Get() { return m_Wrapper.m_DefaultActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActionMapActions instance)
        {
            if (m_Wrapper.m_DefaultActionMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnMovement;
                @TorsoMovementVertical.started -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnTorsoMovementVertical;
                @TorsoMovementVertical.performed -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnTorsoMovementVertical;
                @TorsoMovementVertical.canceled -= m_Wrapper.m_DefaultActionMapActionsCallbackInterface.OnTorsoMovementVertical;
            }
            m_Wrapper.m_DefaultActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @TorsoMovementVertical.started += instance.OnTorsoMovementVertical;
                @TorsoMovementVertical.performed += instance.OnTorsoMovementVertical;
                @TorsoMovementVertical.canceled += instance.OnTorsoMovementVertical;
            }
        }
    }
    public DefaultActionMapActions @DefaultActionMap => new DefaultActionMapActions(this);
    public interface IDefaultActionMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnTorsoMovementVertical(InputAction.CallbackContext context);
    }
}
