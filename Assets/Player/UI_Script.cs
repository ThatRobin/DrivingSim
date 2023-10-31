using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTutorial.Manager;

public class UI_Script : MonoBehaviour
{
    // Getting Values Input
    [SerializeField] private PMovement Car;
    [SerializeField] private InputManager inputManager;

    // Needles UI variables
    public GameObject GNeedle;
    public GameObject SNeedle;
    public float Gauge;
    public float Speed;

    //Rotations of the needles
    private float GstartPosition = 80f, GendPosiotion = -140;
    private float SMeterStartP = 140f, SMeterEndP = -140f;

    private float Max_NeedleSpeed;
    private float GaugeNeedleSpeed;
    private float SpeedNeedle;
    private float GetSpeedMomentum;

    // UI Stats variable
    public TextMeshProUGUI DisplaySpeed;
    public TextMeshProUGUI DisplayRev;
    public TextMeshProUGUI DisplayGear;

    public GameObject[] Gears;

    private void Update()
    {
        //Display the UI information
        DisplaySpeed.text = "Speed: "+Car.Current_Speed.ToString("0") +"Km/h";
        DisplayRev.text = "Engine Rev: "+Car.currentRPM.ToString("0,00")+"RPM";
        //DisplayGear.text = "Gear: " +Car.CurrentGear.ToString();

        GetSpeedGauge();
        GetSpeed();
        Input(); 
        GearHandler();

        //Handle Gauge Acceleration and Deceleration
        if (GaugeNeedleSpeed > Max_NeedleSpeed)
        {
            GaugeNeedleSpeed = Max_NeedleSpeed;
        }

        if (GaugeNeedleSpeed < 0f)
        {
            GaugeNeedleSpeed = 5f;
        }

        if (Car.isEngineOn)
        {
            float EngineOff = GstartPosition - 80f;
            GstartPosition -= EngineOff * Time.deltaTime;
        }else
        {
            float EngineOn = GstartPosition - 140f;
            GstartPosition -= EngineOn * Time.deltaTime;
        }

        if(SpeedNeedle > 200)
        {
            SpeedNeedle = 200;
        }

        //Handles rotating the Gauge needle
        GNeedle.transform.eulerAngles = new Vector3(0f, 0f, GetSpeedGauge());
        SNeedle.transform.eulerAngles = new Vector3(0f, 0f, GetSpeed());

    }

    private void Awake()
    {
        GaugeNeedleSpeed = 0f;
        SpeedNeedle = 0f;
        Max_NeedleSpeed = Car.maxRPM;
    }

    private void GearHandler()
    {
        //int CurrentGear = Car.CurrentGear;
        
        // In Development 
    }

    private float GetSpeedGauge()
    {
        float totalangleS = GstartPosition - GendPosiotion;
        float SpeendNld = GaugeNeedleSpeed / Max_NeedleSpeed;

        return GstartPosition - SpeendNld * totalangleS;
    }

    private float GetSpeed()
    {
        float totalangleS = SMeterStartP - SMeterEndP;
        float SpeedNld = SpeedNeedle / Max_NeedleSpeed;

        return SMeterStartP - SpeedNld * totalangleS;
    }

    private void Input()
    {
        
        GetSpeedMomentum = Car.Current_Speed;

        float SetSpeed = GetSpeedMomentum + SMeterStartP;
        SpeedNeedle += SetSpeed * Time.deltaTime;

        float Accelerate = inputManager.Acceleration;
        if(Accelerate == 1)
        {
            float acceletion = Car.currentRPM;
            GaugeNeedleSpeed += acceletion * Time.deltaTime;
        }
        else if(Accelerate != 1&& Car.currentRPM > 0)
        { 
            float deceletion = Car.currentRPM;
            GaugeNeedleSpeed -= deceletion * Time.deltaTime;
        }
        else
        {
            GstartPosition = 145;
        }
    }

}
