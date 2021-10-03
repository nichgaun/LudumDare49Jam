using UnityEngine;

public abstract class Driver : MonoBehaviour
{
    public abstract void Claim(Car car);
    public abstract void ControlCar(out int hMove, out int vMove, out bool sprint);
}
