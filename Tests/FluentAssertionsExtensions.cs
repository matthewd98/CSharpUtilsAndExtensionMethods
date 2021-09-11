using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;

namespace CSharpUtilsAndExtensionMethods.Tests
{
    public static class FluentAssertionsExtensions
    {
        public static AndConstraint<PropertyInfoAssertions> HasAttributeCount(this PropertyInfoAssertions propertyInfoAssertion, int expectedAttributeCount)
        {
            var attributesExcludingCompilerAdded = propertyInfoAssertion.Subject.GetCustomAttributes(false).Where(ca => !ca.GetType().FullName.Contains("System.Runtime.CompilerServices")).ToList();
            var actualAttributesCount = attributesExcludingCompilerAdded.Count;

            Execute.Assertion
                .ForCondition(actualAttributesCount == expectedAttributeCount)
                .FailWith($"Expected property {propertyInfoAssertion.Subject.Name} to be decorated with {expectedAttributeCount} custom attributes. Actual count: {actualAttributesCount}");

            return new AndConstraint<PropertyInfoAssertions>(propertyInfoAssertion);
        }
    }
}
