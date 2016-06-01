using UnityEngine;
using System.Collections;

/// <summary>
/// the base item class for all objects that the player will interact with
/// </summary>
public class Item : MonoBehaviour
{
    private bool _equipped;
    private bool _colliding;
    private string _name;
    private int _weight;
    private Transform _itemTrans;
    private Rigidbody _itemRigid;

    public GameObject myCollidingObj;

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

    public bool equipped
    {
        get { return _equipped; }
        set
        {
            _equipped = value;
            highlight(false);
        }
    }

    public bool colliding
    {
        get { return _colliding; }
        set
        {
            _colliding = value;
            highlight(_colliding);
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
    public virtual void pickUp(Transform parentToChild)
    {
        Debug.Log("PICKING UP");
        equipped = true;
        parentToChild.GetComponent<PlayerController>().weapon = this.gameObject;
        _itemRigid.isKinematic = true;
        _itemTrans.SetParent(parentToChild);
        _itemTrans.position = parentToChild.position;
        _itemTrans.rotation = parentToChild.rotation;
    }

    public virtual void drop()
    {
        equipped = false;
        _itemRigid.isKinematic = false;
        //parentToChild.GetComponent<PlayerController>().myWeapon = null;
        _itemTrans.SetParent(null);
    }

    public virtual void highlight(bool doIt)
    {
        //ADD THE HIGHLIGHT SHADER TO THE GAME OBJECT
    }
    #endregion

    public virtual void Awake()
    {
        if (_itemTrans == null)
        {
            _itemTrans = GetComponent<Transform>();
        }
        if(_itemRigid == null)
        {
            _itemRigid = GetComponent<Rigidbody>();
        }
        equipped = false;
    }

    public virtual void Update()
    {
        if (!colliding) { return; }
        if (myCollidingObj.GetComponent<PlayerController>() != null)
        {
            if (myCollidingObj.GetComponent<PlayerController>().isTriggerClicked)
            {
                Debug.Log("TRIGGER IS CLICKED ");
                pickUp(myCollidingObj.transform);
            }
        }
    }
    
    void OnTriggerEnter(Collider enteringObject)
    {
        Debug.Log("ENTERED " + enteringObject.name);
        myCollidingObj = enteringObject.gameObject;
        if (myCollidingObj.GetComponent<PlayerController>() != null)
        {
            Debug.Log("COLLIDING");
            colliding = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        colliding = false;
        myCollidingObj = null;
    }
}