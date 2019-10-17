using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The SlotDef class is not a subclass of MonoBehaviour,  so it doesn't need
// a separate C# file.
[System.Serializable] //THis makes SlotDefs visible in the Unity Inspector pane
public class SlotDef
{
    public float x;
    public float y;
    public bool faceUp = false;
    public string layerName = "Default";
    public int LayerID = 0;
    public int id;
    public List<int> hiddenBy = new List<int>();
    public string type = "slot";
    public Vector2 stagger;
}

public class Layout : MonoBehaviour
{
    public PT_XMLReader xmlr; //Just like Deck, this has a PT_XMLReader
    public PT_XMLHashtable xml; //This variable is for faste xml access
    public Vector2 multiplier; // THe offset of the tableau's center
    //SlotDef references
    public List<SlotDef> slotDefs; //All the SlotDefs for Row0-Row3
    public SlotDef drawPile;
    public SlotDef discardPile;
    //This holds all of the possible names for the layers set by laterID
    public string[] sortingLayerNmes = new string[] { "Row0", "Row1", "Row2", "Row3", "Discard", "Draw" };
    // Start is called before the first frame update
    void ReadLayout(string xmlText)
    {
        xmlr = new PT_XMLReader();
        xmlr.Parse(xmlText); //The XML is parsed
        xml = xmlr.xml["xml"][0]; //And xml is set as a shortcut to the XML

        //Read in the multiplier, which sets card spacing
        multiplier.x = float.Parse(xml["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml["multiplier"][0].att("y"));

        //Read in the slots
        SlotDef tS;
        //SlotsX is used as a shortcut to all the <slot>s
        PT_XMLHashList slotsX = xml["slot"];
        for(int i = 0; i<slotsX.Count; i++)
        {
            tS = new SlotDef(); //Create a new SlotDef instance
            if (slotsX[i].HasAtt("type"))
            {
                //If this <slot> has a type attribute parse it
                tS.type = slotsX[i].att("type");

            }
            else
            {
                //If not, set its type to "Slot"; it's a card in the rows
                tS.type = "slot";
            }
            //Various attributes are parsed into numerical values
            tS.x = float.Parse(slotsX[i].att("x"));
            tS.y = float.Parse(slotsX[i].att("y"));
            tS.LayerID = int.Parse(slotsX[i].att("layer"));
            //This converts the number of the layerID into a text layerName
            tS.layerName = sortingLayerNmes[tS.LayerID];
            switch (tS.type)
            {
                //pull additional attributes based on the type of this <slot.
                case "slot":
                    tS.faceUp = (slotsX[i].att("faceup") == "1");
                    tS.id = int.Parse(slotsX[i].att("id"));
                    if (slotsX[i].HasAtt("hiddenby"))
                    {
                        string[] hiding = slotsX[i].att("hiddenby").Split(',');
                        foreach(string s in hiding)
                        {
                            tS.hiddenBy.Add(int.Parse(s));
                        }
                    }
                    slotDefs.Add(tS);
                    break;

                case "drawpile":
                    tS.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tS;
                    break;
                case "discardpile":
                    discardPile = tS;
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
