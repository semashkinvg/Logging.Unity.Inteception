using System.Text;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Logging.Unity.Interception
{
    public static class MethodInvocationExtensions
    {
        public static string CreateMethodInvocationDescription(this IMethodInvocation input)
        {
            var sbParam = new StringBuilder();
            for (int i = 0; i < input.Arguments.Count; i++)
            {
                var arg = input.Arguments[i];
                var argInfo = input.Arguments.GetParameterInfo(i);
                if (sbParam.Length > 0)
                {
                    sbParam.Append(",");
                }

                sbParam.Append($"({argInfo.ParameterType.Name}){arg?.ToString() ?? "<null>"}");
            }

            return $"{input.Target?.GetType()?.Name ?? "<null>"}.{input.MethodBase.Name}({sbParam})";
        }
    }
}
