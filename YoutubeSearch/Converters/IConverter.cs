

namespace YoutubeSearch.Converters
{
    public interface IConverter<in TInput, out TOutput>
    {
        TOutput Convert(TInput model);
    }
}