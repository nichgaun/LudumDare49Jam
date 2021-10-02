using UnityEngine;

public class NonPlayerDriver : Driver
{
    public override void ControlCar(out int hMove, out int vMove)
    {
        hMove = 0;
        vMove = 0;
    }
}
