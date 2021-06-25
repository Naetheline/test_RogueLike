using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsGUI : MonoBehaviour {

    [SerializeField]
    private Image appleImg;
    [SerializeField]
    private TextMeshProUGUI appleQty;
    [SerializeField]
    private Image manaImg;
    [SerializeField]
    private TextMeshProUGUI manaQty;
    [SerializeField]
    private Image goldImg;
    [SerializeField]
    private TextMeshProUGUI goldQty;

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private Slider xpSlider;

    // Not working for some reasons... FIXME
    private Color disableColor = new Color(3,3,3);
    private Color enableColor = new Color(255, 255, 255);

    private Character character;


    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
    }

    public void OnInventoryChanged(Dictionary<Item.ItemType, int> inventory)
    {
        foreach(Item.ItemType type in inventory.Keys)
        {
            OnItemChanged(type, inventory[type]);
        }
    }
    public void OnItemChanged(Item.ItemType item, int qty)
    {
        switch (item)
        {
            case Item.ItemType.APPLE:
                {
                    appleQty.text = qty.ToString();
                    if(qty == 0)
                    {
                        appleImg.color = disableColor;

                    }
                    else
                    {
                        appleImg.color = enableColor;
                    }
                    break;
                }
            case Item.ItemType.MANA:
                {
                    manaQty.text = qty.ToString();
                    if (qty == 0)
                    {
                        manaImg.color = disableColor;
                    }
                    else
                    {
                        manaImg.color = enableColor;
                    }
                    break;
                }
            case Item.ItemType.COIN:
                {
                    goldQty.text = qty.ToString();
                    if (qty == 0)
                    {
                        goldImg.color = disableColor;

                    }
                    else
                    {
                        goldImg.color = enableColor;
                    }
                    break;
                }
        }
    }

    public void OnHPchanged(int hp)
    {
        hpSlider.value = hp;
    }

    public void OnXPchanged(int xp)
    {
        xpSlider.value = xp;
    }

    public void Onlevelup(int HPMax, int XPMax)
    {
        hpSlider.maxValue = HPMax;
        xpSlider.maxValue = XPMax;
    }

    public void OnClickItem(string typeText)
    {
        Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeText);
        character.UseItem(type, 1);
    }
   
}
