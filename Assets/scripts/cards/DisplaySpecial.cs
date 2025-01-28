using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplaySpecial : MonoBehaviour
{
    [SerializeField]
    private GameObject GameHandler;
    private SpecialCardsList SpecialCards;
    [SerializeField]
    private float spaicingAngle, radius;
    private void Start()
    {
        SpecialCards= GameHandler.GetComponent<SpecialCardsList>();
    }
    public void Draw(List<string> cards)
    {
        int tracker = 0;
        GameObject card;
        Vector3 position;
        Quaternion rotation = Quaternion.Euler(0, 0, 2);
        //clears current cards
        if (transform.childCount>0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (cards.Count == 2)
        {
           position = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * spaicingAngle/2 * tracker) * radius,
                Mathf.Cos(Mathf.Deg2Rad * spaicingAngle/2 * tracker) * radius,
                0
                );
            card = Instantiate(SpecialCards.SpecialCardsUi[cards[0]], position, rotation, transform);
            card.transform.up = transform.position - card.transform.position;

            position = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * spaicingAngle / 2 * tracker) * -radius,
                Mathf.Cos(Mathf.Deg2Rad * spaicingAngle / 2 * tracker) * radius,
                0
                );
            card.transform.up = transform.position - card.transform.position;
        }
        else { 
        cards.ForEach(c =>
        {
            GameObject curCard = SpecialCards.SpecialCardsUi.Where(s => s.Key.Equals(c)).First().Value;
            if (tracker == 0)
            {
                position = new Vector3(
                0,radius,0
                );
            }
            else if(tracker%2==0)
            {
                position = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * spaicingAngle * tracker) * radius,
                Mathf.Cos(Mathf.Deg2Rad * spaicingAngle * tracker) * radius,
                0
                );
            }
            else
            {
                position = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * spaicingAngle * tracker) * -radius,
                Mathf.Cos(Mathf.Deg2Rad * spaicingAngle * tracker) * radius,
                0
                );
            }
            card = Instantiate(curCard, position, rotation, transform);
            card.transform.up = transform.position - card.transform.position;
        });
        }
    }
}
