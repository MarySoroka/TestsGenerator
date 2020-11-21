namespace TestsGeneratorConsole.Data
{
    public class PipelineConfiguration
    {
        public int MaxReadingTasks { get; }

        public int MaxTasks { get; }

        public int MaxWritingTasks { get; }

        public PipelineConfiguration(int maxReadingTasks, int maxTasks, int maxWritingTasks)
        {
            MaxReadingTasks = maxReadingTasks;
            MaxTasks = maxTasks;
            MaxWritingTasks = maxTasks;
        }
    }
}