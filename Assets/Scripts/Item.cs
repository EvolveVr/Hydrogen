using UnityEngine;
using Hydrogen;
using System.Collections;

/// <summary>
/// the base item class for all objects that the player will interact with
/// </summary>
public class Item : MonoBehaviour
{
    //item variables
    private string _name;
    private int _weight;
    private ItemType _type = new ItemType();

    //item read variables
    private bool _equipped;

    //item components
    private Transform _itemTrans;
    private Rigidbody _itemRigid;

    //the object I'm currently touching
    public PlayerController myController;
    
    #region GETTERS AND SETTERS
    public string itemName
    {
        get { return _name; }
        set { _name = value; }
    }
    public int itemWeight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public bool isControllerTouching
    {
        get { return myController == null ? false : true; }
    }

    /// <summary>
    /// Am I currently being held by a player controller
    /// </summary>
    public bool equipped
    {
        get { return _equipped; }
        set
        {
            _equipped = value;
            highlight(false);
        }
    }

    public ItemType type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }
    #endregion

    #region CONSTRUCTORS
    //empty constructor leave it
    public Item() { }

    public Item(string Name, int Weight)
    {
        itemName = Name;
        itemWeight = Weight;
    }
    #endregion

    #region BASE ITEM METHODS
    /// <summary>
    /// Picking up this Item
    /// </summary>
    /// <param name="myNewParent"></param>
    public virtual void pickUp(Transform myNewParent)
    {
        Debug.Log("PICKING UP " + itemName);
        //equipping me
        equipped = true;
        
        //setting this game object to the player controllers item refrence
        myNewParent.GetComponent<PlayerController>().item = this;

        //turning off physics for me
        _itemRigid.isKinematic = true;
        
        //setting me to the parent's postion and rotation
        _itemTrans.SetParent(myNewParent);
        _itemTrans.position = myNewParent.position;
        _itemTrans.rotation = myNewParent.rotation;
    }
    /// <summary>
    /// drop this item
    /// </summary>
    public virtual void drop()
    {
        //no longer equipped
        equipped = false;
        //turning phyiscs back on
        _itemRigid.isKinematic = false;
        //unparenting me
        _itemTrans.SetParent(null);
    }
    public virtual void highlight(bool doIt)
    {
        //ADD THE HIGHLIGHT SHADER TO THE GAME OBJECT
    }
    #endregion

    public virtual void OnEnable()
    {
        //on enable, get components if you don't already have them
        if (_itemTrans == null)
        {
            _itemTrans = GetComponent<Transform>();
        }
        if(_itemRigid == null)
        {
            _itemRigid = GetComponent<Rigidbody>();
        }

        //defaulting to false
        equipped = false;
    }

    /// <summary>
    /// ONLY CALL THIS UPDATE IF THE ITEM IS NOT EQUIPPED
    /// </summary>
    public virtual void Update()
    {
        if (!isControllerTouching) { return; }
        Debug.LogWarning("CONTROLLER IS TOUCHING AND ITEM IS NOT EQUIPPED");

        if (myController.hasItem == false)
        {
            Debug.LogWarning("DOESN NOT HAVE ITEM");
            if (myController.isTriggerClicked)
            {
                Debug.LogWarning("TRIGGER ON MY CONTROLLER IS CLICKED");
                pickUp(myController.transform);
            }
        }
    }
    
    /// <summary>
    /// tracked controller entering item
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED " + other.name);

        //if the object thats touching me is a controller
        if (other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("COLLIDING WITH A PLAYER CONTROLLER");
            myController = other.GetComponent<PlayerController>();
            highlight(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        highlight(false);
        myController = null;
    }
}