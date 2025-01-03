using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeSliderPositionController : MonoBehaviour
{
    public GameObject sliderPrefab; 
    public Camera cam;
    public float duration = 20f; 
    public event Action OnSliderClicked;
    private List<SliderInstance> sliderInstances = new List<SliderInstance>();

    private struct SliderInstance
    {
        public GameObject sliderObject;
        public Slider sliderComponent;
        public Coroutine countdownCoroutine;
        public Button buttonComponent;
    }

    public void MoveSliderTo(Transform target)
    {
        GameObject sliderObj = Instantiate(sliderPrefab, transform);
        Slider sliderComp = sliderObj.GetComponent<Slider>();

        if (sliderComp == null)
        {
            Debug.LogError("Slider prefab does not contain a Slider component.");
            Destroy(sliderObj);
            return;
        }

        // Position the slider relative to the spawn point and camera
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane);
        Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);
        sliderObj.transform.position = target.position + (worldCenter - target.position) * 0.1f;
        sliderObj.transform.position += cam.transform.up * 5f;
        sliderObj.transform.position += cam.transform.right * -2f;
        sliderObj.transform.LookAt(sliderObj.transform.position + cam.transform.forward);
        sliderObj.SetActive(true);

        // Add Button click listener
        Button button = sliderObj.GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                OnSliderClicked?.Invoke();
            });
        }
        else
        {
            Debug.LogError("Slider prefab does not have a Button component.");
        }

        // Start the countdown coroutine for this slider
        Coroutine coroutine = StartCoroutine(EmptySliderOverTime(sliderComp, duration));

        // Add to the list
        sliderInstances.Add(new SliderInstance
        {
            sliderObject = sliderObj,
            sliderComponent = sliderComp,
            countdownCoroutine = coroutine,
            buttonComponent = button
        });
    }

    private IEnumerator EmptySliderOverTime(Slider sliderComponent, float time)
    {
        float currentTime = 0f;
        float startValue = 1f; // Start from full

        sliderComponent.value = startValue;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            sliderComponent.value = Mathf.Lerp(startValue, 0f, currentTime / time);
            yield return null;
        }

        sliderComponent.value = 0f; // Ensure it reaches zero
    }

    public void HideAllSliders()
    {
        foreach (var sliderInstance in sliderInstances)
        {
            if (sliderInstance.countdownCoroutine != null)
            {
                StopCoroutine(sliderInstance.countdownCoroutine);
            }

            if (sliderInstance.buttonComponent != null)
            {
                sliderInstance.buttonComponent.onClick.RemoveAllListeners();
            }

            Destroy(sliderInstance.sliderObject);
        }
        sliderInstances.Clear();
    }
}
