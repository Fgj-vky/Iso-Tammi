using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hotBarController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] cardPrefabs;
    [SerializeField]
    private GameObject player;

    private List<GameObject> cards = new List<GameObject>();
    private GameObject selectedCard = null;

    public int CardCount { get { return cards.Count; } }

    // Start is called before the first frame update
    void Start()
    {
        AddCard(1);
        AddCard(2);
        AddCard(3);
        AddCard(0);


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !player.GetComponent<playerScript>().paused)
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
                player.GetComponent<playerScript>().SetCardIndex(card.GetComponent<cardController>().index);

                if (selectedCard != null)
                {
                    AddCard(selectedCard.GetComponent<cardController>().index);
                    ClearSelectedCard();
                }

                //selectedCard = null;
                selectedCard = card;
                selectedCard.GetComponent<RectTransform>().localPosition = new Vector3(300f, 0f, 0f);

                cards.Remove(card);
                AlignCards();
            }
        }
        
    }

    public void AddCard(int index)
    {

        GameObject card = Instantiate(cardPrefabs[index], new Vector3(-100f, -100f, 0f), Quaternion.identity);
        cards.Add(card);
        card.transform.SetParent(transform, false);
        AlignCards();
    }

    public void AddRandomCard()
    {
        AddCard(Random.Range(0, cardPrefabs.Length));
    }

    private void AlignCards()
    {

        Vector3 center = new Vector3(0f,0f, 0f);
        Vector3 offset = new Vector3(80f, 0f, 0f);

        float cardAmount = cards.Count;

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = cards[i];
            card.GetComponent<RectTransform>().localPosition = i * offset - (cardAmount - 1)/2 * offset;
        }


    }

    public void ClearSelectedCard()
    {
        Destroy(selectedCard);
        selectedCard = null;
    }

}

