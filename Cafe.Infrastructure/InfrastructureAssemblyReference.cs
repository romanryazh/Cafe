using System.Reflection;

namespace Cafe.Infrastructure;

public static class InfrastructureAssemblyReference
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyReference).Assembly;
}