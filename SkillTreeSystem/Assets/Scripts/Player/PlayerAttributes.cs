using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttributes
{
    public static int _points = 100;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jump;


    public float Jump { get => _jump; set => _jump = value; }
    public float Speed { get => _speed; set => _speed = value; }
}
