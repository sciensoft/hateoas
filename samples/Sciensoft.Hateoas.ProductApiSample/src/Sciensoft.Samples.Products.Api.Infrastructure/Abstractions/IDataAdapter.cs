namespace Sciensoft.Samples.Products.Api.Infrastructure.Abstractions
{
    public interface IDataAdapter<TResult>
        where TResult : class
    {
        TResult GetDataDeserialized(object data);
    }
}