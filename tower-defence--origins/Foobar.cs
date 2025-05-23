using Godot;
using System;

public partial class Foobar : Node2D
{
        public override void _Process(double delta)
    {
        base._Process(delta);

        Path2D path = GetNode<Path2D>("Path2D");
        PathFollow2D pf = path.GetNode<PathFollow2D>("PathFollow2D");

        GD.Print(pf);

        pf.ProgressRatio += 0.1f;
    }
}
