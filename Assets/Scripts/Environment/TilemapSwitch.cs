using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSwitch : MonoBehaviour
{
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    private bool usingTilemap1 = true;

    public void TilemapSwitcheroo()
    {
        usingTilemap1 = !usingTilemap1;

        tilemap1.gameObject.SetActive(usingTilemap1);
        tilemap2.gameObject.SetActive(!usingTilemap1);
    }
}