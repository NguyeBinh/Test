using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    public Transform shelfPosition;
    public Transform checkoutCounter;
    public GameObject itemBox;

    private Rigidbody rb;
    private int requiredItems;
    private int currentItems = 0;
    private float speed = 2f; // Tốc độ di chuyển
    private bool atShelf = false;
    private bool atCheckout = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Không bị rơi
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Không bị xoay khi va chạm

        requiredItems = Random.Range(1, 6);
        itemBox.SetActive(false);

        StartCoroutine(MoveToTarget(shelfPosition.position, () =>
        {
            atShelf = true;
            StartCoroutine(TakeItems());
        }));
    }

    IEnumerator MoveToTarget(Vector3 target, System.Action onArrive)
    {
        while (Vector3.Distance(transform.position, target) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        onArrive?.Invoke();
    }

    IEnumerator TakeItems()
    {
        while (currentItems < requiredItems)
        {
            yield return new WaitForSeconds(1f);
            currentItems++;
            itemBox.SetActive(true);
            UpdateBoxDisplay();
        }

        StartCoroutine(MoveToTarget(checkoutCounter.position, () =>
        {
            atCheckout = true;
        }));
    }

    void OnTriggerEnter(Collider other) {
        Player player = other.GetComponent<Player>(); // Kiểm tra xem object có PlayerController không
        if (atCheckout && player != null)
        {
            PayAndLeave();
        }
    }

    public int PayAndLeave()
    {
        int total = currentItems * 5000;
        return total;

    }
    public void SetCheckoutCounter(Transform counter)
    {
        checkoutCounter = counter;
    }
    public void SetDestroy()
    {
        Destroy(gameObject);
    }

    void UpdateBoxDisplay()
    {
        itemBox.transform.localScale = Vector3.one * (0.5f + 0.1f * currentItems);
    }
}
