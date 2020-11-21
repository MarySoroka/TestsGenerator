namespace TestsGenerator
{
    public class TestClassInfo
    {
        public string TestClassName { get; }
        public string TestClassCode { get; }

        public TestClassInfo(string name, string code)
        {
            TestClassName = name;
            TestClassCode = code;
        }
    }
}