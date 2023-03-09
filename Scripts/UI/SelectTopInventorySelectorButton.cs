using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class SelectTopInventorySelectorButton : MonoBehaviour
    {
        public List<Button> inventoryButtons = new List<Button>();

        public void SelectThisButton(int currentIndex)
        {
            inventoryButtons[currentIndex].Select();
        }
    }
}
