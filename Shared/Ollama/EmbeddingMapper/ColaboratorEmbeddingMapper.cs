namespace Shared.Ollama.EmbeddingMapper
{
    public static class ColaboratorEmbeddingMapper
    {
        public static string ToEmbeddingContent(Colaborator colaborator)
        {
            return $"""
                    Colaborador de la empresa.
                    Nombre completo: {colaborator.Name} {colaborator.LastName}.
                    Correo electrónico: {colaborator.Email}.
                    Tipo de documento: {colaborator.DocumentType}.
                    Número de documento: {colaborator.DocumentNumber}
                   """;
        }
    }
}
