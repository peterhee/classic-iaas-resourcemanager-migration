namespace MIGAZ.Generator
{
    public interface ISettingsProvider
    {
        string ExecutionId { get; set;  }
        bool AllowTelemetry { get; set; }
        string UniquenessSuffix { get; set; }
        bool BuildEmpty { get; set; }
    }
}