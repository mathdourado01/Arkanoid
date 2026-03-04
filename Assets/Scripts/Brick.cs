using UnityEngine;

public enum BrickType
{
    Normal,
    ExtraLife,        // roxo
    Indestructible    // cinza
}

public class Brick : MonoBehaviour
{
    public BrickType type = BrickType.Normal;
}