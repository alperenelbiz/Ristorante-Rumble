using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject ovenMenu;
    public Transform mealListContainer;
    public GameObject mealButtonPrefab;
  
    private Oven currentOven;

    public GameObject inGameMenuPanel;
    public Button settingsButton;
    public Button exitButton;

    private bool isMenuActive = false;

    void Start()
    {
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        inGameMenuPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OpenOvenMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            inGameMenuPanel.SetActive(isMenuActive);
            Time.timeScale = isMenuActive ? 0 : 1; // Pause the game when the menu is active
        }
    }

    public void CloseOvenMenu()
    {
        ovenMenu.SetActive(false);
        ClearMealList();
    }

    void OpenOvenMenu()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) &&
            hit.collider.gameObject.CompareTag("Oven"))
        {
            Oven oven = hit.collider.GetComponent<Oven>();
            if (oven != null && !oven.IsCooking())
            {
                currentOven = oven;
                ovenMenu.SetActive(true);
                ovenMenu.transform.position = Input.mousePosition;
                PopulateMealList(oven.GetAvailableMeals());
            }
        }
    }

    void PopulateMealList(List<Meal> meals)
    {
        foreach (Meal meal in meals)
        {
            GameObject mealButton = Instantiate(mealButtonPrefab, mealListContainer);
            mealButton.GetComponentInChildren<TextMeshProUGUI>().text = meal.mealName;

            Button button = mealButton.GetComponent<Button>();
            button.onClick.AddListener(() => OnMealSelected(meal));
        }
    }

    void ClearMealList()
    {
        foreach (Transform child in mealListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void OnMealSelected(Meal meal)
    {
        if (currentOven != null)
        {
            currentOven.StartCooking(meal);
            CloseOvenMenu();
        }
    }
    void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene("Test"); 
    }

    void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
