using Nancy;
using System.IO;


namespace Sebrae.Academico.Trilhas.Extensions
{
    public class ByteArrayResponse : Response
    {
        public ByteArrayResponse(byte[] body, string contentType = null)
        {
            ContentType = contentType ?? "application/octet-stream";

            Contents = stream =>
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(body);
                }
            };
        }
    }
}