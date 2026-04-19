using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Timer")]
    public TMP_Text breakTimerText;
    public float breakDuration = 5f * 60f;

    [Header("Currency")]
    public TMP_Text seaDollarsText;

    [Header("Shop Items")]
    public GameObject shopItemPrefab;
    public Transform shopItemsGrid;

    private float timeRemaining;
    private bool isRunning = false;

    // Define your shop items here
    private ShopItemData[] shopItems = new ShopItemData[]
    {
        new ShopItemData { itemName = "Bubble Trail",    cost = 50  },
        new ShopItemData { itemName = "Golden Scales",   cost = 100 },
        new ShopItemData { itemName = "Crown Jellyfish", cost = 200 },
        new ShopItemData { itemName = "Shark Fin Hat",   cost = 150 },
    };

    void OnEnable()
    {
        timeRemaining = breakDuration;
        isRunning = true;
        RefreshUI();
        PopulateShop();
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            //OnBreakComplete();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        breakTimerText.text = $"Break ends in: {minutes:00}:{seconds:00}";
    }

    void RefreshUI()
    {
        if (seaDollarsText != null)
            seaDollarsText.text = $"💰 {GameManager.Instance.seaDollars} Sea Dollars";
    }

    void PopulateShop()
    {
        // Clear old items first
        foreach (Transform child in shopItemsGrid)
            Destroy(child.gameObject);

        // Spawn a button for each shop item
        foreach (ShopItemData data in shopItems)
        {
            GameObject item = Instantiate(shopItemPrefab, shopItemsGrid);
            TMP_Text[] labels = item.GetComponentsInChildren<TMP_Text>();
            labels[0].text = data.itemName;
            labels[1].text = $"💰 {data.cost}";

            // Wire the buy button
            int cost = data.cost;  // capture for lambda
            string name = data.itemName;
            Button btn = item.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => BuyItem(cost, name, btn));
        }
    }

    public void BuyItem(int cost, string itemName, Button btn)
    {
        if (GameManager.Instance.seaDollars < cost) return;

        GameManager.Instance.seaDollars -= cost;
        RefreshUI();

        // Mark button as purchased
        btn.interactable = false;
        TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
        if (btnText != null) btnText.text = "Owned";

        Debug.Log($"Bought: {itemName}");
        // TODO: apply cosmetic to your fish here
    }

    public void SkipBreak()
    {
        isRunning = false;
        //OnBreakComplete();
    }

    /*
    void OnBreakComplete()
    {
        // Check if all tasks are done
        Task nextTask = GameManager.Instance.tasks.Find(t => !t.isComplete);
        if (nextTask == null)
            GameManager.Instance.SwitchState(GameState.TaskList); // all done!
        else
            GameManager.Instance.SwitchState(GameState.Battle);   // back to work
    }
    */
}

[System.Serializable]
public class ShopItemData
{
    public string itemName;
    public int cost;
}