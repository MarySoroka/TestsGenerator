using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestsGenerator;

namespace TestsGeneratorConsole.Data
{
    public class Pipeline
    {
        private readonly TestGenerator _generator = new TestGenerator();

        private readonly PipelineConfiguration _pipelineConfiguration;

        public Pipeline(PipelineConfiguration pipelineConfiguration)
        {
            _pipelineConfiguration = pipelineConfiguration;
        }

        public async Task Processing(IEnumerable<string> files, string outputDirectory)
        {
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = true};

            var readingBlock = new TransformBlock<string, string>(
                async path => await File.ReadAllTextAsync(path),
                new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = _pipelineConfiguration.MaxReadingTasks});


            var processingBlock = new TransformManyBlock<string, TestClassInfo>(
                async code => await Task.Run(() => TestGenerator.Generate(code)),
                new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = _pipelineConfiguration.MaxTasks});

            var writingBlock = new ActionBlock<TestClassInfo>(
                async fI => await File.WriteAllTextAsync(outputDirectory + fI.TestClassName + ".txt", fI.TestClassCode),
                new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = _pipelineConfiguration.MaxWritingTasks});


            readingBlock.LinkTo(processingBlock, linkOptions);
            processingBlock.LinkTo(writingBlock, linkOptions);

            foreach (var file in files)
            {
                readingBlock.Post(file);
            }

            readingBlock.Complete();

            await writingBlock.Completion;
        }
    }
}