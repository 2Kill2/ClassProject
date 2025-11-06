using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // create assignable weapons list, the list will determine what dice it is
    //axe 18
    //sword 10
    //dagger 6
    //buster 20
    enum itemType { Axe, Sword, Dagger, Buster}
    itemType iType;
}
