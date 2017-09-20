using System;
using System.Collections.Generic;
using UnityEngine;

public class Path  {

	static float moveLim = 1000f;

	Stack<Tile> validPath;

	public Path(Map map, Tile startTile, Tile endTile){
		// Check to see if we have a valid tile graph
		if (map.Graph == null) {
			Debug.LogError ("No graph instantiated");
		}

		// A dictionary of all valid, walkable nodes.
		Dictionary<Tile, PathNode<Tile>> nodes = map.Graph.Current;

		PathNode<Tile> start = nodes[startTile];
		PathNode<Tile> goal = nodes[endTile];

		// Make sure our start/end tiles are in the list of nodes!
		if(nodes.ContainsKey(startTile) == false) {
			Debug.LogError("Path_AStar: The starting tile isn't in the list of nodes!");
			return;
		}

		if(nodes.ContainsKey(endTile) == false) {
			Debug.LogError("Path_AStar: The ending tile isn't in the list of nodes!");
			return;
		}


		// Mostly following this pseusocode:
		// https://en.wikipedia.org/wiki/A*_search_algorithm

		List<PathNode<Tile>> ClosedSet = new List<PathNode<Tile>>();

		PriorityQueue<float, PathNode<Tile>> OpenSet = new PriorityQueue<float, PathNode<Tile>>();
		OpenSet.Enqueue(0, start);


		Dictionary<PathNode<Tile>, PathNode<Tile>> Came_From = new Dictionary<PathNode<Tile>, PathNode<Tile>>();

		Dictionary<PathNode<Tile>, float> g_score = new Dictionary<PathNode<Tile>, float>();
		foreach(PathNode<Tile> n in nodes.Values) {
			g_score[n] = Mathf.Infinity;
		}

		g_score[ start ] = 0;

		Dictionary<PathNode<Tile>, float> f_score = new Dictionary<PathNode<Tile>, float>();
		foreach(PathNode<Tile> n in nodes.Values) {
			f_score[n] = Mathf.Infinity;
		}

		f_score[ start ] = heuristic_cost_estimate( start, goal );

		while( !OpenSet.IsEmpty) {
			PathNode<Tile> current = OpenSet.Dequeue().Value;

			if(current == goal) {
				// We have reached our goal!
				// Let's convert this into an actual sequene of
				// tiles to walk on, then end this constructor function!
				ReconstructPath(Came_From, current);
				return; 
			}

			ClosedSet.Add(current);

			foreach(PathNode<Tile> neighbor in current.edges) {
				if (neighbor == null) {
					continue; //Ignore non-existent neighbour
				}

				if( ClosedSet.Contains(neighbor) == true )
					continue; // ignore this already completed neighbor

				if (neighbor.data.MoveCost >= moveLim) {
					//Ignore impassible tile
					continue; 
				}

				float tentative_g_score = g_score[current] + current.data.MoveCost;

				if(OpenSet.Contains(neighbor) && tentative_g_score >= g_score[neighbor])
					continue;

				Came_From[neighbor] = current;
				g_score[neighbor] = tentative_g_score;
				f_score[neighbor] = g_score[neighbor] + heuristic_cost_estimate(neighbor, goal);

				if(OpenSet.Contains(neighbor) == false) {
					OpenSet.Enqueue(f_score[neighbor], neighbor);
				}

			} // foreach neighbour
		} // while

		// If we reached here, it means that we've burned through the entire
		// OpenSet without ever reaching a point where current == goal.
		// This happens when there is no path from start to goal
		// (so there's a wall or missing floor or something).

		// We don't have a failure state, maybe? It's just that the
		// path list will be null.
	}

	float heuristic_cost_estimate( PathNode<Tile> a, PathNode<Tile> b ) {

		return Mathf.Sqrt(
			Mathf.Pow(a.data.X - b.data.X, 2) +
			Mathf.Pow(a.data.Y - b.data.Y, 2)
		);

	}

	void ReconstructPath(Dictionary<PathNode<Tile>, PathNode<Tile>> Came_From, PathNode<Tile> current) {
		// So at this point, current IS the goal.
		// So what we want to do is walk backwards through the Came_From
		// map, until we reach the "end" of that map...which will be
		// our starting node!
		Queue<Tile> total_path = new Queue<Tile>();
		total_path.Enqueue(current.data); // This "final" step is the path is the goal!

		while( Came_From.ContainsKey(current) ) {
			// Came_From is a map, where the
			//    key => value relation is real saying
			//    some_node => we_got_there_from_this_node

			current = Came_From[current];
			total_path.Enqueue(current.data);
		}

		// At this point, total_path is a queue that is running
		// backwards from the END tile to the START tile, so let's reverse it.

		validPath = new Stack<Tile>();

		while (total_path.Count > 0) {
			validPath.Push (total_path.Dequeue ());
		}
	}

	public Tile GetNextTile(){
		try{
			return validPath.Pop ();
		} catch (Exception e){
			return null;
		}
	}

	public bool IsNextTile(){
		try{
			if(validPath.Peek () != null){
				return true;
			} else {
				return false;
			}
		} catch (Exception e){
			return false;
		}
	}
}
