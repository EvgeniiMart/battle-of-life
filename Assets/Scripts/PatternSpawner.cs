using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] public SimulationControl simulation_control_script;
    List<HashSet<Vector2Int>> patterns;
    List<InputAction> patterns_input;

    void Start()
    {
        patterns = new List<HashSet<Vector2Int>>();
        patterns_input = new List<InputAction>();
        CreatePatterns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePatterns()
    {
        // GLIDER
        HashSet<Vector2Int> glider = new HashSet<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
        };
        patterns.Add(glider);

        InputAction glider_input = new InputAction(binding: "<Keyboard>/Z");
        patterns_input.Add(glider_input);

        // GLIDER GUN
        HashSet<Vector2Int> glider_gun = new HashSet<Vector2Int>()
        {
            new Vector2Int(2, 0),
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(3, 0),
            new Vector2Int(3, 1),
            new Vector2Int(3, -1),
            new Vector2Int(4, 2),
            new Vector2Int(4, -2),
            new Vector2Int(6, 2),
            new Vector2Int(6, 3),
            new Vector2Int(6, -2),
            new Vector2Int(6, -3),

            new Vector2Int(-1, -2),
            new Vector2Int(-2, -2),
            new Vector2Int(-2, -3),
            new Vector2Int(-2, -1),
            new Vector2Int(-3, 0),
            new Vector2Int(-3, -4),
            new Vector2Int(-4, -2),
            new Vector2Int(-5, 1),
            new Vector2Int(-5, -5),
            new Vector2Int(-6, 1),
            new Vector2Int(-6, -5),
            new Vector2Int(-7, 0),
            new Vector2Int(-7, -4),
            new Vector2Int(-8, -1),
            new Vector2Int(-8, -2),
            new Vector2Int(-8, -3),

            new Vector2Int(-17, -1),
            new Vector2Int(-17, -2),
            new Vector2Int(-18, -1),
            new Vector2Int(-18, -2),

            new Vector2Int(16, 0),
            new Vector2Int(16, 1),
            new Vector2Int(17, 0),
            new Vector2Int(17, 1),
        };
        patterns.Add(glider_gun);

        InputAction glider_gun_input = new InputAction(binding: "<Keyboard>/X");
        patterns_input.Add(glider_gun_input);

        // SPACESHIP
        HashSet<Vector2Int> spaceship = new HashSet<Vector2Int>()
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(1, -2),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, -2),

            new Vector2Int(3, -1),
            new Vector2Int(3, -3),
            new Vector2Int(4, -1),
            new Vector2Int(5, 0),
            new Vector2Int(5, -4),
            new Vector2Int(6, 1),
            new Vector2Int(6, 0),
            new Vector2Int(6, -1),
            new Vector2Int(6, -2),
            new Vector2Int(6, -4),
            new Vector2Int(7, 2),
            new Vector2Int(8, 3),
            new Vector2Int(8, 0),
            new Vector2Int(9, 3),
            new Vector2Int(9, 0),
            new Vector2Int(10, 3),
            new Vector2Int(10, -2),
            new Vector2Int(11, 2),
            new Vector2Int(11, 1),
            new Vector2Int(11, 0),
            new Vector2Int(11, -1),
            new Vector2Int(12, 1),
            new Vector2Int(13, 0),
            new Vector2Int(13, -1),
            new Vector2Int(14, -1),
            new Vector2Int(14, -4),
            new Vector2Int(15, -2),
            new Vector2Int(15, -4),

            new Vector2Int(-3, -1),
            new Vector2Int(-3, -3),
            new Vector2Int(-4, -1),
            new Vector2Int(-5, 0),
            new Vector2Int(-5, -4),
            new Vector2Int(-6, 1),
            new Vector2Int(-6, 0),
            new Vector2Int(-6, -1),
            new Vector2Int(-6, -2),
            new Vector2Int(-6, -4),
            new Vector2Int(-7, 2),
            new Vector2Int(-8, 3),
            new Vector2Int(-8, 0),
            new Vector2Int(-9, 3),
            new Vector2Int(-9, 0),
            new Vector2Int(-10, 3),
            new Vector2Int(-10, -2),
            new Vector2Int(-11, 2),
            new Vector2Int(-11, 1),
            new Vector2Int(-11, 0),
            new Vector2Int(-11, -1),
            new Vector2Int(-12, 1),
            new Vector2Int(-13, 0),
            new Vector2Int(-13, -1),
            new Vector2Int(-14, -1),
            new Vector2Int(-14, -4),
            new Vector2Int(-15, -2),
            new Vector2Int(-15, -4),
        };
        patterns.Add(spaceship);

        InputAction spaceship_input = new InputAction(binding: "<Keyboard>/C");
        patterns_input.Add(spaceship_input);

        // Patterns end

        for (int i = 0; i < patterns_input.Count; i++)
        {
            int local_index = i;
            patterns_input[i].performed += _ 
                => simulation_control_script.SpawnPattern(patterns[local_index]);
            patterns_input[i].Enable();
        }
    }
}
