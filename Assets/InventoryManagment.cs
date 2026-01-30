using UnityEngine;
using UnityEngine.UI;


public class InventoryManagment : MonoBehaviour
{
    public GameObject inventoryItem;
    private GameObject[] items;
    public int size = 9;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float gap = 30f;
        float center = (size - 1) / 2f;
        items = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            float xPos = (i - center) * gap;
            
            GameObject item = Instantiate(inventoryItem, transform);
            item.transform.localPosition = new Vector3(xPos, -90f, 0f);
            items[i] = item;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<size;i++){
           if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectItem(i);
                break;
            }
        }
    }

    void SelectItem(int index)
    {
        for (int i = 0; i < size; i++)
        {
            RawImage img = items[i].GetComponent<RawImage>();

            if (i == index)
                img.color = Color.red;   // highlight selected
            else
                img.color = Color.white;     // reset others
        }
    }

}
