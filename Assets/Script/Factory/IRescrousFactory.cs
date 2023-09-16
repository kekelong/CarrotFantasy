using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Carrot
{
    public interface IRescrousFactory<T>
    {
        T GetSingleResources(string resourcePath);
    }
}

