using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bomb : MapEntity
{
    /// <summary>
    /// The blow up visuals (order is Left, Up, Right, Down)
    /// </summary>
    [SerializeField]
    private List<GameObject> blowUpVisuals = new List<GameObject>();
    /// <summary>
    /// The directions where the explosion is spreading
    /// </summary>
    private Dictionary<Direction, bool> ongoingExplosions = new Dictionary<Direction, bool>();

    //Is the bomb currently being blown up
    private bool bombBlowingUp = false;
    //How far has the explosion gotten
    private int currentRange = 0;
    //How quick does the explosion spreading
    private float explosionSpreadTime = Config.BOMBEXPLOSIONSPREADSPEED;

    /// <summary>
    /// How far does the bomb is blasting
    /// </summary>
    public int BlastRadius { get; private set; }
    /// <summary>
    /// Is the bomb is currently placed and active
    /// </summary>
    public bool Placed { get; private set; }
    /// <summary>
    /// The bomb's internal timer
    /// </summary>
    public float BombTimer { get; private set; }
    /// <summary>
    /// How long does the bomb take to blow up
    /// </summary>
    public float TimeTillBlow { get; private set; } = Config.BOMBBLOWTIME;

    // Called when the script is loaded
    private void Awake()
    {
        ongoingExplosions.Add(Direction.Left, false);
        ongoingExplosions.Add(Direction.Up, false);
        ongoingExplosions.Add(Direction.Right, false);
        ongoingExplosions.Add(Direction.Down, false);
    }


    // Update is called once per frame
    private void Update()
    {
        //If the bomb is active
        if (Placed)
        {
            BombTimer += Time.deltaTime;
            //If the bomb is blowing up expand it's destruction
            if (bombBlowingUp)
            {
                
                if (BombTimer >= explosionSpreadTime)
                {
                    ++currentRange;

                    if (currentRange <= BlastRadius)
                    {
                        ExpandExplosion();

                    }
                    else
                    {
                        BlownUp();

                    }





                    BombTimer = 0f;
                }
           
            }
            //Otherwise check if it is ready to blow
            else
            {
                if (BombTimer >= TimeTillBlow)
                {
                    BlowUp();
                }
            }
        }
    }

    // Expands the bomb's explosion
    private void ExpandExplosion()
    {
      
        for (Direction i = 0; i <= Direction.Down; i++)
        {
            //Should we spread the bomb's explosion towards that direction
            if (!ongoingExplosions[i])
            {
                continue;
            }

            //Get the cell in that direction
            Vector3 newPosition = Vector3.zero;
            Vector3 newScale = Vector3.zero;
            Obstacle cell = null;
            //According to the direction adjust the transforms and get the appropiate cell of the gameboard
            switch (i)
            {
                case Direction.Left:
                    cell = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col - 1*currentRange];
                    newPosition = new Vector3(-0.5f * currentRange, 0);
                    newScale = new Vector3(1 * (currentRange + 1), 1);
                    break;
                case Direction.Up:
                    cell = GameBoard.Cells[CurrentBoardPos.Row - 1 * currentRange, CurrentBoardPos.Col];
                    newPosition = new Vector3(0, +0.5f * currentRange);
                    newScale = new Vector3(1, 1 * (currentRange + 1));
                    break;
                case Direction.Right:
                    cell = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col + 1 * currentRange];
                    newPosition = new Vector3(0.5f * currentRange, 0);
                    newScale = new Vector3(1 * (currentRange + 1), 1);
                    break;
                case Direction.Down:
                    cell = GameBoard.Cells[CurrentBoardPos.Row + 1 * currentRange, CurrentBoardPos.Col];
                    newPosition = new Vector3(0, -0.5f * currentRange);
                    newScale = new Vector3(1, 1 * (currentRange + 1));
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }
            //Based on the type of the cell spread the explosion or not
            bool spreadIt = false;
            if (cell.Placed)
            {
                //If the block can be blown up blow it up
                if (cell.Destructible)
                {
                    cell.BlowUp(true);
                    ongoingExplosions[i] = false;
                    spreadIt = true;
                }
                //Otherwise just skip this side for the rest of the bomb blow up cycle
                else
                {
                    ongoingExplosions[i] = false;
                    continue;
                }
            }
            else
            {
                spreadIt = true;
            }
            //Adjust the visuals positions
            if(spreadIt) {
                foreach (var item in this.GameBoard.Players)
                {
                    if (item.CurrentBoardPos.Equals(cell.CurrentBoardPos))
                    {
                        item.Kill();
                    }
                }
                foreach (var item in this.GameBoard.Monsters)
                {
                    if (item.CurrentBoardPos.Equals(cell.CurrentBoardPos))
                    {
                        item.Kill();
                    }
                }


                blowUpVisuals[(int)i].transform.localPosition = newPosition;
            blowUpVisuals[(int)i].transform.localScale = newScale;
            }
        }

    }

    //When the bomb blow up process completed
    private void BlownUp()
    {
        this.GameBoard.Cells[this.CurrentBoardPos.Row,this.CurrentBoardPos.Col].EraseBomb();


        Debug.Log("Bumm");
        this.gameObject.SetActive(false);
        this.Placed = false;
    }

    /// <summary>
    /// Places the bomb at the given place
    /// </summary>
    /// <param name="whereToPlace">The row and col of where to place the bomb on the board</param>
    /// <param name="radius">How far does the bomb's radius is</param>
    public void Place(Position whereToPlace, int radius)
    {
        //Reset the bomb's parameters
        this.BlastRadius = radius;
        this.bombBlowingUp = false;
        this.Placed = true;
        this.explosionSpreadTime = (float)Config.BOMBEXPLOSIONSPREADSPEED / (float)radius;
        this.gameObject.SetActive(true);
        this.currentRange = 0;
        this.CurrentBoardPos = whereToPlace;
        this.gameObject.transform.localPosition = new Vector3(whereToPlace.Col * Config.CELLSIZE, -2.5f - whereToPlace.Row * Config.CELLSIZE, 1);

        //Reset the visuals and their parameters
        for (Direction i = 0; i <= Direction.Down; i++)
        {
            blowUpVisuals[(int)i].SetActive(false);
            blowUpVisuals[(int)i].transform.localScale = Vector3.one;
            blowUpVisuals[(int)i].transform.localPosition = Vector3.zero;
            ongoingExplosions[(i)] = true;
        }

        BombTimer = 0;

        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Singnals the bomb to start blowing up
    /// </summary>
    public void BlowUp()
    {
        bombBlowingUp = true;
        BombTimer = 0f;
        for (Direction i = 0; i <= Direction.Down; i++)
        {
            blowUpVisuals[(int)i].SetActive(true);
        }

    }
}