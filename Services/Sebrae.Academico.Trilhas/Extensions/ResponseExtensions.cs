using Nancy;

namespace Sebrae.Academico.Trilhas.Extensions
{
    public static class ResponseExtensions
    {
        public static Response FromByteArray(this IResponseFormatter formatter, byte[] body, string contentType = null)
        {
            return new ByteArrayResponse(body, contentType);
        }
    }
}