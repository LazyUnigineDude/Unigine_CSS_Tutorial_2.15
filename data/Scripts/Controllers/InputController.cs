using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "9c76c6a9e81f5ff4e5160ac3b3e231464e6357ed")]
public class InputController : Component
{
    [ShowInEditor]
    private int Speed;

    private quat left, right;

    void Init()
    {
        left = new quat(0, 0, 1);
        right = new quat(0, 0, -1);
    }

    private void Update()
    {
        if (Input.IsKeyPressed(Input.KEY.W)) { node.WorldPosition = node.WorldPosition + node.GetWorldDirection(MathLib.AXIS.Y) * Game.IFps * Speed; }
        if (Input.IsKeyPressed(Input.KEY.S)) { node.WorldPosition = node.WorldPosition + node.GetWorldDirection(MathLib.AXIS.NY) * Game.IFps * Speed; }
        if (Input.IsKeyPressed(Input.KEY.A)) { node.WorldRotate(left); }
        if (Input.IsKeyPressed(Input.KEY.D)) { node.WorldRotate(right); }


        if (Input.IsMouseButtonDown(Input.MOUSE_BUTTON.LEFT)) { Log.Message("LMB Pressed\n"); }
    }
}