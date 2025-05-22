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
                Vector2 mp = mouseEvent.Position;
                Vector2 gp = mouseEvent.GlobalPosition;

                TileMapLayer map = GetNode<TileMapLayer>("TileMapLayer");
                Vector2I v = map.LocalToMap((Vector2I)mp);
                
                TileData tile = map.GetCellTileData(v);
                if (!tile.HasCustomData("IsPath"))
                {
                    tile.SetCustomData("IsPath", false);
                }

                bool foo = (bool)tile.GetCustomData("IsPath");
                if (!foo)
                {
                    map.SetCell(v, 3, new Vector2I(1, 0));
                    tile.SetCustomData("IsPath", true);
                }
                else
                {
                    map.SetCell(v, 3, new Vector2I(0, 0));
                    tile.SetCustomData("IsPath", false);
                }

                int cID = map.GetCellSourceId(v);
                
                GD.Print(v.X, v.Y); 

            }
        }
    }
}
