using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public enum GameTouchPhase
{
	None,
	Began,
	Moved,
	Stationary,
	Ended,
	Canceled
}

public struct GameTouchData
{
	public Vector2 Position;
	public GameTouchPhase Phase;
	public int TouchId;
}

public static class GameInput
{
	private const float LegacyMouseSensitivity = 0.1f;

	private static bool _initialized;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		EnsureInitialized();
	}

	private static void EnsureInitialized()
	{
		if (_initialized)
		{
			return;
		}

		if (!EnhancedTouchSupport.enabled)
		{
			EnhancedTouchSupport.Enable();
		}

		if (Accelerometer.current != null)
		{
			InputSystem.EnableDevice(Accelerometer.current);
		}

		_initialized = true;
	}

	public static Vector2 ReadMouseLookAxis()
	{
		EnsureInitialized();

		Vector2 lookInput = (Mouse.current?.delta.ReadValue() ?? Vector2.zero) * LegacyMouseSensitivity;
		if (lookInput != Vector2.zero)
		{
			return lookInput;
		}

		#if ENABLE_LEGACY_INPUT_MANAGER
		return new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
		#else
		return Vector2.zero;
		#endif
	}

	public static Vector3 ReadAcceleration()
	{
		EnsureInitialized();

		if (Accelerometer.current != null)
		{
			return Accelerometer.current.acceleration.ReadValue();
		}

		#if ENABLE_LEGACY_INPUT_MANAGER
		return UnityEngine.Input.acceleration;
		#else
		return Vector3.zero;
		#endif
	}

	public static int TouchCount
	{
		get
		{
			EnsureInitialized();

			int count = Touch.activeTouches.Count;
			#if ENABLE_LEGACY_INPUT_MANAGER
			if (count == 0)
			{
				count = UnityEngine.Input.touchCount;
			}
			#endif
			return count;
		}
	}

	public static GameTouchData GetTouch(int index)
	{
		EnsureInitialized();

		if (index < Touch.activeTouches.Count)
		{
			Touch touch = Touch.activeTouches[index];
			return new GameTouchData
			{
				Position = touch.screenPosition,
				Phase = ToGameTouchPhase(touch.phase),
				TouchId = touch.touchId
			};
		}

		#if ENABLE_LEGACY_INPUT_MANAGER
		if (index < UnityEngine.Input.touchCount)
		{
			UnityEngine.Touch touch = UnityEngine.Input.GetTouch(index);
			return new GameTouchData
			{
				Position = touch.position,
				Phase = ToGameTouchPhase(touch.phase),
				TouchId = touch.fingerId
			};
		}
		#endif

		return default;
	}

	public static bool AnyTouchActive
	{
		get { return TouchCount > 0; }
	}

	public static bool IsContinuePressed(bool isMobile)
	{
		EnsureInitialized();
		return isMobile ? AnyTouchActive : IsDesktopSubmitPressed();
	}

	public static bool IsDesktopSubmitPressed()
	{
		EnsureInitialized();

		Keyboard keyboard = Keyboard.current;
		if (keyboard != null && keyboard.anyKey.isPressed)
		{
			return true;
		}

		Mouse mouse = Mouse.current;
		if (mouse != null && (mouse.leftButton.isPressed || mouse.rightButton.isPressed || mouse.middleButton.isPressed))
		{
			return true;
		}

		Gamepad gamepad = Gamepad.current;
		if (gamepad != null && (gamepad.buttonSouth.isPressed || gamepad.startButton.isPressed))
		{
			return true;
		}

		#if ENABLE_LEGACY_INPUT_MANAGER
		return UnityEngine.Input.anyKey
			|| UnityEngine.Input.GetButton("Fire1")
			|| UnityEngine.Input.GetMouseButton(0)
			|| UnityEngine.Input.GetMouseButton(1)
			|| UnityEngine.Input.GetMouseButton(2);
		#else
		return false;
		#endif
	}

	public static bool WasKeyboardKeyPressed(Key key, KeyCode legacyKey)
	{
		EnsureInitialized();

		Keyboard keyboard = Keyboard.current;
		if (keyboard != null && keyboard[key].wasPressedThisFrame)
		{
			return true;
		}

		#if ENABLE_LEGACY_INPUT_MANAGER
		return UnityEngine.Input.GetKeyDown(legacyKey);
		#else
		return false;
		#endif
	}

	private static GameTouchPhase ToGameTouchPhase(UnityEngine.InputSystem.TouchPhase phase)
	{
		switch (phase)
		{
			case UnityEngine.InputSystem.TouchPhase.Began:
				return GameTouchPhase.Began;
			case UnityEngine.InputSystem.TouchPhase.Moved:
				return GameTouchPhase.Moved;
			case UnityEngine.InputSystem.TouchPhase.Stationary:
				return GameTouchPhase.Stationary;
			case UnityEngine.InputSystem.TouchPhase.Ended:
				return GameTouchPhase.Ended;
			case UnityEngine.InputSystem.TouchPhase.Canceled:
				return GameTouchPhase.Canceled;
			default:
				return GameTouchPhase.None;
		}
	}

	#if ENABLE_LEGACY_INPUT_MANAGER
	private static GameTouchPhase ToGameTouchPhase(UnityEngine.TouchPhase phase)
	{
		switch (phase)
		{
			case UnityEngine.TouchPhase.Began:
				return GameTouchPhase.Began;
			case UnityEngine.TouchPhase.Moved:
				return GameTouchPhase.Moved;
			case UnityEngine.TouchPhase.Stationary:
				return GameTouchPhase.Stationary;
			case UnityEngine.TouchPhase.Ended:
				return GameTouchPhase.Ended;
			case UnityEngine.TouchPhase.Canceled:
				return GameTouchPhase.Canceled;
			default:
				return GameTouchPhase.None;
		}
	}
	#endif
}
