using UnityEngine;
using System.Collections;

public interface IDebris
{
    float Volume { get; }
    float Mass { get; }
    float Density { get; set; }
}
