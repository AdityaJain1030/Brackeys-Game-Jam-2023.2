using UnityEngine;
using DeBroglie;
using DeBroglie.Topo;
using DeBroglie.Models;
using UnityEngine.Tilemaps;

public class WFC_Start : MonoBehaviour {

	public UnityEngine.Tilemaps.Tile tile;
	public UnityEngine.Tilemaps.Tile tile2;
	public Tilemap tilemap;

	public void Start()
	{
		ITopoArray<char> sample = TopoArray.Create(new[]
		{
			new[]{ '*', '*', '_', '*'},
			new[]{ '_', '_', '_', '*'},
			new[]{ '_', '*', '*', '*'},
		}, periodic: false);
		// Specify the model used for generation
		var model = new OverlappingModel(sample.ToTiles(), 3, 1, true);
		// Set the output dimensions
		var topology = new GridTopology(10, 10, periodic: false);
		// Acturally run the algorithm
		var propagator = new DeBroglie.TilePropagator(model, topology);
		var status = propagator.Run();
		if (status != DeBroglie.Resolution.Decided) Debug.LogError("Undecided");
		var output = propagator.ToValueArray<char>();
		// Display the results
		for (var y = 0; y < 10; y++)
		{
			for (var x = 0; x < 10; x++)
			{
				tilemap.SetTile(new Vector3Int(x, y, 0), output.Get(x, y) == '*' ? tile : tile2);
			}
		}

	}
}