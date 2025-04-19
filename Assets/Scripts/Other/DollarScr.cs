using UnityEngine;

public class DollarScr : MonoBehaviour {
    private DeckConfig _config;

    public void SetData(DeckConfig config) {
        _config = config;
    }

    public void ToShop() {
        MainMenuButtons.Instance.To(8);
        ShopScript.Instance.OpenBuyOffer(_config);
    }
}