using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    public Button yellow;
    public Button blue;
    private Button[] items;

    public Text coinsText;

	// Use this for initialization
	void Start () {
        items = new Button[2];
        items[0] = yellow;
        items[1] = blue;
        for(int i = 0; i < GameManager.gameManager.boughtItems.Length; i++)
        {
            if (GameManager.gameManager.boughtItems[i])
            {
                items[i].interactable = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        coinsText.text = "" + GameManager.gameManager.coins;
	}

    // Check if the player has enough coins
	public bool CanBuy(float price){
		if (price <= GameManager.gameManager.coins) {
			return true;
		}
		return false;
	}

    // Buy an item
	public void BuyItem(string values){
        string temp = values.Substring(0, 1);
        int item = int.Parse(temp);

        temp = values.Substring(1, values.Length - 1);
        float price = float.Parse(temp);

        Debug.Log(item + ". " + price);

        if (CanBuy(price)){
			GameManager.gameManager.coins -= price;
			GameManager.gameManager.BoughtItem(item);
            Debug.Log("Bought item " + item + " for " + price + "!");

            GameManager.gameManager.Save();
            items[item].interactable = false;
        }
    }
}
