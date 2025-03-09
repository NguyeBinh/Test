using UnityEngine;
using System.Collections.Generic;

public class Shelf : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab của vật phẩm
    public Transform itemSpawnPoint; // Điểm spawn vật phẩm trên kệ
    private int maxCapacity = 10;
    private List<GameObject> items = new List<GameObject>();

    void Start()
    {
        // Tạo sẵn một số vật phẩm trên kệ
        for (int i = 0; i < maxCapacity / 2; i++)
        {
            AddItem(5);
        }
    }

    public bool AddItem(int itemUpdate )
    {
        if (items.Count < itemUpdate)
        {
            GameObject newItem = Instantiate(itemPrefab, itemSpawnPoint.position + new Vector3(0, items.Count * 0.2f, 0), Quaternion.identity, transform);
            items.Add(newItem);
            return true;
        }
        return false;
    }

    public GameObject RemoveItem()
    {
        if (items.Count > 0)
        {
            GameObject item = items[0];
            items.RemoveAt(0);
            return item;
        }
        return null;
    }
}
