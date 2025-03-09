using UnityEngine;
using System.Collections.Generic;

public class Box : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab của vật phẩm
    public Transform itemContainer; // Vị trí chứa vật phẩm trong hộp
    private List<GameObject> spawnedItems = new List<GameObject>(); // Danh sách vật phẩm hiển thị

    private int itemCount = 0; // Số lượng vật phẩm trong hộp

    void Start()
    {
        UpdateBoxDisplay();
    }

    // Cập nhật số lượng vật phẩm trong hộp
    public void SetItemCount(int count)
    {
        itemCount = count;
        UpdateBoxDisplay();
    }

    void UpdateBoxDisplay()
    {
        // Xóa các vật phẩm cũ trước khi tạo lại
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        // Nếu có vật phẩm, tạo object 3D tương ứng
        if (itemCount > 0)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < itemCount; i++)
            {
                GameObject newItem = Instantiate(itemPrefab, itemContainer);
                newItem.transform.localPosition = new Vector3(0, i * 0.2f, 0); // Xếp chồng vật phẩm theo chiều dọc
                spawnedItems.Add(newItem);
            }
        }
        else
        {
            gameObject.SetActive(false); // Ẩn hộp khi không có vật phẩm
        }
    }
}
