using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.Models
{
    public class Utilities
    {
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
    }
}
