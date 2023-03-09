using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class UIManager : MonoBehaviour
    {
        public PlayerManager player;
        public EquipmentWindowUI equipmentWindowUI;
        public ItemStatsWindowUI itemStatsWindowUI;
        public QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public GameObject crossHair;
        public TextMeshProUGUI soulCountText;
        public GameObject staticEffects;
        public StaticEffectsUI staticEffects01UI;
        public StaticEffectsUI staticEffects02UI;
        public StaticEffectsUI staticEffects03UI;
        public StaticEffectsUI staticEffects04UI;
        public StaticEffectsUI staticEffects05UI;
        public StaticEffectsUI staticEffects06UI;
        public StaticEffectsUI staticEffects07UI;

        public HUDWindowUI hud;
        public float hideHUDTimer = 10.0f;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject ammoQuickSlotsParent;
        public GameObject topInventorySelectorWindow;
        public SelectTopInventorySelectorButton topInventorySelectorButton;
        public GameObject weaponInventoryWindow;
        public GameObject rangedAmmoInventoryWindow;
        public GameObject headEquipmentInventoryWindow;
        public GameObject bodyEquipmentInventoryWindow;
        public GameObject handEquipmentInventoryWindow;
        public GameObject legEquipmentInventoryWindow;
        public GameObject consumableInventoryWindow;
        public GameObject amuletInventoryWindow;
        public GameObject itemStatsWindow;
        public GameObject messageWordListWindow;
        public GameObject gameSettingsWindow;
        public GameObject levelUpWindow;
        public GameObject mapWindow;
        public GameObject messageScreenWindow;
        public GameObject readMessageWindow;
        public GameObject loadingWindow;
        public Slider loadingProgress;

        [Header("Equipment Window Slot Selected")]
        public bool rightHand01Selected;
        public bool rightHand02Selected;
        public bool rightHand03Selected;
        public bool leftHand01Selected;
        public bool leftHand02Selected;
        public bool leftHand03Selected;
        public bool weaponSlotIsSelected;

        public bool bowArrowSlot01Selected;
        public bool bowArrowSlot02Selected;
        public bool crossBowArrowSlot01Selected;
        public bool crossBowArrowSlot02Selected;
        public bool arrowSlotIsSelected;

        public bool headEquipmentSlotSelected;
        public bool bodyEquipmentSlotSelected;
        public bool handEquipmentSlotSelected;
        public bool legEquipmentSlotSelected;

        public bool amuletSlot01Selected;
        public bool amuletSlot02Selected;
        public bool amuletSlot03Selected;
        public bool amuletSlot04Selected;
        public bool amuletSlotIsSelected;

        public bool quickSlot01Selected;
        public bool quickSlot02Selected;
        public bool quickSlot03Selected;
        public bool quickSlot04Selected;
        public bool quickSlot05Selected;
        public bool quickSlot06Selected;
        public bool quickSlot07Selected;
        public bool quickSlot08Selected;
        public bool quickSlot09Selected;
        public bool quickSlot10Selected;
        public bool quickSlotIsSelected;

        [Header("Inventory Window Is Open")]
        public bool isInInventoryWindow;
        int currentInventoryWindowIndex;
        [SerializeField] InventoryWindowUI[] inventoryWindows = new InventoryWindowUI[1];
        public RectTransform rt;
        Vector3 originalRtPos;

        [Header("Pop Ups")]
        public BonfireLitPopUpUI bonfireLitPopUpUI;
        public DeathPopUp deathPopUp;

        [Header("Ammo HUD Slots")]
        public RectTransform ammoRT;
        public float ammoRTOriginalX;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;
        int weaponInventoryIndex = 1;
        public GameObject weaponInventoryDropMenu;
        public WeaponItem inventoryWeaponItemBeingUsed;

        [Header("Ranged Ammo Inventory")]
        public GameObject rangedAmmoInventorySlotPrefab;
        public Transform rangedAmmoInventorySlotsParent;
        RangedAmmoInventorySlot[] rangedAmmoInventorySlots;
        int rangedAmmoInventoryIndex = 7;
        public GameObject rangedAmmoInventoryDropMenu;
        public GameObject useSelectedRangedAmmoItemDropMenu;
        public RangedAmmoItem inventoryRangedAmmoItemBeingUsed;

        [Header("Head Equipment Inventory")]
        public GameObject headEquipmentInventorySlotPrefab;
        public Transform headEquipmentInventorySlotParent;
        HeadEquipmentInventorySlot[] headEquipmentInventorySlots;
        int headEquipmentInventoryIndex = 2;
        public GameObject headEquipmentInventoryDropMenu;
        public HelmetEquipment inventoryHelmetItemBeingUsed;

        [Header("Body Equipment Inventory")]
        public GameObject bodyEquipmentInventorySlotPrefab;
        public Transform bodyEquipmentInventorySlotParent;
        BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;
        int bodyEquipmentInventoryIndex = 3;
        public GameObject bodyEquipmentInventoryDropMenu;
        public TorsoEquipment inventoryBodyItemBeingUsed;

        [Header("Hand Equipment Inventory")]
        public GameObject handEquipmentInventorySlotPrefab;
        public Transform handEquipmentInventorySlotParent;
        HandEquipmentInventorySlot[] handEquipmentInventorySlots;
        int handEquipmentInventoryIndex = 4;
        public GameObject handEquipmentInventoryDropMenu;
        public HandEquipment inventoryHandItemBeingUsed;

        [Header("Leg Equipment Inventory")]
        public GameObject legEquipmentInventorySlotPrefab;
        public Transform legEquipmentInventorySlotParent;
        LegEquipmentInventorySlot[] legEquipmentInventorySlots;
        int legEquipmentInventoryIndex = 5;
        public GameObject legEquipmentInventoryDropMenu;
        public LegEquipment inventoryLegItemBeingUsed;

        [Header("Amulet Inventory")]
        public GameObject amuletInventorySlotPrefab;
        public Transform amuletInventorySlotsParent;
        AmuletInventorySlot[] amuletInventorySlots;
        int amuletInventoryIndex = 6;
        public GameObject amuletInventoryDropMenu;
        public AmuletItem inventoryAmuletItemBeingUsed;

        [Header("Consumable Inventory")]
        public GameObject consumableInvenrorySlotPrefab;
        public Transform consumableInventorySlotsParent;
        ConsumablesInventorySlot[] consumablesInventorySlots; // Change back to private
        public GameObject consumableInventoryDropMenu;
        public GameObject useSelectedConsumablesDropMenu;
        public bool usingThroughInventory;
        public ConsumableItem inventoryConsumableItemBeingUsed;
        int consumableInventoryIndex = 0;

        

        void Awake() 
        {
            player = FindObjectOfType<PlayerManager>();
            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
            hud = hudWindow.GetComponentInChildren<HUDWindowUI>();
            //equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
            //itemStatsWindowUI = FindObjectOfType<ItemStatsWindowUI>();

            topInventorySelectorButton = topInventorySelectorWindow.GetComponentInChildren<SelectTopInventorySelectorButton>();
            rt = itemStatsWindow.GetComponentInChildren<Image>().gameObject.GetComponent<RectTransform>();
            originalRtPos = new Vector3(rt.position.x, rt.position.y, rt.position.z);
            Vector3 originalRtDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y);

            ammoRT = ammoQuickSlotsParent.GetComponent<RectTransform>();
            ammoRTOriginalX = ammoRT.position.x;

            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();

            rangedAmmoInventorySlots = rangedAmmoInventorySlotsParent.GetComponentsInChildren<RangedAmmoInventorySlot>();

            headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();

            amuletInventorySlots = amuletInventorySlotsParent.GetComponentsInChildren<AmuletInventorySlot>();

            consumablesInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumablesInventorySlot>();

            //bonfireLitPopUpUI = GetComponentInChildren<BonfireLitPopUpUI>();
        }

        void Start() 
        {
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(player.playerInventoryManager);

            equipmentWindowUI.LoadRangedAmmoOnEquipmentScreen(player.playerInventoryManager);

            equipmentWindowUI.LoadArmorOnEquipmentScreen(player.playerInventoryManager);

            equipmentWindowUI.LoadAmuletItemsOnEquipmentScreen(player.playerInventoryManager);

            equipmentWindowUI.LoadQuickSlotItemsOnEquipmentScreen(player.playerInventoryManager);

            if (player.playerInventoryManager.currentSpell != null)
            {
                quickSlotsUI.UpdateCurrentSpellIcon(player.playerInventoryManager.currentSpell);
            }
            
            if (player.playerInventoryManager.currentConsumable != null)
            {
                quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
            }

            if (player.playerInventoryManager.currentAmmo01 != null)
            {
                quickSlotsUI.UpdateAmmoQuickSlotsUI(player.playerInventoryManager.currentAmmo01, true);
            }

            if (player.playerInventoryManager.currentAmmo02 != null)
            {
                quickSlotsUI.UpdateAmmoQuickSlotsUI(player.playerInventoryManager.currentAmmo02, false);
            }

            soulCountText.text = player.playerStatsManager.currentSoulCount.ToString();
            if (player.isInCharacterCreator)
            {
                HideHUD();
            }
        }

        public void UptadeUI()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < player.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Head Equipment Inventory Slots
            for (int i = 0; i < headEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.headEquipmentInventory.Count)
                {
                    if (headEquipmentInventorySlots.Length < player.playerInventoryManager.headEquipmentInventory.Count)
                    {
                        Instantiate(headEquipmentInventorySlotPrefab, headEquipmentInventorySlotParent);
                        headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                    }
                    headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
                }
                else
                {
                    headEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion

            #region Body Equipment Inventory Slots
            for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
                {
                    if (bodyEquipmentInventorySlots.Length < player.playerInventoryManager.bodyEquipmentInventory.Count)
                    {
                        Instantiate(bodyEquipmentInventorySlotPrefab, bodyEquipmentInventorySlotParent);
                        bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                    }
                    bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
                }
                else
                {
                    bodyEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Hand Equipment Inventory Slots
            for (int i = 0; i < handEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.handEquipmentInventory.Count)
                {
                    if (handEquipmentInventorySlots.Length < player.playerInventoryManager.handEquipmentInventory.Count)
                    {
                        Instantiate(handEquipmentInventorySlotPrefab, handEquipmentInventorySlotParent);
                        handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
                    }
                    handEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.handEquipmentInventory[i]);
                }
                else
                {
                    handEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Leg Equipment Inventory Slots
            for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.legEquipmentInventory.Count)
                {
                    if (legEquipmentInventorySlots.Length < player.playerInventoryManager.legEquipmentInventory.Count)
                    {
                        Instantiate(legEquipmentInventorySlotPrefab, legEquipmentInventorySlotParent);
                        legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                    }
                    legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
                }
                else
                {
                    legEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Consumable Inventory Slots
            for (int i = 0; i < consumablesInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.consumablesInventory.Count)
                {
                    if (consumablesInventorySlots.Length < player.playerInventoryManager.consumablesInventory.Count)
                    {
                        Instantiate(consumableInvenrorySlotPrefab, consumableInventorySlotsParent);
                        consumablesInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumablesInventorySlot>();
                    }
                    consumablesInventorySlots[i].AddItem(player.playerInventoryManager.consumablesInventory[i]);
                }
                else
                {
                    consumablesInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Ranged Ammo Inventory Slots
            for (int i = 0; i < rangedAmmoInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.rangedAmmoItemsInventory.Count)
                {
                    if (rangedAmmoInventorySlots.Length < player.playerInventoryManager.rangedAmmoItemsInventory.Count)
                    {
                        Instantiate(rangedAmmoInventorySlotPrefab, rangedAmmoInventorySlotsParent);
                        rangedAmmoInventorySlots = rangedAmmoInventorySlotsParent.GetComponentsInChildren<RangedAmmoInventorySlot>();
                    }
                    rangedAmmoInventorySlots[i].AddItem(player.playerInventoryManager.rangedAmmoItemsInventory[i]);
                }
                else
                {
                    rangedAmmoInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Amulet Inventory Slots
            for (int i = 0; i < amuletInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.amuletsInventory.Count)
                {
                    if (amuletInventorySlots.Length < player.playerInventoryManager.amuletsInventory.Count)
                    {
                        Instantiate(amuletInventorySlotPrefab, amuletInventorySlotsParent);
                        amuletInventorySlots = amuletInventorySlotsParent.GetComponentsInChildren<AmuletInventorySlot>();
                    }
                    amuletInventorySlots[i].AddItem(player.playerInventoryManager.amuletsInventory[i]);
                }
                else
                {
                    amuletInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion

        }

        public void HideHUD()
        {
            hud.quickSlots.SetActive(false);
            hud.soulCount.SetActive(false);
            hud.statsBars.SetActive(false);
            // hudWindow.GetComponentInChildren<QuickSlotsUI>().gameObject.SetActive(false);
            // hudWindow.GetComponentInChildren<SoulCountBar>().gameObject.SetActive(false);
            // hudWindow.GetComponentInChildren<StatBars>().gameObject.SetActive(false);
        }

        public void ShowHUD()
        {
            hud.quickSlots.SetActive(true);
            hud.soulCount.SetActive(true);
            hud.statsBars.SetActive(true);
            hideHUDTimer = 10.0f;
            // hudWindow.GetComponentInChildren<QuickSlotsUI>().gameObject.SetActive(true);
            // hudWindow.GetComponentInChildren<SoulCountBar>().gameObject.SetActive(true);
            // hudWindow.GetComponentInChildren<StatBars>().gameObject.SetActive(true);
        }

        // public void UpdateCurrentStaticEffectsUI()
        // {
        //     if (staticEffects.activeInHierarchy)
        //     {
                
        //     }
        //     // if (effect.effectIcon != null)
        //     // {
        //     //     staticEffectsUI.gameObject.SetActive(true);
        //     //     effect.effectIcon = staticEffectsUI.effectImage.sprite;
        //     // }
        //     // else
        //     // {
        //     //     staticEffectsUI.effectImage.sprite = null;
        //     //     staticEffectsUI.gameObject.SetActive(false);
        //     // }
        // }

        public void CheckWhichEquipmentSlotIsSelected()
        {
            if (quickSlotIsSelected)
            {
                foreach (ConsumablesInventorySlot consumablesInventorySlot in consumablesInventorySlots)
                {
                    consumablesInventorySlot.UnEquipThisItem();
                }
            }
            else if (weaponSlotIsSelected)
            {
                foreach (WeaponInventorySlot weaponInventorySlot in weaponInventorySlots)
                {
                    weaponInventorySlot.UnEquipThisItem();
                }

                // DIFFERENT IDEA:
                // foreach (WeaponEquipmentSlotsUI weaponEquipmentSlotsUI in equipmentWindowUI.weaponEquipmentSlotsUI)
                // {
                //     Debug.Log(("Inside foreach"));
                //     weaponEquipmentSlotsUI.UnEquipThisItem();
                // }
            }
            else if (headEquipmentSlotSelected)
            {
                foreach (HeadEquipmentInventorySlot headEquipmentInventorySlot in headEquipmentInventorySlots)
                {
                    headEquipmentInventorySlot.UnEquipThisItem();
                }
            }
            else if  (bodyEquipmentSlotSelected)
            {
                foreach (BodyEquipmentInventorySlot bodyEquipmentInventorySlot in bodyEquipmentInventorySlots)
                {
                    bodyEquipmentInventorySlot.UnEquipThisItem();
                }
            }
            else if (handEquipmentSlotSelected)
            {
                foreach (HandEquipmentInventorySlot handEquipmentInventorySlot in handEquipmentInventorySlots)
                {
                    handEquipmentInventorySlot.UnEquipThisItem();
                }
            }
            else if (legEquipmentSlotSelected)
            {
                foreach (LegEquipmentInventorySlot legEquipmentInventorySlot in legEquipmentInventorySlots)
                {
                    legEquipmentInventorySlot.UnEquipThisItem();
                }
            }
            else if (arrowSlotIsSelected)
            {
                foreach (RangedAmmoInventorySlot rangedAmmoInventorySlot in rangedAmmoInventorySlots)
                {
                    rangedAmmoInventorySlot.UnEquipThisItem();
                }
            }
            else if (amuletSlotIsSelected)
            {
                foreach (AmuletInventorySlot amuletInventorySlot in amuletInventorySlots)
                {
                    amuletInventorySlot.UnEquipThisItem();
                }
            }
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            isInInventoryWindow = false;
            // ChangeItemStatsWindowSameAsEquipmentWindow();
            // ChangeSizeOfInventoryWindowsBackToOriginal();
            player.inputHandler.inventoryFlag = false;

            topInventorySelectorWindow.SetActive(false);

            weaponInventoryWindow.SetActive(false);

            rangedAmmoInventoryWindow.SetActive(false);
            
            // foreach (WeaponInventorySlot weaponInventorySlot in weaponInventorySlots)
            // {
            //     weaponInventorySlot.CloseWeaponItemDropMenu();
            // }

            consumableInventoryWindow.SetActive(false);

            headEquipmentInventoryWindow.SetActive(false);
            bodyEquipmentInventoryWindow.SetActive(false);
            handEquipmentInventoryWindow.SetActive(false);
            legEquipmentInventoryWindow.SetActive(false);

            amuletInventoryWindow.SetActive(false);

            CloseAllDropMenus();
            
            // foreach (ConsumablesInventorySlot consumablesInventorySlot in consumablesInventorySlots)
            // {
            //     consumablesInventorySlot.CloseWeaponItemDropMenu();
            // }

            equipmentScreenWindow.SetActive(false);

            messageScreenWindow.SetActive(false);
            
            itemStatsWindow.SetActive(false);

            messageWordListWindow.SetActive(false);

            gameSettingsWindow.SetActive(false);
            //mapWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHand01Selected = false;
            rightHand02Selected = false;
            rightHand03Selected = false;
            leftHand01Selected = false;
            leftHand02Selected = false;
            leftHand03Selected = false;
            weaponSlotIsSelected = false;

            bowArrowSlot01Selected = false;
            bowArrowSlot02Selected = false;
            crossBowArrowSlot01Selected = false;
            crossBowArrowSlot02Selected = false;
            arrowSlotIsSelected = false;

            headEquipmentSlotSelected = false;
            bodyEquipmentSlotSelected = false;
            handEquipmentSlotSelected = false;
            legEquipmentSlotSelected = false;

            amuletSlot01Selected = false;
            amuletSlot02Selected = false;
            amuletSlot03Selected = false;
            amuletSlot04Selected = false;
            amuletSlotIsSelected = false;

            quickSlot01Selected = false;
            quickSlot02Selected = false;
            quickSlot03Selected = false;
            quickSlot04Selected = false;
            quickSlot05Selected = false;
            quickSlot06Selected = false;
            quickSlot07Selected = false;
            quickSlot08Selected = false;
            quickSlot09Selected = false;
            quickSlot10Selected = false;
            quickSlotIsSelected = false;
        }

        public void CloseAllDropMenus()
        {
            weaponInventoryDropMenu.SetActive(false);
            rangedAmmoInventoryDropMenu.SetActive(false);
            useSelectedRangedAmmoItemDropMenu.SetActive(false);
            consumableInventoryDropMenu.SetActive(false);
            useSelectedConsumablesDropMenu.SetActive(false);
            headEquipmentInventoryDropMenu.SetActive(false);
            bodyEquipmentInventoryDropMenu.SetActive(false);
            handEquipmentInventoryDropMenu.SetActive(false);
            legEquipmentInventoryDropMenu.SetActive(false);
            amuletInventoryDropMenu.SetActive(false);
        }

        public void ToggleIsInInventoryWindow()
        {
            isInInventoryWindow = !isInInventoryWindow;
            // rt.sizeDelta = new Vector2(rt.sizeDelta.x, 919.9128f);
            // rt.position = new Vector3(rt.position.x, rt.position.y - 40, rt.position.z);
        }

        // public void ChangeItemStatsWindowSameAsEquipmentWindow()
        // {
        //     rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1004.938f);
        //     rt.position = new Vector3(rt.position.x, originalRtPos.y, rt.position.z);
        // }

        // public void ChangeSizeOfInventoryWindowsBackToOriginal()
        // {
        //     foreach (InventoryWindowUI inventoryWindow in inventoryWindows)
        //     {
        //         inventoryWindow.GetComponentInChildren<InventoryWindowUIBackground>().ChangeSizeOfUIBackgroundToOriginal();
        //     }
        // }

        public void ChangeInventoryItemWindowToRight()
        {
            currentInventoryWindowIndex += 1;

            OpenCurrentInventoryWindownFromIndex();
        }

        public void ChangeInventoryItemWindowToLeft()
        {
            currentInventoryWindowIndex -= 1;

            if (currentInventoryWindowIndex == -1)
            {
                currentInventoryWindowIndex = 7;
            }

            OpenCurrentInventoryWindownFromIndex();
        }

        public void OpenCurrentInventoryWindownFromIndex()
        {
            CloseInventoryWindows();

            if (currentInventoryWindowIndex == consumableInventoryIndex) //&& weaponsInRightHandSlots[0] != null)
            {
                consumableInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateConsumableItemStats(consumablesInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == weaponInventoryIndex)
            {
                weaponInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateWeaponItemStats(weaponInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == headEquipmentInventoryIndex)
            {
                headEquipmentInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateArmorItemStats(headEquipmentInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == bodyEquipmentInventoryIndex)
            {
                bodyEquipmentInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateArmorItemStats(bodyEquipmentInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == handEquipmentInventoryIndex)
            {
                handEquipmentInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateArmorItemStats(handEquipmentInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == legEquipmentInventoryIndex)
            {
                legEquipmentInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateArmorItemStats(legEquipmentInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == rangedAmmoInventoryIndex)
            {
                rangedAmmoInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateRangedAmmoItemStats(rangedAmmoInventorySlots[0].item);
            }
            else if (currentInventoryWindowIndex == amuletInventoryIndex)
            {
                amuletInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateAmuletItemStats(amuletInventorySlots[0].item);
            }
            // else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            // {
            //     currentRightWeaponIndex = currentRightWeaponIndex + 1;
            // }
            else
            {
                currentInventoryWindowIndex = consumableInventoryIndex;
                consumableInventoryWindow.SetActive(true);
                itemStatsWindowUI.UpdateConsumableItemStats(consumablesInventorySlots[0].item);
            }

            CloseAllDropMenus();
            topInventorySelectorButton.SelectThisButton(currentInventoryWindowIndex);
        }

        // Open Inventory Windown from click event

        public void OpenConsumableInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 0;
            consumableInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateConsumableItemStats(consumablesInventorySlots[0].item);
        }

        public void OpenWeaponInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 1;
            weaponInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateWeaponItemStats(weaponInventorySlots[0].item);
        }

        public void OpenHeadEquipmentInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 2;
            headEquipmentInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateArmorItemStats(headEquipmentInventorySlots[0].item);
        }

        public void OpenBodyEquipmentInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 3;
            bodyEquipmentInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateArmorItemStats(bodyEquipmentInventorySlots[0].item);
        }

        public void OpenHandEquipmentInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 4;
            handEquipmentInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateArmorItemStats(handEquipmentInventorySlots[0].item);
        }

        public void OpenLegEquipmentInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = 5;
            legEquipmentInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateArmorItemStats(legEquipmentInventorySlots[0].item);
        }

        public void OpenRangedAmmoInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = rangedAmmoInventoryIndex;
            rangedAmmoInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateRangedAmmoItemStats(rangedAmmoInventorySlots[0].item);
        }

        public void OpenAmuletInventoryWindowFromClick()
        {
            CloseInventoryWindows();
            currentInventoryWindowIndex = amuletInventoryIndex;
            amuletInventoryWindow.SetActive(true);
            CloseAllDropMenus();
            itemStatsWindowUI.UpdateAmuletItemStats(amuletInventorySlots[0].item);
        }

        public void CloseInventoryWindows()
        {
            foreach (InventoryWindowUI inventoryWindow in inventoryWindows)
            {
                inventoryWindow.gameObject.SetActive(false);
            }
        }

        public void ActivateBonfirePopUp()
        {
            bonfireLitPopUpUI.DisplayBonfireLitPopUp();
        }

        public void ActivateDeathPopUp()
        {
            deathPopUp.DisplayDeathtPopUp();
        }

        public void DeactiveDeathPopUp()
        {
            deathPopUp.gameObject.SetActive(false);
        }

        public void OpenMessage(string message)
        {
            readMessageWindow.SetActive(true);
            readMessageWindow.GetComponentInParent<MessageUI>().UpdateMessageText(message);
        }

    }
}
