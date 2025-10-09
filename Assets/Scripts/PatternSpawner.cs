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
