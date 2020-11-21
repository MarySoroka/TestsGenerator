using System.IO;
using System.Threading.Tasks;
using TestsGeneratorConsole.Data;

namespace TestsGeneratorConsole
{
    namespace TestGeneratorConsole
    {
        internal static class Program
        {
            private static async Task Main(string[] args)
            {
                var files = Directory.GetFiles(@"TestData\", "*.cs");

                var configure = new PipelineConfiguration(2, 2, 2);
                var pipeline = new Pipeline(configure);

                var outputDirectory = Directory.GetCurrentDirectory() + @"\TestResult\";

                await pipeline.Processing(files, outputDirectory);


            }
        }
    }
}