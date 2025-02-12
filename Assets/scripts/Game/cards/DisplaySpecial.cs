using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpecial : MonoBehaviour
{
    [SerializeField]
    private GameObject GameHandler,ViewField,ShopPrefab,CardPref;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private bool Displaydice;
    [SerializeField]
    private List<Sprite> diceShop;
    
    //[SerializeField]
    //private float spaicingAngle, radius, totalAngle;
    //private float spaicingInternal;
    private void Start()
    {
    }
    
    public void Display(List<string> Cards)
    {
        foreach(Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }

        
        ViewField.SetActive(true);
        Cards.ForEach(c =>
        {
            PlayerSPEffectDto effectMapper = GlobalData.Effects.First(e => e.name.Equals(c));

            //build card from dto and prefab
            GameObject card = Instantiate(CardPref,content);
            EffectDto cardeffect = card.GetComponent<EffectDto>();
            cardeffect.effect=effectMapper.effect;
            cardeffect.text.text = effectMapper.Description;
            cardeffect.isDice = effectMapper.isDice;
            cardeffect.permanent = effectMapper.permanent;
            cardeffect.Price = effectMapper.Price;
            cardeffect.preStand = effectMapper.preStand;
            //missing image mapping

            //sets scale depening on the card being a dice or not
            if (card.GetComponent<EffectDto>().isDice)
            {
                card.transform.localScale = new Vector3(1, 0.75f, 1);
                card.transform.GetChild(0).transform.localScale = new Vector3(1, 1.25f, 1);
            }
            else
            {
                card.transform.localScale = Vector3.one;
            }

        });


    }

    public void EndDisplay()
    {
        ViewField.SetActive(false);
    }

    public void DisplayShop()
    {
        
        foreach (Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }


        ViewField.SetActive(true);
        System.Random r = new System.Random();
        if (Displaydice)
        {
            List<PlayerSPEffectDto> dice = GlobalData.Effects.Where(e => e.isDice && !e.name.Contains("Default")).ToList();
            Debug.Log(dice.Count);
            for (int i = 0; i < 4; i++)
            {
                GameObject cShop = Instantiate(ShopPrefab, content);
                cShop.GetComponent<EffectDto>().text.text = dice[i].Description;
                cShop.GetComponent<EffectDto>().name = dice[i].name;
                cShop.GetComponent<EffectDto>().Price = dice[i].Price;
                cShop.GetComponent<Image>().sprite = diceShop[i];
                cShop.transform.localScale = Vector3.one;
            }
        }
        else
        {
            List<PlayerSPEffectDto> cards = GlobalData.Effects.Where(e=>!e.isDice).ToList();
            for (int i = 0; i < 5; i++)
            {

                int pos = r.Next(cards.Count());

                GameObject cShop = Instantiate(ShopPrefab, content);
                cShop.GetComponent<EffectDto>().text.text = cards[pos].Description;
                cShop.GetComponent<EffectDto>().name = cards[pos].name;
                cShop.GetComponent<EffectDto>().Price = cards[pos].Price;
                cShop.transform.localScale = Vector3.one;
            }
        }
        
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
