using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;

	[Header("Set in Inspector")]
	public TextAsset			deckXML;
    public TextAsset layoutXML;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutCenter;


	[Header("Set Dynamically")]
	public Deck					deck;
    public Layout layout;
    public List<CardProspector> drawPile;
    public Transform layoutAnchor;
    public CardProspector target;
    public List<CardProspector> tableau;
    public List<CardProspector> discardPile;

	void Awake(){
		S = this;
	}

	void Start() {
		deck = GetComponent<Deck> ();
		deck.InitDeck (deckXML.text);
        Deck.Shuffle(ref deck.cards); // This shuffles the deck by reference
        //Card c;
        //for (int cNum = 0; cNum < deck.cards.Count; cNum++)
        //{
          //  c = deck.cards[cNum];
            //c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
       // }
        layout = GetComponent<Layout>(); //Get the Layout component
        //layout.ReadLayout(layoutXML.text); //Pass LayoutXML to it
        drawPile = ConvertListCardsToListCardProspectors(deck.cards);
        LayoutGame();
	}
    List<CardProspector> ConvertListCardsToListCardProspectors(List<Card> lCD)
    {
        List<CardProspector> lCP = new List<CardProspector>();
        CardProspector tCP;
        foreach(Card tCD in lCD)
        {
            tCP = tCD as CardProspector;
            lCP.Add(tCP);
        }
        return (lCP);
    }
    //The Draw function will pull a single card from the drawPile and return it
    CardProspector Draw()
    {
        CardProspector cd = drawPile[0]; //Pull the 0th CardProspector
        drawPile.RemoveAt(0);
        return (cd); 
    }
    //LayoutGame() positions the intial tableau of cards, a.k.a the "mine"
    void LayoutGame()
    {
        //Create an empty GameObject to serve as an anchor for the tableau
        if(layoutAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            //^ Create an empty GameObject names _LayoutAnchor in the Hieracrchy
            layoutAnchor = tGO.transform; // Grab its transform
            layoutAnchor.transform.position = layoutCenter; //Position it
        }
        CardProspector op;
        //Follow the layout
        foreach(SlotDef tS in layout.slotDefs)
        {
            // ^ Iterate through all the SlotDefs in the layout.slotDefs as TS
            op = Draw(); // Pull a card from the top (beginning) of the drwa Pile
            op.faceUp = tS.faceUp; // Set its faceUp to the value in slotDef
            op.transform.parent = layoutAnchor; //Make its parent layoutAnchor
        }
    }

}
