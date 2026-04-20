using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ModestTree.Zenject
{
    // Note: this corresponds to the values expected by Mouse.current buttons
    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
    }

    public class UnityEventManager : MonoBehaviour, ITickable
    {
        public event Action ApplicationGainedFocus = delegate { };
        public event Action ApplicationLostFocus = delegate { };
        public event Action<bool> ApplicationFocusChanged = delegate { };

        public event Action ApplicationQuit = delegate {};
        public event Action Gui = delegate {};
        public event Action DrawGizmos = delegate {};

        public event Action LeftMouseButtonDown = delegate {};
        public event Action LeftMouseButtonUp = delegate {};

        public event Action RightMouseButtonDown = delegate {};
        public event Action RightMouseButtonUp = delegate {};

        public event Action MouseMove = delegate {};

        Vector2 _lastMousePosition;

        [InjectNamed("TickPriority")]
        [InjectOptional]
        int _tickPriority = 0;

        public bool IsFocused
        {
            get;
            private set;
        }

        public int TickPriority
        {
            get
            {
                return _tickPriority;
            }
        }

        public void Tick()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                #if ENABLE_LEGACY_INPUT_MANAGER
                if (UnityEngine.Input.GetMouseButtonDown((int)MouseButtons.Left))
                {
                    LeftMouseButtonDown();
                }
                else if (UnityEngine.Input.GetMouseButtonUp((int)MouseButtons.Left))
                {
                    LeftMouseButtonUp();
                }

                if (UnityEngine.Input.GetMouseButtonDown((int)MouseButtons.Right))
                {
                    RightMouseButtonDown();
                }
                else if (UnityEngine.Input.GetMouseButtonUp((int)MouseButtons.Right))
                {
                    RightMouseButtonUp();
                }

                var legacyMousePosition = (Vector2)UnityEngine.Input.mousePosition;
                if (_lastMousePosition != legacyMousePosition)
                {
                    _lastMousePosition = legacyMousePosition;
                    MouseMove();
                }
                #endif
                return;
            }

            if (mouse.leftButton.wasPressedThisFrame)
            {
                LeftMouseButtonDown();
            }
            else if (mouse.leftButton.wasReleasedThisFrame)
            {
                LeftMouseButtonUp();
            }

            if (mouse.rightButton.wasPressedThisFrame)
            {
                RightMouseButtonDown();
            }
            else if (mouse.rightButton.wasReleasedThisFrame)
            {
                RightMouseButtonUp();
            }

            var mousePosition = mouse.position.ReadValue();
            if (_lastMousePosition != mousePosition)
            {
                _lastMousePosition = mousePosition;
                MouseMove();
            }
        }

        void OnGUI()
        {
            Gui();
        }

        void OnApplicationQuit()
        {
            ApplicationQuit();
        }

        void OnDrawGizmos()
        {
            DrawGizmos();
        }

        void OnApplicationFocus(bool newIsFocused)
        {
            if (newIsFocused && !IsFocused)
            {
                IsFocused = true;
                ApplicationGainedFocus();
                ApplicationFocusChanged(true);
            }

            if (!newIsFocused && IsFocused)
            {
                IsFocused = false;
                ApplicationLostFocus();
                ApplicationFocusChanged(false);
            }
        }
    }
}
