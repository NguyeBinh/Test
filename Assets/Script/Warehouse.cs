using UnityEngine;
using System.Collections;

public class Warehouse : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab vật phẩm
    public Transform itemSpawnPoint; // Vị trí sinh vật phẩm
    private int maxItems = 5;
    private int currentItems;

    void Start()
    {
        RestockItems(); // Tạo vật phẩm ban đầu
    }

    public GameObject TakeItem()
    {
        if (currentItems > 0)
        {
            currentItems--;
            GameObject newItem = Instantiate(itemPrefab, itemSpawnPoint.position, Quaternion.identity);
            return newItem;
        }
        return null;
    }

    public void RestockItems()
    {
        StartCoroutine(RestockRoutine());
    }

    IEnumerator RestockRoutine()
    {
        yield return new WaitForSeconds(5f); // Chờ 5 giây trước khi cập nhật
        currentItems = Random.Range(1, maxItems + 1); // Random từ 1 đến 5 món
        Debug.Log("Kho hàng cập nhật: " + currentItems + " vật phẩm");
    }
}
