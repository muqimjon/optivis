namespace OptiVis.Application.Interfaces;

public interface IOperatorMappingService
{
    IReadOnlySet<string> PrimaryExtensions { get; }
    string GetPrimaryExtension(string extension);
    bool IsPrimaryExtension(string extension);
}
