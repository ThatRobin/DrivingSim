using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        public Vector2 Turn { get; private set; }

        public float Acceleration { get; private set; }

        public float Brake {  get; private set; }

        public bool BackGear { get; private set; }

        public bool NextGear { get; private set; }

        public bool EngineOnOff { get; private set; }


        private bool NextGearPressed = false;

        private bool PrevGearPressed = false;

        private bool EngineOnOffPressed = false;

        private InputActionMap _currentMap;
        private InputAction _TurnAction;
        private InputAction _AccelerateAction;
        private InputAction _BrakeAction;
        private InputAction _BGearAction;
        private InputAction _NGearAction;
        private InputAction _EngineAction;

        private void Start()
        {
            EngineOnOff = true;
        }

        private void Awake()
        {
            _currentMap = playerInput.currentActionMap;

            _TurnAction = _currentMap.FindAction("Turn");
            _AccelerateAction = _currentMap.FindAction("Accelerate");
            _BrakeAction = _currentMap.FindAction("Brake");
            _BGearAction = _currentMap.FindAction("BackGear");
            _NGearAction = _currentMap.FindAction("NextGear");
            _EngineAction = _currentMap.FindAction("EngineOn");

            _TurnAction.performed += onTurn;
            _AccelerateAction.performed += onAccelerate;
            _BrakeAction.performed += onBrake;
            _BGearAction.performed += onBGear;
            _NGearAction.performed += onNGear;
            _EngineAction.performed += onEngine;

            _TurnAction.canceled += onTurn;
            _AccelerateAction.canceled += onAccelerate;
            _BrakeAction.canceled += onBrake;
            _BGearAction.canceled += onBGear;
            _NGearAction.canceled += onNGear;
            _EngineAction.canceled += onEngine;

        }

        private void onTurn(InputAction.CallbackContext context)
        {
            Turn = context.ReadValue<Vector2>();
        }

        private void onAccelerate(InputAction.CallbackContext context)
        {
            Acceleration = context.ReadValue<float>();
        }

        private void onBrake(InputAction.CallbackContext context)
        {
            Brake = context.ReadValue<float>();
        }

        private void onEngine(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0 && !EngineOnOffPressed)
            {
                EngineOnOff = !EngineOnOff;
                EngineOnOffPressed = true;
            }
            else
            {
                EngineOnOffPressed = false;
            }

        }

        private void onBGear(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0 && !PrevGearPressed)
            {
                PMovement.CurrentGear--;
                PMovement.CurrentGear = Mathf.Clamp(PMovement.CurrentGear, 0, 6);
                PrevGearPressed = true;
            }
            else
            {
                PrevGearPressed = false;
            }
        }

        private void onNGear(InputAction.CallbackContext context)
        {
            
            if (context.ReadValue<float>() > 0 && !NextGearPressed)
            {
                PMovement.CurrentGear++;
                PMovement.CurrentGear = Mathf.Clamp(PMovement.CurrentGear, 0, 6);
                NextGearPressed = true;
            }
            else
            {
                NextGearPressed = false;
            }
        }

        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }

    }
}
