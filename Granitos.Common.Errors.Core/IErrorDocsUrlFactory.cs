namespace Granitos.Common.Errors.Core;

public interface IErrorDocsUrlFactory
{
    string Create(string errorCode);
}