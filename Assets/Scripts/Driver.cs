using UnityEngine;

public abstract class Driver : MonoBehaviour
{
    public abstract void ControlCar(out int hMove, out int vMove);
}
