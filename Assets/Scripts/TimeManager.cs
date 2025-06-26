using UnityEngine;
using UnityEngine.UI; 

public class TimeManager : MonoBehaviour
{
    [Header("Time Scale Settings")]
    [SerializeField] private float normalTimeScale = 1f; 
    [SerializeField] private float slowMotionTimeScale = 0.2f; 
    [SerializeField] private float slowMotionDuration = 2f; 

    [Header("Slow Motion Resource (Scale)")]
    [SerializeField] private float maxSlowMotionResource = 100f; 
    [SerializeField] private float currentSlowMotionResource; 
    [SerializeField] private float resourceDrainRate = 20f; 
    [SerializeField] private float resourceRegenRate = 10f; 
    [SerializeField] private float regenDelayAfterUse = 1f; 
    private float regenDelayTimer;

    [Header("UI References")]
    [SerializeField] private Slider slowMotionResourceSlider; 

    private bool isSlowMotionActive = false;
    private float slowMotionTimer = 0f;

    void Awake()
    {
        currentSlowMotionResource = maxSlowMotionResource;
        UpdateUISlider(); 
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (isSlowMotionActive)
        {
            currentSlowMotionResource -= resourceDrainRate * Time.unscaledDeltaTime;

            if (currentSlowMotionResource <= 0)
            {
                currentSlowMotionResource = 0; 
                DeactivateSlowMotion();
            }

            slowMotionTimer -= Time.unscaledDeltaTime;
            if (slowMotionTimer <= 0 && isSlowMotionActive) 
            {
                DeactivateSlowMotion();
            }

            regenDelayTimer = regenDelayAfterUse; 
        }
        else 
        {
            if (currentSlowMotionResource < maxSlowMotionResource)
            {
                if (regenDelayTimer > 0)
                {
                    regenDelayTimer -= Time.unscaledDeltaTime;
                }
                else
                {
                    currentSlowMotionResource += resourceRegenRate * Time.unscaledDeltaTime;
                    currentSlowMotionResource = Mathf.Min(currentSlowMotionResource, maxSlowMotionResource); 
                }
            }
        }

        UpdateUISlider(); 
    }

   
    public void ActivateSlowMotion()
    {
        if (!isSlowMotionActive && currentSlowMotionResource > 0)
        {
            Time.timeScale = slowMotionTimeScale;
            isSlowMotionActive = true;
            slowMotionTimer = slowMotionDuration; 
            Debug.Log("Slow motion activated! Time Scale: " + Time.timeScale);
        }
    }

    public void DeactivateSlowMotion()
    {
        if (isSlowMotionActive)
        {
            Time.timeScale = normalTimeScale;
            isSlowMotionActive = false;
            regenDelayTimer = regenDelayAfterUse; 
            Debug.Log("Slow motion deactivated! Time Scale: " + Time.timeScale);
        }
    }

    
    public void ToggleSlowMotion()
    {
        if (isSlowMotionActive)
        {
            DeactivateSlowMotion();
        }
        else
        {
            ActivateSlowMotion();
        }
    }

    private void UpdateUISlider()
    {
        if (slowMotionResourceSlider != null)
        {
            slowMotionResourceSlider.maxValue = maxSlowMotionResource;
            slowMotionResourceSlider.value = currentSlowMotionResource;
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}