using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class SimulationControl : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile alive_cell_tile;
    [SerializeField] private float sims_gap;
    [SerializeField] private bool is_simulating;
    [SerializeField] private bool is_random_start;
    [SerializeField] private float random_cell_probability;
    private float time_passed;

    private HashSet<Vector2Int> alive_cells;
    private HashSet<Vector2Int> frozen_alive_cells;
    private Dictionary<Vector2Int, bool> was_updated;

    private InputAction button_pause;
    private InputAction button_speed_up;
    private InputAction button_speed_down;

    [SerializeField] public TMP_Text sim_speed_text;

    // Start is called before the first frame update
    void Start()
    {
        is_simulating = true;
        time_passed = 0;
        alive_cells = new HashSet<Vector2Int>();
        was_updated = new Dictionary<Vector2Int, bool>();

        button_pause = new InputAction(binding: "<Keyboard>/space");
        button_pause.performed += _ => Pause();
        button_pause.Enable();

        button_speed_up = new InputAction(binding: "<Keyboard>/d");
        button_speed_up.performed += _ => ChangeSimulationSpeed(true);
        button_speed_up.Enable();

        button_speed_down = new InputAction(binding: "<Keyboard>/a");
        button_speed_down.performed += _ => ChangeSimulationSpeed(false);
        button_speed_down.Enable();

        UpdateSimSpeedUI();

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
        CheckMouseClicks();

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

    void Pause()
    {
        is_simulating = !is_simulating;
        UpdateSimSpeedUI();
    }

    void ChangeSimulationSpeed(bool is_speed_up)
    {
        float low_limit = 0.125f;
        if (is_speed_up)
        {
            if (sims_gap <= low_limit)
            {
                sims_gap = 0;
            } else
            {
                sims_gap /= 2;
            }
        } else
        {
            if (sims_gap < low_limit)
            {
                sims_gap = low_limit;
            } else if (sims_gap <= 1)
            {
                sims_gap *= 2;
            }
        }
        UpdateSimSpeedUI();
    }

    void UpdateSimSpeedUI()
    {
        float eps = 1e-9f;
        if (!is_simulating) {
            sim_speed_text.text = "Simulation is paused";
        } else if (sims_gap < eps)
        {
            sim_speed_text.text = "Simulation speed: as fast as possible";
        } else
        {
            float sims_in_second = 1 / sims_gap;
            sim_speed_text.text = $"Simulation speed: {sims_in_second} simulations in second";
        }
    }

    void CheckMouseClicks()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int v3cell = tilemap.WorldToCell(mouse_world_pos);
            InvertCell(new Vector2Int(v3cell.x, v3cell.y));
        }
    }

    public void SpawnPattern(HashSet<Vector2Int> pattern)
    {
        Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int v3cell = tilemap.WorldToCell(mouse_world_pos);
        Vector2Int mouse_cell = new Vector2Int(v3cell.x, v3cell.y);
        foreach (Vector2Int cell_shift in pattern)
        {
            BornCell(mouse_cell + cell_shift);
        }
    }

    void SimStep()
    {
        was_updated.Clear();
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

    void InvertCell(Vector2Int cell)
    {
        if (alive_cells.Contains(cell))
        {
            KillCell(cell);
        } else
        {
            BornCell(cell);
        }
    }
    void BornCell(Vector2Int cell)
    {
        alive_cells.Add(cell);
        Vector3Int v3cell = new Vector3Int(cell.x, cell.y, 0);
        tilemap.SetTile(v3cell, alive_cell_tile);
    }
    void KillCell(Vector2Int cell)
    {
        alive_cells.Remove(cell);
        Vector3Int v3cell = new Vector3Int(cell.x, cell.y, 0);
        tilemap.SetTile(v3cell, null);
    }
}
