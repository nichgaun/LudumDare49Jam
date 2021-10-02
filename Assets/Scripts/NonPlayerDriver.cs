using UnityEngine;

public class NonPlayerDriver : Driver
{
    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
    {
        hMove = 0;
        vMove = 0;
        sprint = false;
    }
}
