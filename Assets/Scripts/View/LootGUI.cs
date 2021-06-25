using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootGUI : MonoBehaviour
{

    [SerializeField]
    private Button itemSlot_0;
    [SerializeField]
    private Button itemSlot_1;
    [SerializeField]
    private Button itemSlot_2;

    private Button[] slots;

    private Dictionary<string, Sprite> itemsSpritesNames;


    private Room currentRoom;
    private Character mainCharacter;

    public void Awake()
    {
        slots = new Button[Room.MAX_LOOT];
        slots[0] = itemSlot_0;
        slots[1] = itemSlot_1;
        slots[2] = itemSlot_2;

        itemsSpritesNames = new Dictionary<string, Sprite>();

        LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite[] sprites;
        sprites = Resources.LoadAll<Sprite>("Items/");
        foreach (Sprite s in sprites)
        {
            itemsSpritesNames.Add(s.name, s);
        }
    }
    

    public void SetCharacter(Character character)
    {
        mainCharacter = character;
    }

    public void OnRoomLoaded(Room room)
    {
        if(slots == null)
        {
            return;
        }

        currentRoom = room;
        int i = 0;
        foreach (KeyValuePair<Item.ItemType, int> item in currentRoom.Loot)
        {
            if (itemsSpritesNames.ContainsKey(item.Key.ToString()))
            {
                slots[i].GetComponent<Image>().sprite = itemsSpritesNames[item.Key.ToString()];
                slots[i].GetComponentInChildren<TextMeshProUGUI>().text = item.Value.ToString();
                slots[i].gameObject.SetActive(false);
                // Copy otherwise the i is passed as a ref (black magic with lambda think it is called closure)
                int slot = i;
                slots[i].onClick.AddListener(() => { OnClickItem(slot, item.Key); });
            }
            i++;
        }
        for(int j = currentRoom.Loot.Count; j < Room.MAX_LOOT; j++)
        {
            slots[j].GetComponent<Image>().sprite = itemsSpritesNames["TRANSPARENCY"];
            slots[j].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        if(currentRoom.Monsters.Count == 0)
        {
            OnMonstersDefeated();
        }
       
    }

    public void OnMonstersDefeated()
    {
        for (int i = 0; i < currentRoom.Loot.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
        }
    }

    public void OnClickItem(int slot, Item.ItemType type)
    {
        int qty;
        if( !int.TryParse(slots[slot].GetComponentInChildren<TextMeshProUGUI>().text, out qty))
        {
            qty = 1;
        }
        int qtyTaken = mainCharacter.PickUpItem(type, qty);
        if (qtyTaken > 0)
        {
            currentRoom.RemoveLoot(type, qtyTaken);
            if (qty - qtyTaken <= 0)
            {
                slots[slot].gameObject.SetActive(false);
                slots[slot].onClick.RemoveAllListeners();
            }
            else
            {
                slots[slot].GetComponentInChildren<TextMeshProUGUI>().text = (qty - qtyTaken).ToString();
            }
        }

    }
}
