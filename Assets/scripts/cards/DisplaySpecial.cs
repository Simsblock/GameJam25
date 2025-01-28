using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class DisplaySpecial : MonoBehaviour
{
    [SerializeField]
    private GameObject GameHandler,ViewField;
    private SpecialCardsList SpecialCards;
    [SerializeField]
    private int perRow;
    [SerializeField]
    private float spaicingX, spaicingY;
    [SerializeField]
    private Transform content;
    //[SerializeField]
    //private float spaicingAngle, radius, totalAngle;
    //private float spaicingInternal;
    private void Start()
    {
        ViewField.SetActive(false);
        SpecialCards = GameHandler.GetComponent<SpecialCardsList>();
    }

    public void Display(List<string> Cards)
    {
        foreach(Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }

        Debug.Log(Cards);
        ViewField.SetActive(true);
        Cards.ForEach(c =>
        {
            Debug.Log(c);
            Debug.Log(SpecialCards.SpecialCardsUi[c]);
           GameObject card= Instantiate(SpecialCards.SpecialCardsUi[c], content);
            Debug.Log(card);
            card.transform.localScale = Vector3.one;
        });


    }

    public void EndDisplay()
    {
        ViewField.SetActive(false);
    }

    //TvT
    //public void Draw(List<string> cards)
    //{
    //    float spaicing = 0;
    //    float startDeg = 0;
    //    GameObject card;
    //    Vector3 position;

    //    //clears current cards
    //    if (transform.childCount > 0)
    //    {
    //        foreach (Transform child in transform)
    //        {
    //            Destroy(child.gameObject);
    //        }
    //    }
    //    if ((cards.Count - 1) * spaicingAngle <= totalAngle)
    //    {
    //        spaicingInternal = spaicingAngle;
    //        spaicing = (totalAngle - ((cards.Count - 1) * spaicingInternal)) / 2;
    //        startDeg = -totalAngle/2+spaicing;
    //    }
    //    else { spaicingInternal = totalAngle / cards.Count; startDeg =-totalAngle/2; }
    //        cards.ForEach(c =>
    //        {
    //                position = new Vector3(
    //                    transform.position.x + (Mathf.Sin(Mathf.Deg2Rad * startDeg) * radius),
    //                    transform.position.y + (Mathf.Cos(Mathf.Deg2Rad * startDeg) * radius),
    //                    -2
    //                    );
    //            card = Instantiate(SpecialCards.SpecialCardsUi.Where(s => s.Key.Equals(c)).First().Value, position, Quaternion.identity, transform);
    //            card.transform.up = transform.position - card.transform.position;
    //            card.transform.rotation = Quaternion.Euler(card.transform.rotation.x,-15, card.transform.rotation.z);

    //            startDeg+=spaicingInternal;
    //        });
        

        
    //}
}
