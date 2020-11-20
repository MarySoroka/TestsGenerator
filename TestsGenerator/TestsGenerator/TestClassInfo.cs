namespace TestsGenerator
{
    public class TestClassInfo
    {
        public string TestClassName { get; set; }
        public string TestClassCode { get; set; }

        public TestClassInfo(string name, string code)
        {
            TestClassName = name;
            TestClassCode = code;
        }
    }
}