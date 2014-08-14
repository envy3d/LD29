using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class TileScript : MonoBehaviour
{
    public TileState state;
    public TileType type;
    public int proximityToMine = 0;
    public int ProximityToMine
    {
        get { return proximityToMine; }
        set
        {
            proximityToMine = value;
            UpdateHeatmap();
        }
    }

    private GameObject heatmap;
    private GameObject model;
    private Timer heatmapUpdateTimer;

    void Start()
    {
        heatmapUpdateTimer = new Timer(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().heatmapUpdateDelay, () => UpdateHeatmap());
        renderer.enabled = false;
        ChangeModel();
    }

    void Update()
    {
        heatmapUpdateTimer.Update(Time.deltaTime);
    }

    public bool QueryPassability()
    {
        switch (type)
        {
            case TileType.Blocked:
                return false;
            case TileType.Start:
                return false;
            default:
                return true;
        }
    }

    public bool Detonate()
    {
        if (type == TileType.Landmine)
        {
            type = TileType.Normal;
            state = TileState.Crater;
            ChangeModel();
            Camera.main.GetComponent<ShakeScript>().Shake();
            GameObject.Instantiate(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().explosionPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
            heatmapUpdateTimer.Restart();
            return true;
        }
        return false;
    }

    public bool Dig()
    {
        if (type == TileType.Normal || type == TileType.Golden)
        {
            if (state == TileState.Hole || state == TileState.Crater)
            {
                return false;
            }
            else if (state == TileState.Normal)
            {
                state = TileState.Hole;
                ChangeModel();
                heatmapUpdateTimer.Restart();
                return true;
            }
        }
        return false;
    }

    private void ChangeModel()
    {
        if (model != null)
            GameObject.Destroy(model);
        model = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().ChangeState(state);
        if (model != null)
        {
            model = GameObject.Instantiate(model) as GameObject;
            model.transform.parent = transform;
            model.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void UpdateHeatmap()
    {
        if (heatmap != null)
            GameObject.Destroy(heatmap);
        if (state == TileState.Crater || state == TileState.Hole)
        {
            GameObject heat = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<TilePrefabHelpScript>().GetHeatmap(proximityToMine) as GameObject;
            if (heat != null)
            {
                heatmap = GameObject.Instantiate(heat) as GameObject;
                heatmap.transform.parent = transform;
                heatmap.transform.localPosition = new Vector3(0, heatmap.transform.position.y, 0);
            }
        }
    }

}

public enum TileType
{
    Normal,
    Blocked,
    Landmine,
    Start,
    Finish,
    Golden
}

public enum TileState
{
    Blank,
    Normal,
    Obstacle,
    Hole,
    Crater
}
