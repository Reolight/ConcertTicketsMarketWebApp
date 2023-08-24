namespace SorterByCriteria.DI;

/// <summary>
/// InspectionType defines the way of object inspection.
/// <code>Properties:</code> all public properties with getters will be used. Use IgnoreDescriptionAttribute to ignore unnecessary properties.
/// <code>Attributes:</code> all public properties with getters and UseDescriptionAttribute will be used.
/// </summary>
public enum InspectionType
{
    Properties,
    Attributes
}