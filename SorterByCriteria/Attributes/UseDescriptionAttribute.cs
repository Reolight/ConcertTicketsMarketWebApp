using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SorterByCriteria.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class UseDescriptionAttribute : Attribute { }