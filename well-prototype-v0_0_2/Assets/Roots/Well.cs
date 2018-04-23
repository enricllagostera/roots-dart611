using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorful;

public class Well : Singleton<Well>
{
    public AnimationCurve movementCurve;
    public float layerOffset;
    public Gradient gradient;
    public Garden[] gardenLayers;
    public float loopTime;
    public float timer;

    [Range(0.01f, 5f)]
    public float timeScale = 1f;
    public float timeScaleMin, timeScaleMax;

    public float activeLayerMin;
    public float activeLayerMax;
    public float wellDepth;
    public float waterRate;
    public KeyCode[] possibleWaterKeys;
    public float inputStagnationTimer;
    public float inputStagnationThreshold;
    public float pacingFactor;

    public ScreenFlashFX inputFXRoot;
    public ScreenFlashFX inputFXWater;

    public List<KeyCode> inputPool;

    public float inputFXCooldown;
    private float _inputFXtimer;

    public Glitch glitchFX;
    public float glitchDurationBaserate;

    [Header("SFX")]
    public RandomSampleSFX newPlantSFX;
    public RandomSampleSFX glitchSFX;
    public RandomSampleSFX rootConnectionSFX;
    public RandomSampleSFX waterConnectionSFX;
    public AmbienceSFX rainAmbienceSFX;

    [Header("Instructions")]
    public float instructionInterval;
    public float instructionFadeFactor;
    public SpriteRenderer instructionImg;

    void Start()
    {
        gardenLayers = FindObjectsOfType<Garden>();
        // gameplay-related setup
        foreach (var layer in gardenLayers)
        {
            layer.waterKey = possibleWaterKeys[Random.Range(0, possibleWaterKeys.Length)];
        }
        layerOffset = 1f / gardenLayers.Length;
    }


    void Update()
    {
        _inputFXtimer -= Time.deltaTime;
        Time.timeScale = timeScale;
        float progress = (Time.time % loopTime).Map(0, loopTime, 0, 1f);
        float rain = 0f;

        for (int i = 0; i < gardenLayers.Length; i++)
        {
            // progress-based handling
            gardenLayers[i].gardenIndex = i;
            float layerProgress = (progress + (layerOffset * i)) % 1f;
            gardenLayers[i].progress = layerProgress;
            if (layerProgress <= 0.1f)
            {
                gardenLayers[i].resetLock = false;
            }
            if (layerProgress >= 0.99f && !gardenLayers[i].resetLock)
            {
                gardenLayers[i].resetLock = true;
                gardenLayers[i].Reset();
            }
            gardenLayers[i].transform.position = new Vector3(0, 0, movementCurve.Evaluate(layerProgress) * wellDepth);
            gardenLayers[i].baseSortingOrder = (int)(gardenLayers[i].transform.position.z * -20f);
        }

        bool waterOn = false;
        for (int j = 0; j < possibleWaterKeys.Length; j++)
        {
            if (Input.GetKey(possibleWaterKeys[j]))
            {
                waterOn = true;
                if (Input.GetKeyDown(possibleWaterKeys[j]))
                {
                    InputChangeHandler(false);
                }
            }
        }

        for (int i = 0; i < gardenLayers.Length; i++)
        {
            if (waterOn)
            {
                gardenLayers[i].biome.humidity = Mathf.Clamp01(gardenLayers[i].biome.humidity + waterRate * Time.deltaTime);
            }
            rain += gardenLayers[i].biome.humidity / gardenLayers.Length;
        }
        rainAmbienceSFX.SetVolume(rain);
        UpdateInputMetrics();
    }



    void UpdateInputMetrics()
    {
        inputStagnationTimer += Time.deltaTime;
        float rate = inputStagnationTimer.Map(0, inputStagnationThreshold, 0f, 1f);
        float unclampedTimeScale = Mathf.Lerp(timeScale, rate.Map(0f, 1f, timeScaleMin, timeScaleMax), Time.deltaTime * pacingFactor);
        timeScale = Mathf.Clamp(unclampedTimeScale, timeScaleMin, timeScaleMax);

        if (inputStagnationTimer >= instructionInterval)
        {
            Color color = Color.white;
            color.a = inputStagnationTimer.Map(instructionInterval, instructionInterval + 2f, 0, 1f);
            instructionImg.color = color;
        }
        else
        {
            Color color = Color.white;
            color.a = Mathf.Lerp(instructionImg.color.a, Color.clear.a, Time.deltaTime * instructionFadeFactor);
            instructionImg.color = color;

        }
    }



    public void InputChangeHandler(bool isRoot)
    {
        inputStagnationTimer = 0;
        if (_inputFXtimer <= 0)
        {
            Instantiate<ScreenFlashFX>((isRoot) ? inputFXRoot : inputFXWater, Vector3.zero, Quaternion.identity);
            if (isRoot)
            {
                rootConnectionSFX.Play(1f);
            }
            else
            {
                waterConnectionSFX.Play(1f);
            }
            _inputFXtimer = inputFXCooldown;
        }
    }

    public void PlantDeadFX(float layerProgress)
    {
        if (glitchFX.enabled)
        {

        }
        else
        {
            StartCoroutine(StartDeadFX(layerProgress));
        }
    }

    IEnumerator StartDeadFX(float duration)
    {
        glitchFX.enabled = true;
        glitchSFX.Begin(1f);
        yield return new WaitForSeconds(duration * glitchDurationBaserate);
        glitchFX.enabled = false;
        glitchSFX.Stop();
    }

    public void PlayNewPlantSFX(float value)
    {
        newPlantSFX.Play(value);
    }
}
