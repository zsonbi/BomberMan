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
        if (Placed)
        {
            BombTimer += Time.deltaTime;
            if (bombBlowingUp)
            {
                if (BombTimer >= explosionSpreadTime)
                {
                    ++currentRange;
                    ExpandExplosion();
                    
                    BombTimer = 0f;
                }
                if (currentRange == BlastRadius)
                {
                    BlownUp();
                }
            }
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
            if (!ongoingExplosions[i])
            {
                continue;
            }

            Vector3 newPosition=Vector3.zero;
            Vector3 newScale=Vector3.zero;
            Obstacle cell=null;
            switch (i)
            {
                case Direction.Left:
                    cell = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col - 1];
                    break;
                case Direction.Up:
                    cell = GameBoard.Cells[CurrentBoardPos.Row-1, CurrentBoardPos.Col];
                    break;
                case Direction.Right:
                    cell = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col+1];
                    break;
                case Direction.Down:
                    cell = GameBoard.Cells[CurrentBoardPos.Row+1, CurrentBoardPos.Col];
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }

            if (cell.Placed)
            {
                if (cell.Destructible)
                {

                }
                else
                {
                    ongoingExplosions[i]=false;
                    blowUpVisuals[(int)i].SetActive(false);
                    Debug.Log("disaappear");
                    continue;
                }
            }
            else
            {
                switch (i)
                {
                    case Direction.Left:
                        newPosition = new Vector3(-0.5f * currentRange, 0);
                        newScale = new Vector3(1 * (currentRange + 1), 1);
                        break;
                    case Direction.Up:
                        newPosition = new Vector3(0,+0.5f * currentRange);
                        newScale = new Vector3(1, 1 * (currentRange + 1));
                        break;
                    case Direction.Right:
                        newPosition = new Vector3(0.5f * currentRange, 0);
                        newScale = new Vector3(1 * (currentRange + 1), 1);
                        break;
                    case Direction.Down:
                        newPosition = new Vector3(0,-0.5f * currentRange);
                        newScale = new Vector3(1, 1 * (currentRange+1));
                        break;
       
                }
            }
            Debug.Log("Expanded");

            blowUpVisuals[(int)i].transform.localPosition = newPosition;
            blowUpVisuals[(int)i].transform.localScale = newScale;

        }

    }

    //When the bomb blow up process completed
    private void BlownUp()
    {
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
        this.BlastRadius = radius;
        this.bombBlowingUp = false;
        this.Placed = true;
        this.explosionSpreadTime = (float)Config.BOMBEXPLOSIONSPREADSPEED / (float)radius;
        this.gameObject.SetActive(true);
        this.currentRange=0;
        this.CurrentBoardPos = whereToPlace;
        this.gameObject.transform.localPosition = new Vector3(whereToPlace.Col * Config.CELLSIZE, -2.5f - whereToPlace.Row * Config.CELLSIZE, 1);

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


    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
        //throw new System.NotImplementedException();
    }
}