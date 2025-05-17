using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.Models
{
    public class Utilities
    {
        #region Media
        public async Task<string> TakePhoto()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);

                        await sourceStream.CopyToAsync(localFileStream);
                        return localFilePath;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
        public async Task<string> TakePhotoFromGallery()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                    if (photo != null)
                    {
                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);

                        await sourceStream.CopyToAsync(localFileStream);
                        return localFilePath;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
        public async Task<string> UploadProfilePhoto(string localPath)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent
            {
                { new StringContent("fileupload"), "reqtype" },
                { new ByteArrayContent(File.ReadAllBytes(localPath)), "fileToUpload", $"{Guid.NewGuid()}.jpg" }
            };

                    var response = await client.PostAsync("https://catbox.moe/user/api.php", content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
        #endregion
        public static Dictionary<string, string> ValidateModel<T>(T instance, string prefix = "") where T : class
        {
            var context = new ValidationContext(instance);
            var results = new List<ValidationResult>();
            var errors = new Dictionary<string, string>();

            // Validate current object
            Validator.TryValidateObject(instance, context, results, true);

            foreach (var result in results)
            {
                var propiedad = result.MemberNames.FirstOrDefault();
                if (!string.IsNullOrEmpty(propiedad))
                {
                    var key = string.IsNullOrEmpty(prefix) ? propiedad : $"{prefix}{propiedad}";
                    errors[key] = result.ErrorMessage;
                }
            }

            //Search for complex properties and validate recursively
            var propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

            foreach (var prop in propiedades)
            {
                var propValue = prop.GetValue(instance);
                if (propValue == null) continue;

                // Avoid primitive types and strings
                if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string) || prop.PropertyType.IsEnum)
                    continue;

                //If it is a collection, validate it individually
                if (typeof(IEnumerable<object>).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                {
                    var enumerable = (IEnumerable<object>)propValue;
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        var nestedErrors = ValidateModel(item, $"{prefix}{prop.Name}[{index}]");
                        foreach (var err in nestedErrors)
                            errors[err.Key] = err.Value;
                        index++;
                    }
                }
                else
                {
                    // Validate complex property
                    var nestedErrors = ValidateModel(propValue, string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}{prop.Name}");
                    foreach (var err in nestedErrors)
                        errors[err.Key] = err.Value;
                }
            }

            return errors;
        }
    }
}
