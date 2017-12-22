using UnityEngine;
using System.Collections;

public class AIScript_DavidGreene : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds; //how fast the bomb is moving
    public float[] buttonCooldowns; //button cool down
    public float playerSpeed; //how fast the player is moving
    public int[] beltDirections; //which way the belt is moving?
    public float[] buttonLocations; //array of button locations
    public float characterLoc; //player locations
    public float opponentLoc; //opponent locations
    public float[] bombDistance; //distance between button and bomb?

    // Use this for initialization
    void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();
	}

    int targetBeltIndex = 0;

    // Update is called once per frame
    void Update() {

        //initializing
        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
        buttonLocations = mainScript.getButtonLocations();
        characterLoc = mainScript.getCharacterLocation();
        opponentLoc = mainScript.getOpponentLocation();
        bombDistance = mainScript.getBombDistances();
        bombSpeeds = mainScript.getBombSpeeds();

        //Your AI code goes here

        //to access any functions simple type mainScript.function()
        //Cluster the buttons, in groups of 3: 123, 345, 567, 678
        //See which group has a higher potiental for damage
        //find bomb locations which each of the 4 groups
        //find players location, calc the distance to each group
        //move player to the best group
        //calc the bombs location in the move and press button
        //last determine which time is shorter for the new target or bomb
        
        float minDistance = 1000;
        int buttonIndex = 0;
        float currentPoint;
        float playerTime = 0;
        float bombTime = 0;
        int bombSlot = 0;

        float[] t = mainScript.getButtonCooldowns();
        for (int i = 0; i < t.Length; i++)
        {
            //print (t [i]);
        }
        print("----------------");

        for (int i = 0; i < beltDirections.Length; i++)
        {
            currentPoint = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation()); //where character is

            //print("getBeltDirection: " + beltDirections[i]);
            //print("getBombSpeed: " + bombSpeeds[i]);

            //if beltdirection equal -1 then check bomb distance and speed if danager, 
            //check playerspeed and distance, if close move, if not find another

            if (buttonCooldowns[i] <= 0 && (beltDirections[i] == -1 || beltDirections[i] == 0))
            {
                int beltDir = beltDirections[i]; //test
                float bombDis = bombDistance[i]; //test
                float playerLoc = mainScript.getCharacterLocation(); //players location

                bombTime = (bombDistance[i] / bombSpeeds[i]); //bomb time until explodes
                //determining this early on helps when deciding to move to either the target or the bomb
                playerTime = Mathf.Abs(playerLoc - buttonLocations[i]) / playerSpeed; //player time until button for bomb
                //print ("Loc:" + i + "  Bomb: " + bombTime + "  Player:" + playerTime);

                if (beltDirections[i] == -1) //only focuses on belts that have bombs
                {
                    bombSlot = i; //saves bomb index for moving later
                    print("player time" + playerTime + "bomb" + buttonLocations[i]);
                    print("getBombDistances: " + bombDistance[i] + "button: " + buttonLocations[i]);
                    //print("bombTime: " + disPlay);
                    print("wev" + playerTime);

                    if (playerTime < bombTime) //if player has enough time
                    {
                        if (bombTime > buttonCooldowns[i]) //if bomb doesn't explode before button cools
                        {
                            if (currentPoint < minDistance)
                            {
                                buttonIndex = i; //placing index for moving
                                minDistance = currentPoint; //reset
                            }
                        }
                    }
                }
            }
        }

        targetBeltIndex = buttonIndex; //replacing old index

        //bool canMakeIt = (Mathf.Abs(playerLoc - buttonLocations[targetBelt]) / playerSpeed) + 0.35f < bombDistance[targetBelt] / bombSpeed[targetBelt];
        float disNewTarget = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[buttonIndex]) / playerSpeed; //new target time for player
        //differene between the bomb time and the new target index
        //print("disNewTarget: " + disNewTarget + "display: " + playerTime);

        if (disNewTarget < bombTime) //moving player to bomb index
        {
            if (buttonLocations[targetBeltIndex] < mainScript.getCharacterLocation()) //determine which direction to move
            {
                mainScript.moveDown(); //moving player down
                mainScript.push(); //push bombs
            }
            else if (buttonLocations[targetBeltIndex] > mainScript.getCharacterLocation()) //determine which direction to move
            {
                mainScript.moveUp(); //moving player up
                mainScript.push(); //push bombs
            }
        } else if (disNewTarget > bombTime) //moving player to new target index
        {
            if (buttonLocations[bombSlot] < mainScript.getCharacterLocation()) //determine which direction to move
            {
                mainScript.moveDown(); //moving player down
                mainScript.push(); //push bombs
            }
            else if (buttonLocations[bombSlot] > mainScript.getCharacterLocation()) //determine which direction to move
            {
                mainScript.moveUp(); //moving player up
                mainScript.push(); //push bombs
            }
        }
        
    }
    
}
