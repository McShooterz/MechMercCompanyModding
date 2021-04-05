using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public Vector3 velocity = Vector3.zero;

    public bool invertMouseX = false;
    public bool invertMouseY = true;

	public float boost = 2f;
    public float speed = 100f;
    public float friction = 7f;

    public float sensitivity = 15f;

    public float dampening = 3f;

    void Start()
    {
        
    }

    void Update()
    {
		var speed = this.speed * ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? boost : 1f);

		velocity.x += InputManager.Instance.turn.GetValue() * Time.deltaTime * speed;
		velocity.z += InputManager.Instance.throttle.GetValue() * Time.deltaTime * speed;
		
		if (InputManager.Instance.externalCamera.IsPressed()) { velocity.y -= Time.deltaTime * speed; }
        if (InputManager.Instance.jumpJet.IsPressed()) { velocity.y += Time.deltaTime * speed; }
        
        transform.Translate(velocity * Time.deltaTime);
		
        Quaternion prevRotation = transform.rotation;
        float flipX = invertMouseX ? -1 : 1;
        float flipY = invertMouseY ? -1 : 1;
        transform.Rotate(0, flipX * sensitivity * InputManager.Instance.torsoYaw.GetValue(), 0);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
        transform.Rotate(flipY * sensitivity * InputManager.Instance.torsoPitch.GetValue(), 0, 0);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));

        transform.rotation = Quaternion.Lerp(prevRotation, transform.rotation, Time.deltaTime * dampening);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
        

        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * friction);
    }

	/*
    [RegisterCommand(Help = "Switches between in game and free camera", MinArgCount = 0, MaxArgCount = 0)]
    public static void CommandToggleFreeCamera(CommandArg[] args)
    {
        EnsureFreeCameraExists();
        if (!freeCameraController.enabled)
        {
            freeCameraController.enabled = true;

            PlayerHUD.instance.SetHudVisibility(false);
            MechController.playerMech.SetPlayerControl(false);
			var mech = MechController.playerMech;
			mech?.HideCockpit();

			originalCameraPosition = freeCameraController.transform.localPosition;
			originalCameraRotation = freeCameraController.transform.localRotation;
			originalParent = freeCameraController.transform.parent;
			freeCameraController.transform.SetParent(null);

			Terminal.Log("Switching to free camera");
        }
        else
        {
            freeCameraController.enabled = false;

			PlayerHUD.instance.SetHudVisibility(true);
            MechController.playerMech.SetPlayerControl(true);
			var mech = MechController.playerMech;
			mech?.ShowCockpit();

			freeCameraController.transform.SetParent(originalParent);
			freeCameraController.transform.localPosition = originalCameraPosition;
			freeCameraController.transform.localRotation = originalCameraRotation;

			Terminal.Log("Switching to game camera");
        }


    }

	[RegisterCommand(Help = "Changes or prints the speed of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraSpeed(CommandArg[] args) 
	{
		EnsureFreeCameraExists();
		if (args.Length > 0) 
		{
			float speed = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Movement Speed changed, {freeCameraController.speed} => {speed}");
			freeCameraController.speed = speed;
		} 
		else 
		{
			Terminal.Log($"Free Camera Movement Speed is {freeCameraController.speed}");
		}
	}

	[RegisterCommand(Help = "Changes or prints the friction of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraFriction(CommandArg[] args) {
		EnsureFreeCameraExists();
		if (args.Length > 0) 
		{
			float friction = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Movement Friction changed, {freeCameraController.friction} => {friction}");
			freeCameraController.friction = friction;
		} 
		else 
		{
			Terminal.Log($"Free Camera Movement Friction is {freeCameraController.friction}");
		}
	}

	[RegisterCommand(Help = "Changes or prints the Dampening of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraDampening(CommandArg[] args) 
	{
		EnsureFreeCameraExists();
		if (args.Length > 0) 
		{
			float dampening = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Look Dampening changed, {freeCameraController.dampening} => {dampening}");
			freeCameraController.dampening = dampening;
		} 
		else 
		{
			Terminal.Log($"Free Camera Look Dampening is {freeCameraController.dampening}");
		}
	}

	[RegisterCommand(Help = "Changes or prints the sensitivity of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraSensitivity(CommandArg[] args) 
	{
		EnsureFreeCameraExists();
		if (args.Length > 0) 
		{
			float sensitivity = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Look Sensitivity changed, {freeCameraController.sensitivity} => {sensitivity}");
			freeCameraController.sensitivity = sensitivity;
		} 
		else 
		{
			Terminal.Log($"Free Camera Look Sensitivity is {freeCameraController.sensitivity}");
		}
	}

	[RegisterCommand(Help = "Changes or prints the boost (hold shift) of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraBoost(CommandArg[] args)
	{
		EnsureFreeCameraExists();
		if (args.Length > 0) 
		{
			float boost = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Movement Boost changed, {freeCameraController.boost} => {boost}");
			freeCameraController.boost = boost;
		} 
		else 
		{
			Terminal.Log($"Free Camera Movement Boost is {freeCameraController.boost}");
		}
	}

	[RegisterCommand(Help = "Changes or prints the field of view of the Free Camera", MinArgCount = 0, MaxArgCount = 1)]
	public static void CommandFreeCameraFov(CommandArg[] args) 
	{
		EnsureFreeCameraExists();
		Camera cam = freeCameraController.GetComponent<Camera>();
		if (args.Length > 0) 
		{
			float fov = Mathf.Abs(args[0].Float);
			if (Terminal.IssuedError) { return; }
			Terminal.Log($"Free Camera Field of View changed, {cam.fieldOfView} => {fov}");
			cam.fieldOfView = fov;
		} 
		else 
		{
			Terminal.Log($"Free Camera Field of View is {cam.fieldOfView}");
		}
	}


	private static void EnsureFreeCameraExists()
	{
		if (freeCameraController == null) 
		{
			freeCameraController = CameraController.Instance.gameObject.AddComponent<FreeCameraController>();
			freeCameraController.enabled = false;
		}
	}*/
}
