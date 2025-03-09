using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    public DynamicJoystick joystick; // Gán prefab Joystick từ Asset Store hoặc tự tạo
    public Animator animator;
    public Box itemBoxScript;
    public TextMeshPro moneyText;
   
    private List<GameObject> inventory = new List<GameObject>();
    public Transform carryPosition;
    private float interactRange = 2f;
    private Queue<Customer> waitingCustomers = new Queue<Customer>(); // Hàng đợi khách
    public GameObject itemBox; // Hộp hiển thị đồ


    private int money = 0;
    private int maxCarry = 5;
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemBox.SetActive(false); // Ban đầu hộp ẩn
    }

    
    void Update()
    {
        HandleMovement();
        AutoPickupFromWarehouse();
        AutoPlaceOnShelf();
        AutoCheckoutCustomers();
    }

    public void SetAnimPlayer()
    {
       if(movement != Vector3.zero)
        {
            animator.SetBool("IsMove", true);
        } else { animator.SetBool("IsMove", false); }
    }

    public void HandleMovement()
    {
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
        if(movement.magnitude > 1) movement.Normalize();
        SetAnimPlayer();
        HandleRotation(movement);
    }

    public void HandleRotation(Vector3 playerMovement)
    {
        Vector3 lookDirection = playerMovement;
        lookDirection.y = 0f;
        if(lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
        }
    }

    void AutoPickupFromWarehouse()
    {
        if (inventory.Count >= maxCarry) return;

        Warehouse warehouse = FindClosestWarehouse();
        if (warehouse != null && Vector3.Distance(transform.position, warehouse.transform.position) < interactRange)
        {
            PickUpItem(warehouse);
        }
    }

    void AutoPlaceOnShelf()
    {
        if (inventory.Count == 0) return;

        Shelf shelf = FindClosestShelf();
        if (shelf != null && Vector3.Distance(transform.position, shelf.transform.position) < interactRange)
        {
            PlaceItemOnShelf(shelf);
        }
    }

    void AutoCheckoutCustomers()
    {
        if (waitingCustomers.Count > 0)
        {
            Customer customer = waitingCustomers.Dequeue();
            int earnedMoney = customer.PayAndLeave();
            money += earnedMoney;
            customer.SetDestroy();
            moneyText.text = " " + money;
        }
    }

    void PickUpItem(Warehouse warehouse)
    {
        GameObject item = warehouse.TakeItem();
        if (item != null)
        {
            inventory.Add(item);
            item.transform.SetParent(carryPosition);
            item.transform.localPosition = new Vector3(0, inventory.Count * 0.2f, 0);

            itemBox.SetActive(true);
            itemBoxScript.SetItemCount(inventory.Count); // Cập nhật hiển thị vật phẩm
        }
    }

    void PlaceItemOnShelf(Shelf shelf)
    {
        if (inventory.Count == 0) return;

        GameObject item = inventory[0];
        inventory.RemoveAt(0);
        shelf.AddItem(inventory.Count);

        itemBoxScript.SetItemCount(inventory.Count); // Cập nhật hiển thị vật phẩm

        if (inventory.Count == 0)
            itemBox.SetActive(false); // Ẩn hộp khi hết đồ
    }

    public void AddCustomerToQueue(Customer customer)
    {
        waitingCustomers.Enqueue(customer);
    }

    Warehouse FindClosestWarehouse()
    {
        Warehouse[] warehouses = FindObjectsOfType<Warehouse>();
        foreach (var wh in warehouses)
        {
            if (Vector3.Distance(transform.position, wh.transform.position) < interactRange)
                return wh;
        }
        return null;
    }

    Shelf FindClosestShelf()
    {
        Shelf[] shelves = FindObjectsOfType<Shelf>();
        foreach (var sh in shelves)
        {
            if (Vector3.Distance(transform.position, sh.transform.position) < interactRange)
                return sh;
        }
        return null;
    }

}


