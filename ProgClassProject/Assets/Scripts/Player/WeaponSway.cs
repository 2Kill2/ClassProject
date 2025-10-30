using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.05f;
    public float swaySpeed = 6f;
    public PlayerController pc;

    private Vector3 initialpos;

    private void Start()
    {
        initialpos = transform.localPosition;

        if(pc == null)
        {
            pc = GetComponent<PlayerController>();
        }
    }

    //swing the weapon when player rotates
    //halo0 weapon sway doesnt apply here because that uses
    //new input method and .lookinput which i do not use here

}
