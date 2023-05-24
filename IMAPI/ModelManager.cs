using IMAPI.Attributes;
using IMAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMAPI
{
    public static class ModelManager
    {
        public static readonly string ImageTypeRegex = "image/(jpeg|jpg|png)";

        public static void FillDefaults(TBASE Model, bool bCreated = false)
        {
            if(bCreated)
                Model.CREATED = DateTime.Now;

            Model.CHANGED = DateTime.Now;
        }

        public static void CopyModel<T1, T2>(T1 Dest, T2 Source, bool bCreated = false, bool bFillDefaults = true) where T1 : TBASE
        {
            if (Dest is null || Source is null)
                return;

            if (bFillDefaults)
                FillDefaults(Dest, bCreated);

            var properties = Dest.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<InitialOnlyAttribute>() != null && !bCreated)
                    continue;

                var sourceProp = Source.GetType().GetProperty(property.Name);
                if (sourceProp is null || sourceProp.GetType() != property.GetType())
                    continue;

                var value = sourceProp.GetValue(Source);
                if (value != null)
                    property.SetValue(Dest, value);
            }
        }

        public static async Task<TIMAGE?> GetImage(int EntityId, string EntityType, DbSet<TIMAGE> Images)
        {
            string internalEntityType = EntityType.ToUpper();

            var query = from i in Images
                        where i.ENTITYID == EntityId && i.ENTITYTYPE == internalEntityType
                        select i;

            return await query.FirstOrDefaultAsync();
        }

        public static async Task<TIMAGE?> GetImage<T>(int EntityId, DbSet<TIMAGE> Images) where T : TBASE
        {
            return await GetImage(EntityId, typeof(T).Name, Images);
        }

        public static async Task<bool> UploadImage(int EntityId, string EntityType, IFormFile File, DbSet<TIMAGE> Images)
        {
            if (!Regex.Match(File.ContentType, ImageTypeRegex).Success)
                return false;

            string trimmedContentType = File.ContentType.Replace("image/", "");

            string internalEntityType = EntityType.ToUpper();
            var query = from i in Images
                        where i.ENTITYID == EntityId && i.ENTITYTYPE == internalEntityType
                        select i;

            var image = await query.FirstOrDefaultAsync();
            if (image is null)
            {
                image = new TIMAGE();
                image.ENTITYID = EntityId;
                image.ENTITYTYPE = internalEntityType;
                image.NAME = File.FileName;
                image.TYPE = File.ContentType;
                image.DATA = new byte[File.Length];
                image.TYPE = trimmedContentType;

                var stream = File.OpenReadStream();
                await stream.ReadAsync(image.DATA);
                stream.Close();

                Images.Add(image);
            }
            else
            {
                image.NAME = File.FileName;
                image.TYPE = File.ContentType;
                image.DATA = new byte[File.Length];
                image.TYPE = trimmedContentType;

                var stream = File.OpenReadStream();
                await stream.ReadAsync(image.DATA);
                stream.Close();

                Images.Update(image);
            }

            return true;
        }

        public static async Task<bool> UploadImage<T>(int EntityId, IFormFile File, DbSet<TIMAGE> Images) where T : TBASE
        {
            return await UploadImage(EntityId, typeof(T).Name, File, Images);
        }

        public static async Task<bool> UploadImage<T>(T Entity, IFormFile File, DbSet<TIMAGE> Images) where T : TBASE
        {
            Type t = Entity.GetType();
            var p = t.GetProperty("ID")!.GetValue(Entity, null);
            int id = (int)p!;

            return await UploadImage<T>(id, File, Images);
        }
    }
}
