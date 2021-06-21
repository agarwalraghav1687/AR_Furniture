using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameObject furniture;

    [SerializeField] private ButtonManager buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private List<Item> items;

    private int curr_id = 0;

    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        loadItems();
        CreateButton();
    }


    void loadItems()
    {
        var obj_items = Resources.LoadAll("Items", typeof(Item));

        foreach(var item in obj_items)
        {
            items.Add(item as Item);
        }
    }

    void CreateButton()
    {
        foreach(Item i in items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.ItemId = curr_id;
            b.ButtonTexture = i.itemImage;
            curr_id++;
        }
    }

    public void setFurni(int id)
    {
        furniture = items[id].itemPrefab;
    }

    public GameObject getFurniture()
    {
        return furniture;
    }
}
