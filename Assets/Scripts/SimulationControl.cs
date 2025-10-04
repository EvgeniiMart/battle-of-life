using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimulationControl : MonoBehaviour
{
    [SerializeField] private Tilemap frozen_tilemap;
    [SerializeField] private Tilemap actual_tilemap;
    [SerializeField] private Tile alive_cell_tile;
    [SerializeField] private float sims_gap;
    [SerializeField] private bool is_simulating;
    [SerializeField] private bool is_random_start;
    [SerializeField] private float random_cell_probability;
    private float time_passed;

    private HashSet<Vector2Int> alive_cells;
    private HashSet<Vector2Int> frozen_alive_cells;
    private Dictionary<Vector2Int, bool> was_updated;

    // Start is called before the first frame update
    void Start()
    {
        is_simulating = true;
        time_passed = 0;
        alive_cells = new HashSet<Vector2Int>();
        was_updated = new Dictionary<Vector2Int, bool>();

        if (is_random_start)
        {
            for (int x = -100; x <= 100; ++x)
            {
                for (int y = -100; y <= 100; ++y)
                {
                    if (Random.Range(0.0f, 1.0f) <= random_cell_probability)
                    {
                        BornCell(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is_simulating)
        {
            time_passed += Time.deltaTime;
            if (time_passed > sims_gap)
            {
                time_passed = 0;
                SimStep();
            }
        }
    }

    void SimStep()
    {
        was_updated.Clear();
        // frozen_tilemap = actual_tilemap;
        frozen_alive_cells = new HashSet<Vector2Int>(alive_cells);

        Vector2Int observed_cell;
        foreach (Vector2Int cell in frozen_alive_cells)
        {
            for (int shift_x = -1; shift_x <= 1; ++shift_x)
            {
                for (int shift_y = -1; shift_y <= 1; ++shift_y)
                {
                    observed_cell = new Vector2Int(cell.x + shift_x, cell.y + shift_y);
                    if (!was_updated.ContainsKey(observed_cell) || !was_updated[observed_cell])
                    {
                        was_updated[observed_cell] = true;
                        UpdateCell(observed_cell);
                    }
                }
            }
        }
    }

    void UpdateCell(Vector2Int cell)
    {
        int alive_neighbours = 0;
        Vector2Int neighbour_cell;
        for (int shift_x = -1; shift_x <= 1; ++shift_x)
        {
            for (int shift_y = -1; shift_y <= 1; ++shift_y)
            {
                neighbour_cell = new Vector2Int(cell.x + shift_x, cell.y + shift_y);
                if (neighbour_cell != cell && frozen_alive_cells.Contains(neighbour_cell))
                {
                    ++alive_neighbours;
                }
            }
        }

        if (frozen_alive_cells.Contains(cell) && (alive_neighbours < 2 || alive_neighbours > 3))
        {
            KillCell(cell);
        }
        else if (!frozen_alive_cells.Contains(cell) && alive_neighbours == 3)
        {
            BornCell(cell);
        }
    }

    void BornCell(Vector2Int cell)
    {
        alive_cells.Add(cell);
        Vector3Int v3cell = new Vector3Int(cell.x, cell.y, 0);
        actual_tilemap.SetTile(v3cell, alive_cell_tile);
    }
    void KillCell(Vector2Int cell)
    {
        alive_cells.Remove(cell);
        Vector3Int v3cell = new Vector3Int(cell.x, cell.y, 0);
        actual_tilemap.SetTile(v3cell, null);
    }
}
