using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject customerPrefab; // Prefab khách hàng
    public Transform spawnPoint; // Vị trí spawn khách hàng
    public Transform checkoutCounter; // Gán sẵn trong Inspector
    private List<GameObject> customers = new List<GameObject>();
    private int maxCustomers = 3;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (customers.Count < maxCustomers && !isSpawning)
            {
                isSpawning = true;
                GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

                // Truyền quầy thu ngân vào Customer.cs
                Customer customerScript = newCustomer.GetComponent<Customer>();
                customerScript.SetCheckoutCounter(checkoutCounter);

                customers.Add(newCustomer);

                yield return new WaitForSeconds(5f); // Chờ 5s trước khi spawn tiếp
                isSpawning = false;
            }
            yield return null;
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        customers.Remove(customer);
        Destroy(customer);
    }
}
