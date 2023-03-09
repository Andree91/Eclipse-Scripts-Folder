using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class WordSelector : MonoBehaviour
    {
        public bool isBaseWord = true;
        [SerializeField] private TextMeshProUGUI baseMessageText = null;
        [SerializeField] private TextMeshProUGUI secondMessageText = null;
        [SerializeField] private TextMeshProUGUI selectedWordText = null;
        [SerializeField] private TextMeshProUGUI wholeMessageBasePreviewText = null;
        [SerializeField] private TextMeshProUGUI wholeMessageSecondPreviewText = null;
        [SerializeField] private string finalMessage = null;
        [SerializeField] private Image gestureIcon = null;
        [SerializeField] private Image selectedGestureIcon = null;
        [SerializeField] private WordSelector[] wordButtons = new WordSelector[0];

        [SerializeField] PlayerManager player = null;

        public void ChooseBaseWord()
        {
            foreach (WordSelector wordSelector in wordButtons)
            {
                wordSelector.isBaseWord = true;
            }
        }

        public void ChooseSecondWord()
        {
            foreach (WordSelector wordSelector in wordButtons)
            {
                wordSelector.isBaseWord = false;
            }
        }

        public void SelectMessageWordFromList()
        {
            if (isBaseWord)
            {
                baseMessageText.text = selectedWordText.text;
            }
            else if (!isBaseWord)
            {
                secondMessageText.text = " " + selectedWordText.text;
            }
        }

        public void SelecGestureFromList()
        {
            gestureIcon.sprite = selectedGestureIcon.sprite;
        }

        public void ShowMessagePreview()
        {
            wholeMessageBasePreviewText.text = baseMessageText.text;
            wholeMessageSecondPreviewText.text = secondMessageText.text;
        }

        public void WriteDownMessage()
        {
            finalMessage = wholeMessageBasePreviewText.text +  wholeMessageSecondPreviewText.text;
            player.WriteDownMessage(finalMessage);
            player.inputHandler.inventoryFlag = false;
            player.uIManager.CloseSelectWindow();
            player.uIManager.CloseAllInventoryWindows();
            player.uIManager.hudWindow.SetActive(true);
            player.uIManager.ShowHUD();
        }
    }
}