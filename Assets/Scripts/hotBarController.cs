using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hotBarController : MonoBehaviour
{

    [SerializeField]
    private GameObject card;

    private List<GameObject> cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AddCard();
        AddCard();
        AddCard();
        AddCard();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = Input.mousePosition;
            // This is some nasty garbage collection going on here!
            List<RaycastResult> mouseUIResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, mouseUIResults);

            GameObject card = null;
            foreach (var result in mouseUIResults)
            {
                if (result.gameObject.tag == "Card")
                {
                    card = result.gameObject;
                    break;
                }
            }

            if (card != null)
            {
                cards.Remove(card);
                Destroy(card);
                AlignCards();
            }
        }
        
    }

    public void AddCard()
    {

        GameObject card = Instantiate(this.card, new Vector3(-100f, -100f, 0f), Quaternion.identity);
        cards.Add(card);
        card.transform.SetParent(transform);
        AlignCards();
    }

    private void AlignCards()
    {
        float hotbarWidth = gameObject.GetComponent<RectTransform>().rect.width;
        float hotbarHeight = gameObject.GetComponent<RectTransform>().rect.height;

        Vector3 center = new Vector3(hotbarWidth / 2, hotbarHeight / 2, 0f);
        Vector3 offset = new Vector3(80f, 0f, 0f);

        float cardAmount = cards.Count;

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = cards[i];
            card.transform.position = center + i * offset - (cardAmount - 1)/2 * offset;
        }


    }

}

