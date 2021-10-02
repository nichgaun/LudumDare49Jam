using UnityEngine;

public class PlayerDriver : Driver
{
    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
    {
        hMove = 0;
        vMove = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hMove -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            hMove += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            vMove -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            vMove += 1;
        }
        sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}
