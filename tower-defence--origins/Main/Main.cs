using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : CanvasLayer
{
    Sprite2D Turret;
    Sprite2D Turret2;
    //Sprite2D BadGuy;

    List<Badguy> badGuys = new List<Badguy>();
    List<Bulllet> bulllets = new List<Bulllet>();
    PackedScene badguyscene = ResourceLoader.Load<PackedScene>("res://badguy.tscn");
    PackedScene bullletscene = ResourceLoader.Load<PackedScene>("res://bulllet.tscn");

    public override void _Ready()
    {
        base._Ready();

        this.Turret = GetNode<Sprite2D>("turret");
        this.Turret2 = GetNode<Sprite2D>("turret2");

        OnSpawnTimer();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        Path2D path = GetNode<Path2D>("Path2D");
        //PathFollow2D pf = path.GetNode<PathFollow2D>("PathFollow2D");

        var pfs = GetTree().GetNodesInGroup("pfs");

        foreach (PathFollow2D pf in pfs)
        {
            pf.ProgressRatio += 0.01f * (float)delta;
        }



        var enemyNodes = GetTree().GetNodesInGroup("badguys");

        if (enemyNodes.Count == 0)
        {
            return;
        }

        var distanceSorted = from Node2D enemy in enemyNodes
                             let distance = enemy.GlobalPosition.DistanceSquaredTo(Turret.GlobalPosition)
                             orderby distance
                             select enemy;
        var distanceSorted2 = from Node2D enemy in enemyNodes
                              let distance = enemy.GlobalPosition.DistanceSquaredTo(Turret2.GlobalPosition)
                              orderby distance
                              select enemy;

        var target = distanceSorted.ToArray()[0];

        foreach (Bulllet bulll in bulllets) {
            var v = (bulll.GlobalPosition - target.GlobalPosition).Normalized();
            bulll.MoveAndCollide(v * bulll.Speed * (float)delta);
            //bulll.MoveAndCollide(target.GlobalPosition);
        }

        Turret.LookAt(target.GlobalPosition);
        Turret.RotationDegrees += 45;

        Turret2.LookAt(distanceSorted2.ToArray()[0].GlobalPosition);
        Turret2.RotationDegrees += 45;
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouse mouseEvent)
        {
            if (@event.IsActionPressed("Click"))
            {
                Vector2 mp = mouseEvent.Position;
                Vector2 gp = mouseEvent.GlobalPosition;

                TileMapLayer map = GetNode<TileMapLayer>("TileMapLayer");
                Vector2I v = map.LocalToMap((Vector2I)mp);

                TileData tile = map.GetCellTileData(v);

                foreach (Vector2I cell in map.GetSurroundingCells(v))
                {
                    map.SetCell(cell, 3, new Vector2I(1, 0));
                }

                map.SetCell(v, 3, new Vector2I(1, 0));

                int cID = map.GetCellSourceId(v);

                GD.Print(v.X, v.Y);
            }
        }
    }

    public void OnSpawnTimer()
    {
        Path2D path = GetNode<Path2D>("Path2D");
        int count = badGuys.Count + 1;

        PathFollow2D pf = new PathFollow2D();
        pf.Name = "PathFollow2D_" + count;
        pf.Loop = true;
        pf.AddToGroup("pfs");

        badGuys.Add(badguyscene.Instantiate<Badguy>());

        path.AddChild(pf);
        pf.AddChild(badGuys[^1]);
    }

    public void OnShootTimer()
    {
        if (badGuys.Count < 1) return;

        //create a bullllllllllet
        Bulllet b = bullletscene.Instantiate<Bulllet>();
        b.GlobalPosition = Turret.GlobalPosition;

        AddChild(b);

        bulllets.Add(b);
        //move bullllllllllet
        //collide with badguy
        //damage badguy
        //delete bulllllllllllllllllllllet
    }
}
