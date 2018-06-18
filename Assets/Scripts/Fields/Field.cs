using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour
{

    [SerializeField]protected string Name;

    public abstract void LandOn(Player player);
}
