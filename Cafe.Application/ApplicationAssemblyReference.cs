using System.Reflection;

namespace Cafe.Application;

public class ApplicationAssemblyReference
{
    public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
}