using System.Collections;
using System.Collections.Generic;

namespace WebApi.Test.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["en"];
        yield return ["pt-BR"];
        yield return ["fr"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}