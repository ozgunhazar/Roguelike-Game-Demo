using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject {

    public Tile ExitTile;


    public override void Init(Vector2Int coord) {
        
        base.Init(coord);
        GameManager.Instance.BoardManager.SetCellTile(coord, ExitTile);
        
    }

    public override void PlayerEntered() {
        
        GameManager.Instance.NewLevel();
        
    }
    
}
