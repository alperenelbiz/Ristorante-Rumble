using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class ItemPlacePair
{
    public string itemTag;
    public string placeTag;
}

public class PlayerKitchenMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private List<ItemPlacePair> itemPlacePairs;
    [SerializeField] private Material highlightMaterial;

    private Customer customer;
    private Rigidbody rb;
    private Vector3 input;
    private GameObject heldItem;
    private GameObject highlightedObject;
    private Material originalMaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!isLocalPlayer)
            this.enabled = false;
    }

    void Update()
    {
        GetInput();
        HandleItemInteraction();
        HandleHighlighting();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        input = new Vector3(moveX, 0f, moveZ).normalized;
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + input * movementSpeed * Time.fixedDeltaTime);

        if (input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    private void HandleItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldItem == null)
            {
                CmdGrabItem();
            }
            else
            {
                CmdPlaceItem();
            }
        }
    }

    [Command]
    private void CmdGrabItem()
    {
        Vector3 startPoint = transform.position + Vector3.up * 1.5f;

        float radius = 0.5f;
        float distance = 2f;

        RaycastHit hit;
        if (Physics.SphereCast(startPoint, radius, transform.forward, out hit, distance))
        {
            foreach (var pair in itemPlacePairs)
            {
                if (hit.collider.CompareTag(pair.itemTag))
                {
                    RpcHoldItem(hit.collider.gameObject);
                    break;
                }
            }
        }
    }


    [Command]
    private void CmdPlaceItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            foreach (var pair in itemPlacePairs)
            {
                if (heldItem != null && heldItem.CompareTag(pair.itemTag) && hit.collider.CompareTag(pair.placeTag))
                {
                    RpcPlaceItem(hit.collider.gameObject);

                    if (pair.itemTag == "Meal" && pair.placeTag == "Customer")
                    {
                        customer.ReceiveFood(heldItem);
                    }

                    break;
                }
            }
        }
    }

    [ClientRpc]
    private void RpcHoldItem(GameObject item)
    {
        heldItem = item;
        heldItem.transform.SetParent(holdPosition);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
    }

    [ClientRpc]
    private void RpcPlaceItem(GameObject place)
    {
        heldItem.transform.SetParent(null);
        heldItem.transform.position = place.transform.position;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem = null;
    }

    private void HandleHighlighting()
    {
        Vector3 startPoint = transform.position + Vector3.up * 1.5f;

        float radius = 0.5f;
        float distance = 2f;

        RaycastHit hit;
        if (Physics.SphereCast(startPoint, radius, transform.forward, out hit, distance))
        {
            foreach (var pair in itemPlacePairs)
            {
                if (hit.collider.CompareTag(pair.itemTag) || (heldItem != null && hit.collider.CompareTag(pair.placeTag)))
                {
                    HighlightObject(hit.collider.gameObject);
                    return;
                }
            }
        }
        ClearHighlight();
    }


    private void HighlightObject(GameObject obj)
    {   
        if (highlightedObject != obj)
        {
            ClearHighlight();

            highlightedObject = obj;
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
                renderer.material = highlightMaterial;
            }
        }
    }

    private void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            highlightedObject = null;
            originalMaterial = null;
        }
    }
}