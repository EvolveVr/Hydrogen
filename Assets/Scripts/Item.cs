using UnityEngine;
using System.Collections;

/// <summary>
/// the base item class for all objects that the player will interact with
/// </summary>
public class Item : MonoBehaviour
{
    private bool _pickedUp;
    private bool _colliding;
    private string _name;
    private int _weight;
    private Transform _itemTrans;
    private Rigidbody _itemRigid;

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

    public bool pickedUp
    {
        get { return _pickedUp; }
        set
        {
            _pickedUp = value;
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
        pickedUp = true;
        _itemRigid.isKinematic = true;
        _itemTrans.SetParent(parentToChild);
        _itemTrans.position = new Vector3(parentToChild.position.x, parentToChild.position.y, parentToChild.position.z);
        transform.eulerAngles = new Vector3(parentToChild.eulerAngles.x, parentToChild.eulerAngles.y, -parentToChild.eulerAngles.z);
    }

    public virtual void drop()
    {
        pickedUp = false;
        _itemRigid.isKinematic = false;
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
    }
    
    void OnTriggerEnter(Collider enteringObject)
    {
        if (pickedUp) { return; }
        if (enteringObject.GetComponent<PlayerController>() != null)
        {
            colliding = true;
            /*
            TODO: FOR NOW, ITEMS WILL BE COLLECTED AS SOON AS YOU RUN INTO THEM
            WE NEED TO MAKE IT SO WHEN TRIGGER CLICKED AND ITEM IS CURRENTLY COLLIDING - THEN PICK UP
            VIBRATE CONTROLLER
            */
            pickUp(enteringObject.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        colliding = false;
    }
}