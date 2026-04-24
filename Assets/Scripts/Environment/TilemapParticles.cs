using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTileBurst : MonoBehaviour
{
    // [SerializeField] private Tilemap tilemap;
    // [SerializeField] private ParticleSystem particlePrefab;

    // private void OnEnable()
    // {
    //     if (tilemap == null) tilemap = GetComponent<Tilemap>();
    //     SpawnParticlesOnTiles();
    // }

    // private void SpawnParticlesOnTiles()
    // {
    //     BoundsInt bounds = tilemap.cellBounds;

    //     for (int x = bounds.xMin; x < bounds.xMax; x++)
    //     {
    //         for (int y = bounds.yMin; y < bounds.yMax; y++)
    //         {
    //             Vector3Int cellPos = new Vector3Int(x, y, 0);

    //             if (tilemap.HasTile(cellPos))
    //             {
    //                 Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);

    //                 ParticleSystem ps = Instantiate(particlePrefab, worldPos, Quaternion.identity);
    //                 ps.Play();
    //                 Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    //             }
    //         }
    //     }
    // }


    [SerializeField] private Tilemap tilemap;
    [SerializeField] private ParticleSystem particlePrefab;

    private readonly Vector3Int[] directions =
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0)
    };

    private void OnEnable()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        SpawnBorderParticles();
    }

    private void SpawnBorderParticles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                if (!tilemap.HasTile(cellPos))
                    continue;

                if (IsBorderTile(cellPos))
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);
                    ParticleSystem ps = Instantiate(particlePrefab, worldPos, Quaternion.identity);
                    ps.Play();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }
    }

    private bool IsBorderTile(Vector3Int cellPos)
    {
        foreach (var dir in directions)
        {
            if (!tilemap.HasTile(cellPos + dir))
                return true;
        }

        return false;
    }
    
}