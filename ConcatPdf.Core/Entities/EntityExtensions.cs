using ConcatPdf.Core.Implementations;
using ConcatPdf.Core.Interfaces;
using ConcatPdf.Core.Models;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace ConcatPdf.Core.Entities
{
    internal static class EntityExtensions
    {
        public static ConcatPdfJobEntity ToEntity(this ConcatPdfJob model)
        {
            if (model == null)
            {
                return null;
            }

            IJsonSerializer serializer = ContainerProvider.Current.Resolve<IJsonSerializer>();

            return new ConcatPdfJobEntity
            {
                ETag = model.ETag,
                Id = model.Id,
                InputUris = serializer.Serialize(model.InputUris),
                OutputFile = model.OutputFile,
                OutputUri = model.OutputUri,
                ProcessedDate = model.ProcessedDate,
                ProcessingAttempts = model.ProcessingAttempts,
                Status = model.Status,
                SubmittedDate = model.SubmittedDate,
                Timestamp = model.Timestamp
            };
        }

        public static ConcatPdfJob ToModel(this ConcatPdfJobEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            
            IJsonSerializer serializer = ContainerProvider.Current.Resolve<IJsonSerializer>();

            return new ConcatPdfJob
            {
                ETag = entity.ETag,
                Id = entity.Id,
                InputUris = serializer.Deserialize<List<string>>(entity.InputUris),
                OutputFile = entity.OutputFile,
                OutputUri = entity.OutputUri,
                ProcessedDate = entity.ProcessedDate,
                ProcessingAttempts = entity.ProcessingAttempts,
                Status = entity.Status,
                SubmittedDate = entity.SubmittedDate,
                Timestamp = entity.Timestamp
            };
        }
    }
}
