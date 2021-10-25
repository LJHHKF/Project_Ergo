using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopSceneManager : MonoBehaviour
{
    [Serializable]
    struct Sellable
    {
        public GameObject prefab;
        public int price;
        public int weight;
    }

    [Header("Product Registration")]
    [SerializeField] private Sellable[] cards;
    [SerializeField] private Sellable[] items;

    [Header("Object Registration")]
    [SerializeField] private GameObject detailWindow;

    //private Shop_DetailWindow detailManager;
    private List<Sellable> cards_canList = new List<Sellable>();
    private bool isCardsBought = false; 

    // Start is called before the first frame update
    void Start()
    {
        StoryTurningManager.instance.SetShopStage(true);
        //무언가 구매했으면 bought, 아니면 eyeShoping 플래그 세워야 함. 알아둘 것.

        //detailManager = detailWindow.GetComponent<Shop_DetailWindow>();
        detailWindow.SetActive(false);

        GameMaster.instance.OnStageStart();
    }

    public void BtnConfirm()
    {
        if (isCardsBought)
            CardPack.instance.SaveHadCnt();

        CardPack.instance.TempHadCntReset();
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        LoadManager.instance.LoadStoryScene();
    }

    public void Bought()
    {
        StoryTurningManager.instance.isShop_Bought = true;
    }

    public void SetCardTarget(out Card_Base _target, out int _price/*, out int _index*/)
    {
        int fullWeight = 0;
        int rand;
        GameObject tempObject = null;
        Card_Base temp1 = null;
        int temp2 = 0;
        //int temp_index = 1;

        cards_canList.Clear();

        for(int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            if (CardPack.instance.GetCardHadCnt_ID(cards[_i].prefab.GetComponent<Card_Base>().GetID(), true) >= CardPack.instance.GetMaxDuplicateNum())
                cards[_i].weight = 0;

            if (cards[_i].weight > 0)
                cards_canList.Add(cards[_i]);
        }

        for(int i = 0; i < cards_canList.Count; i++)
        {
            int _i = i;
            fullWeight += cards[_i].weight;
        }
        rand = UnityEngine.Random.Range(0, fullWeight);
        fullWeight = 0;
        for(int i = 0; i < cards_canList.Count; i++)
        {
            int _i = i;
            fullWeight += cards_canList[_i].weight;
            if(rand >= fullWeight - cards_canList[_i].weight && rand < fullWeight)
            {
                tempObject = cards_canList[_i].prefab;
                temp1 = tempObject.GetComponent<Card_Base>();
                temp2 = cards_canList[_i].price;
                CardPack.instance.TempHadCntUpDown(temp1.GetID(), true);
                break;
            }
        }

        //for(int i = 0; i < cards.Length; i++)
        //{
        //    int _i = i;
        //    if(ReferenceEquals(cards[_i], temp1))
        //    {
        //        temp_index = _i;
        //        break;
        //    }
        //}

        _target = temp1;
        _price = temp2;
        //_index = temp_index;
    }

    //public GameObject GetCardTarget_ToIndex(int index)
    //{
    //    return cards[index].prefab;
    //}

    public void SetItemTarget(out IItem _target, out int _price)
    {
        int fullWeight = 0;
        int rand;
        IItem temp1 = null;
        int temp2 = 0;

        for(int i = 0; i < items.Length; i++)
        {
            int _i = i;
            fullWeight += items[_i].weight;
        }
        rand = UnityEngine.Random.Range(0, fullWeight);
        fullWeight = 0;
        for (int i = 0; i < items.Length; i++)
        {
            int _i = i;
            fullWeight += items[_i].weight;
            if(rand >= fullWeight - items[_i].weight && rand < fullWeight)
            {
                temp1 = items[_i].prefab.GetComponent<IItem>();
                temp2 = items[_i].price;
                break;
            }
        }
        _target = temp1;
        _price = temp2;
    }
}
