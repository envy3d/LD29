using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePrefabHelpScript : MonoBehaviour
{

    public GameObject[] startingPrefabs;
    public GameObject[] holePrefabs;
    public GameObject[] craterPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject explosionPrefab;
    public GameObject[] heatmaps;
    public float heatmapUpdateDelay = 0.5f;

    public GameObject ChangeState(TileState state)
    {
        switch (state)
        {
            case TileState.Normal:
                return SetLook(startingPrefabs);
            case TileState.Hole:
                return SetLook(holePrefabs);
            case TileState.Crater:
                return SetLook(craterPrefabs);
            case TileState.Obstacle:
                return SetLook(obstaclePrefabs);
            default:
                return null;
        }
    }

    public GameObject GetHeatmap(int proximityToMine)
    {
        return (proximityToMine > 0 && proximityToMine <= heatmaps.Length) ? heatmaps[proximityToMine - 1] : null;
    }

    private GameObject SetLook(GameObject[] prefabs)
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

}