using UnityEngine;

public class BoxCastTest : MonoBehaviour {
	public float maxDistance = 1f;
	public float width = 1f;
	public float height = 1f;
	public float angle = 0f;
	public LayerMask layerMask;

	private void FixedUpdate() {
		var direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
		var origin = transform.position;
		var hit = Physics2D.BoxCastAll(origin, new Vector2(width, height), angle, direction, maxDistance, layerMask);
		// Debug.Log(hit ? hit.collider.name : "Nothing");
		Debug.Log(hit.Length);
	}

	private void OnDrawGizmos() {
		var direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
		var origin = transform.position;
		var hit = Physics2D.BoxCast(origin, new Vector2(width, height), angle, direction, maxDistance, layerMask);
		Gizmos.color = hit ? Color.red : Color.green;
		Gizmos.DrawRay(origin, direction * maxDistance);
		Gizmos.DrawWireCube(origin + direction * maxDistance, new Vector2(width, height));
	}
}