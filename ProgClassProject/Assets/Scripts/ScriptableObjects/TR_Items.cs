using NUnit.Framework;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TR_Items", menuName = "Scriptable Objects/TR_Items")]
public class TR_Items : ScriptableObject
{
    [Header("Items")]
    [SerializeField] private Array[] items;

    [Header("Enemies")]
    [SerializeField] private Array[] Enemies;    
}
