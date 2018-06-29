using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : Ownable
{

    [SerializeField] private Railway[] otherRailways;
    
    public override int Rent()
    {
        var rent = 25;
        foreach (var railway in otherRailways)
        {
            if (railway.Owner == this.Owner && !railway.IsMortgage)
                rent *= 2;
        }

        return rent;
    }
}
