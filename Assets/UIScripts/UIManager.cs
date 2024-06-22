using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ovenMenu;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OpenOvenMenu();
        }
    }

    public void CloseOvenMenu()
    {
        ovenMenu.SetActive(false);
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
            ovenMenu.SetActive(true);
            ovenMenu.transform.position = Input.mousePosition;
        }
    }
}
