using Godot;
using System;

public partial class Main : CanvasLayer
{
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouse mouseEvent)
        {
            if (@event.IsActionPressed("Click"))
            {
                GD.Print("barf");

                Vector2 mp = mouseEvent.Position;
                Vector2 gp = mouseEvent.GlobalPosition;
                GD.Print(mp);
                var map = GetNode<TileMapLayer>("TileMapLayer");
                Vector2I v = map.LocalToMap((Vector2I)mp);
                TileData tile = map.GetCellTileData(v);
                GD.Print(tile);

                //foreach(Vector2I cell in map.GetSurroundingCells(v) ) {
                //    map.SetCell(cell, 3, new Vector2I(1, 0));
                //}

                //var c = map.GetCellSourceId(v);

                map.SetCell(v, 3, new Vector2I(1,0));
                GD.Print(v.X, v.Y); 

            }
        }
    }
}
