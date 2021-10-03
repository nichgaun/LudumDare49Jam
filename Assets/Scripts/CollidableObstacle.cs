using UnityEngine;

public class CollidableObstacle : MonoBehaviour
{
    [SerializeField] float _inertia; // set in editor
    public float Inertia { get { return _inertia; } }
}