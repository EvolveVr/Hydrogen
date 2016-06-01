using UnityEngine;
using System.Collections;

/// <summary>
/// the base item class for all objects that the player will interact with
/// </summary>
public class Item : MonoBehaviour
{
    private bool _pickedUp;
    private string _name;
    private int _weight;
    private Transform _itemTrans;

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
        set { _pickedUp = value; }
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
        _itemTrans.SetParent(parentToChild);
        _itemTrans.position = new Vector3(parentToChild.position.x, parentToChild.position.y, parentToChild.position.z);
        transform.eulerAngles = new Vector3(parentToChild.eulerAngles.x, parentToChild.eulerAngles.y, -parentToChild.eulerAngles.z);
    }

    public virtual void drop()
    {

    }
    #endregion

    void Awake()
    {
        if (_itemTrans == null)
        {
            _itemTrans = GetComponent<Transform>();
        }
    }

    //trigger event
    void OnTriggerEnter(Collider enteringObject)
    {
        if (pickedUp) { return; }
        if (enteringObject.GetComponent<PlayerController>() != null)
        {
            pickUp(enteringObject.transform);
        }
    }
}