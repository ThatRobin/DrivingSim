using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityTutorial.Manager;

public class PMovement : MonoBehaviour
{
    // Car Specs Force Power and Turn Angles
    public float _motorPower;
    public float TurningAngle;
    public float DownForce = 50;
    public bool isEngineOn;
    public float BrakeForce;
    public bool CanResetPos;

    // Rev And Engine Temp Varible Area ;;
    public float maxRPM;
    public float minRPM = 0f;
    [Range(0, 6)]
    public int CurrentGearValue;
    [Range(0,6)]
    public static int CurrentGear;
    public float[] GearsRatio;
    public float Current_Speed;
    public float currentRPM = 0f;
    public float NeedleAccelerationForce;
    public float NeedleDecelerationForce;
    
    private float CurrentTorque;
    private bool Next;
    private bool Previous;
    
    private bool IsAirborne;
    private float Acceleration;
    private float Brakes;

    private Rigidbody _rb;
    private InputManager _inputManager;

    [SerializeField] private WheelCollider frontWheel_R_Col, frontWheel_L_Col;
    [SerializeField] private WheelCollider backWheel_R_Col, backWheel_L_Col;

    [SerializeField] private Transform frontWheel_R, frontWheel_L;
    [SerializeField] private Transform backWheel_R, backWheel_L;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        currentRPM = 0;
        isEngineOn = true;
    }

    private void FixedUpdate()
    {

        UpdateWheel(frontWheel_R_Col, frontWheel_R);
        UpdateWheel(frontWheel_L_Col, frontWheel_L);
        UpdateWheel(backWheel_R_Col, backWheel_R);
        UpdateWheel(backWheel_L_Col, backWheel_L);

        CurrentGearValue = PMovement.CurrentGear;

        Steering();
        RevEngine();
        ChangeGear();
        Engine();  
        Brake();
        Airborne();
    }

    private void Update()
    {
        Acceleration = _inputManager.Acceleration;
        Brakes = _inputManager.Brake;
        Current_Speed = _rb.velocity.magnitude;
       
    }

    private void Engine()
    {
       
        if (isEngineOn && Brakes != 1 && GearsRatio[CurrentGear] != 1 && Acceleration == 1)
        {

            CurrentTorque = Acceleration * _motorPower * currentRPM * GearsRatio[CurrentGear] * Time.deltaTime;
            backWheel_L_Col.motorTorque = CurrentTorque;
            backWheel_R_Col.motorTorque = CurrentTorque;

            backWheel_L_Col.brakeTorque = 0;
            backWheel_R_Col.brakeTorque = 0;

        }else if(isEngineOn && Brakes ==1 && Acceleration ==1)
        {
            backWheel_L_Col.brakeTorque = 0;
            backWheel_R_Col.brakeTorque = 0;
        }
        
    }

    private void Brake()
    {
        if(Brakes == 1)
        {
            backWheel_L_Col.brakeTorque += BrakeForce * Time.deltaTime;
            backWheel_R_Col.brakeTorque += BrakeForce * Time.deltaTime;
        }
        else
        {
            backWheel_L_Col.brakeTorque = 0;
            backWheel_R_Col.brakeTorque = 0;
        }
    }

    private void RevEngine()
    {
        //Check if engine status 
        if (isEngineOn && Acceleration != 1 && currentRPM <= minRPM)
        {
            currentRPM = minRPM;

        }
        else if (isEngineOn && Acceleration != 1 && currentRPM > minRPM)
        {
            currentRPM -= currentRPM * Time.deltaTime;//Decrease the currentRPM to minRPM if Engine is On
        }
        else if (!isEngineOn && currentRPM > 0)// Check if Engine is on to Turn Off RPM
        {
            currentRPM -= NeedleDecelerationForce * Time.deltaTime;//Decrease the currentRPM to minRPM if Engine is On
        }

        //Check if RPM has reached MAX RPM
        if (currentRPM == maxRPM)
        {
            currentRPM = maxRPM;
        }

        //Check if Player is accelerating and if not accelerate if max RPM has been reached
        if (isEngineOn && Acceleration == 1 && currentRPM < maxRPM)
        {
            currentRPM += NeedleAccelerationForce * Time.deltaTime;
        }

    }

    private void ChangeGear()
    {
       Next = _inputManager.NextGear;
       Previous = _inputManager.BackGear;
       

        if(Next)
        {
            CurrentGear++;
        }
        if (Previous)
        {
            CurrentGear--;
        }
       
        
    }

    private void Steering()
    {
        Vector2 turning = _inputManager.Turn;
        if (turning.x == 1)
        {
            frontWheel_L_Col.steerAngle = TurningAngle * Time.deltaTime;
            frontWheel_R_Col.steerAngle = TurningAngle * Time.deltaTime;

        }
        else if (turning.x == -1)
        {
            frontWheel_L_Col.steerAngle = -TurningAngle * Time.deltaTime;
            frontWheel_R_Col.steerAngle = -TurningAngle * Time.deltaTime;
        }
        else if (turning.x == 0)
        {
            frontWheel_L_Col.steerAngle = 0 * Time.deltaTime;
            frontWheel_R_Col.steerAngle = 0 * Time.deltaTime;
        }
    }

    private void UpdateWheel(WheelCollider col, Transform tr)
    {
        Vector3 pos = tr.position;
        Quaternion rot = tr.rotation;

        col.GetWorldPose(out pos, out rot);
        tr.position = pos;
        tr.rotation = rot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Track") { IsAirborne = false; }
        if (collision.gameObject.tag == "Ground") { ResetPos(); }
        CanResetPos = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Track") { IsAirborne = true; }

    }

    private void Airborne()
    {
        if (IsAirborne)
        {
            //_rb.drag = 1;
            _rb.AddForce(-transform.up * DownForce * _rb.velocity.magnitude);
            _rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            _rb.drag = 0;
            _rb.AddForce(-transform.up * DownForce * _rb.velocity.magnitude);
            _rb.constraints = RigidbodyConstraints.None;

        }
    }

    private void ResetPos()
    {
        Debug.Log("Resetting Position");
        CanResetPos = true;
    }

}
