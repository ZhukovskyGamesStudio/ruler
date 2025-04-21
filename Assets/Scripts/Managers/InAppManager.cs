using UnityEngine;

public class InAppManager : MonoBehaviour { }

/*
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace IAP {
    public class InAppManager : MonoBehaviour, IDetailedStoreListener {
        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;

        public const string pMoney100 = "money_100";
        public const string pMoney500 = "money_500";
        public const string pMoney1000 = "money_1000";

        public const string pMoney100GooglePlay = "gp_money_100";
        public const string pMoney500GooglePlay = "gp_money_500";
        public const string pMoney1000GooglePlay = "gp_money_1000";

        void Start() {
            if (m_StoreController == null) {
                InitializePurchasing();
            }
        }

        public void InitializePurchasing() {
            if (IsInitialized()) {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(pMoney100, ProductType.Consumable, new IDs() { { pMoney100GooglePlay, GooglePlay.Name } });
            builder.AddProduct(pMoney500, ProductType.Consumable, new IDs() { { pMoney500GooglePlay, GooglePlay.Name } });
            builder.AddProduct(pMoney1000, ProductType.Consumable, new IDs() { { pMoney1000GooglePlay, GooglePlay.Name } });

            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized() {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyProductID(string productId) {
            try {
                if (IsInitialized()) {
                    Product product = m_StoreController.products.WithID(productId);

                    if (product != null && product.availableToPurchase) {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'",
                            product.definition
                                .id)); // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        m_StoreController.InitiatePurchase(product);
                    } else {
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                } else {
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            } catch (Exception e) {
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            Debug.Log("OnInitialized: Completed!");

            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error) {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message) {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error + "\nmessage: " + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            if (String.Equals(args.purchasedProduct.definition.id, pMoney100, StringComparison.Ordinal)) {
                int bucks = PlayerPrefs.GetInt("bucks");
                Debug.Log("+100, congrats!");
                bucks += 100;
                PlayerPrefs.SetInt("bucks", bucks);
            }

            if (String.Equals(args.purchasedProduct.definition.id, pMoney500, StringComparison.Ordinal)) {
                int bucks = PlayerPrefs.GetInt("bucks");
                Debug.Log("+500, congrats!");
                bucks += 500;
                PlayerPrefs.SetInt("bucks", bucks);
            }

            if (String.Equals(args.purchasedProduct.definition.id, pMoney1000, StringComparison.Ordinal)) {
                int bucks = PlayerPrefs.GetInt("bucks");
                Debug.Log("+1000, congrats!");
                bucks += 1000;
                PlayerPrefs.SetInt("bucks", bucks);
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureDescription: {1}",
                product.definition.storeSpecificId, failureDescription));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId,
                failureReason));
        }
    }
}*/