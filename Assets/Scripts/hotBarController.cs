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

    private Vector3 canvasScale; 

    // Start is called before the first frame update
    void Start()
    {
        canvasScale = gameObject.transform.parent.GetComponent<RectTransform>().localScale;


        AddCard(0);
        AddCard(0);
        AddCard(0);
        AddCard(0);


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
                player.GetComponent<playerScript>().SetCardIndex(card.GetComponent<cardController>().index);
                cards.Remove(card);
                Destroy(card);
                AlignCards();
            }
        }
        
    }

    public void AddCard(int index)
    {

        GameObject card = Instantiate(cardPrefabs[index], new Vector3(-100f, -100f, 0f), Quaternion.identity);
        cards.Add(card);
        card.transform.SetParent(transform, false);
        //card.GetComponent<RectTransform>().localScale = canvasScale;
        AlignCards();
    }

    public void AddRandomCard()
    {
        AddCard(Random.Range(0, cards.Count));
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

}

