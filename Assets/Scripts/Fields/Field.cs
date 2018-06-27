using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour
{

    [SerializeField]protected string Name; //todo сделать что-то с именем
    public Field NextField;

    public abstract void LandOn(Player player);
}
