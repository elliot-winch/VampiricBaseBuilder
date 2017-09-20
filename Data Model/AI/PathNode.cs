using System.Collections;

public class PathNode<T> where T : INode{

	public T data;

	public PathNode<T>[] edges;

	public PathNode(T data){
		this.data = data;
	}
}
