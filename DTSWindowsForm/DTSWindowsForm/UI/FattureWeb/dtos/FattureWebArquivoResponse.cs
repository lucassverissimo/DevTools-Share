using Newtonsoft.Json;

namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public class FattureWebArquivoResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("dados")]
        public List<ContentFattureWebArquivoResponse> Dados { get; set; }
    }

    public class ContentFattureWebArquivoResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("tamanho")]
        public int Tamanho { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("aws_presigned_url")]
        public string AwsPresignedUrl { get; set; }
    }
}
