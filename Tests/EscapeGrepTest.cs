using System;
using Xunit;
using Curtains.Helpers;

namespace Tests
{
    public class EscapeGrepTest
    {
        [Fact]
        public void StarsAreEscaped() =>
            Assert.Equal("\\* \\* \\* \\* \\* test", "* * * * * test".EscapeSpecialCharacterForGrep());


        [Fact]
        public void DotsAreEscaped() =>
            Assert.Equal("\\. \\. \\. \\. \\. test", ". . . . . test".EscapeSpecialCharacterForGrep());

        [Fact]
        public void BackSlashAreEscaped() =>
            Assert.Equal("1 2 3 4 5 te \\\\ st", "1 2 3 4 5 te \\ st".EscapeSpecialCharacterForGrep());

        [Fact]
        public void BracketsAreEscaped() =>
            Assert.Equal("1 2 3 4 5 test \\[test]", "1 2 3 4 5 test [test]".EscapeSpecialCharacterForGrep());

    }
}
