using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 amazonS3;
        private readonly string bucketName;

        public S3Service()
        {
            var config = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                UseHttp = true,
                ForcePathStyle = true,
                AuthenticationRegion = "us-east-2",
            };

            AWSCredentials credentials = new BasicAWSCredentials("key", "secret");
            this.amazonS3 = new AmazonS3Client(credentials, config);
            // Workaround due to for some reason appsetiings became unreachable from test solution
            this.bucketName = "morrygun-s3";
        }

        public async Task<Stream> GetFile(string key)
        {
            try
            {
                Stream fileStream;
                GetObjectResponse response = await this.amazonS3.GetObjectAsync(this.bucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    fileStream = response.ResponseStream;
                    return fileStream;
                }
                else
                {
                    Exception ex = new Exception($"File '{key}' Not Found");
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
